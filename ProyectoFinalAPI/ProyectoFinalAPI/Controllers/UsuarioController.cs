using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;
using System.Threading;
using System.Security.Cryptography;

namespace ProyectoFinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ProyectoContext _context;
        public UsuarioController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<ActionResult> GetListadoUsuarios()
        {
            // Listar usuarios externos (type = 0) e internos (type = 1)
            var usuariosExternos = await _context.Usuario.Where(u => u.type == 0).ToListAsync();
            var usuariosInternos = await _context.Usuario.Where(u => u.type == 1).ToListAsync();

            return Ok(new
            {
                Externos = usuariosExternos,
                Internos = usuariosInternos
            });
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> SearchUsuario(string nameUsuario)
        {
            // Incluir la categoría relacionada en la consulta
            return await _context.Usuario.Where(u => u.nombreUsuario.Contains(nameUsuario)).ToListAsync();

        }


        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> addUsuario([FromBody] Usuario request)
        {
            var usuario = new Usuario
            {
                idUsuario = 0,
                nombreUsuario = request.nombreUsuario,
                correo = request.correo,
                contrasenia = request.contrasenia,
                rol = request.rol,
                type = 0,
            };

            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        // Endpoint para registrar empleados (usuarios internos)
        [HttpPost]
        [Route("registrarInterno")]
        public async Task<IActionResult> addUsuarioInterno([FromBody] Usuario request)
        {
            var usuario = new Usuario
            {
                idUsuario = 0,
                nombreUsuario = request.nombreUsuario,
                correo = request.correo,
                contrasenia = request.contrasenia,
                rol = request.rol,
                type = 1, // Usuario interno (empleado)
            };

            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return Ok(request);
        }


        [HttpPut]
        [Route("ModificarUsuario/{id:int}")]

        public async Task<IActionResult> updateUsuario(int id, [FromBody] Usuario request)
        {
            var usuarioModificar = await _context.Usuario.FindAsync(id);

            if (usuarioModificar == null)
            {
                return BadRequest("No existe el usuario");
            }

            usuarioModificar.nombreUsuario = request.nombreUsuario;
            usuarioModificar.correo = request.correo;
            usuarioModificar.contrasenia = request.contrasenia;
            usuarioModificar.rol = request.rol;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();

            }

            return Ok();
        }


        [HttpDelete]
        [Route("EliminarUsuario/{id:int}")]
        public async Task<IActionResult> deleteUsuario(int id)
        {
            var usuarioEliminar = await _context.Usuario.FindAsync(id);

            if (usuarioEliminar == null)
            {
                return BadRequest("No se encontro el usuario.");
            }
            _context.Usuario.Remove(usuarioEliminar);

            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpGet("DetalleUsuario/{id:int}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            return Ok(usuario);
        }


        //EndPoint para login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Usuario request)
        {
            // Buscar usuario por nombre de usuario o correo y contraseña
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => (u.nombreUsuario == request.nombreUsuario || u.correo == request.correo)
                                          && u.contrasenia == request.contrasenia);

            // Si no se encuentra el usuario, devolver una respuesta no autorizada
            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            // Si se encuentra el usuario, devolver una respuesta de éxito
            return Ok(new { message = "Inicio de sesión exitoso", user = usuario });
        }

        [HttpGet("BuscarPorNombre")]
        public async Task<ActionResult<IEnumerable<Usuario>>> SearchUsuariosPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre de usuario es requerido.");
            }

            var usuarios = await _context.Usuario
                .Where(u => u.nombreUsuario.Contains(nombre))
                .ToListAsync();

            return Ok(usuarios); // Devuelve 200 OK con una lista (posiblemente vacía)
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, [FromServices] EmailService emailService)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.correo == request.Correo);

            if (usuario == null)
            {
                return BadRequest(new { message = "El correo no está registrado." });
            }

            // Verificar si ya hay un token activo y no ha expirado
            if (usuario.ResetTokenExpires > DateTime.UtcNow)
            {
                return BadRequest(new { message = "Ya se ha iniciado un proceso de recuperación. Inténtalo más tarde." });
            }

            // Generar un nuevo token y establecer su fecha de expiración
            var token = GenerateResetToken();
            usuario.ResetToken = token;
            usuario.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync();

            // Preparar el correo con el template
            var resetLink = $"http://localhost:4200/reset-password?token={token}";
            var body = System.IO.File.ReadAllText("Templates/ForgotPassword.html")
                       .Replace("[LOGO_URL]", "https://i.imgur.com/EmvHFiH.png")
                       .Replace("[RESET_LINK]", resetLink);

            await emailService.SendEmailAsync(usuario.correo, "Recuperación de contraseña", body);

            return Ok(new { message = "Correo de recuperación enviado." });
        }


        private string GenerateResetToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var tokenData = new byte[32]; // Generar 32 bytes aleatorios
                rng.GetBytes(tokenData);

                // Convertir a Base64 y reemplazar caracteres problemáticos
                return Convert.ToBase64String(tokenData)
                              .Replace("+", "-")
                              .Replace("/", "_")
                              .Replace("=", ""); // Eliminar relleno
            }
        }

        [HttpGet]
        [Route("validate-token")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpires > DateTime.UtcNow);
            if (usuario == null)
            {
                return BadRequest(new { message = "El token es inválido o ha expirado." });
            }

            return Ok(true);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, [FromServices] EmailService emailService)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(
                u => u.ResetToken == request.Token && u.ResetTokenExpires > DateTime.UtcNow);

            if (usuario == null)
            {
                return BadRequest(new { message = "El token es inválido o ha expirado." });
            }

            // Actualizar la contraseña y limpiar el token
            usuario.contrasenia = request.NuevaContrasenia;
            usuario.ResetToken = null;
            usuario.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            // Preparar y enviar el correo con el template "PasswordUpdated.html"
            var body = System.IO.File.ReadAllText("Templates/PasswordUpdated.html")
                       .Replace("[LOGO_URL]", "https://i.imgur.com/EmvHFiH.png");

            await emailService.SendEmailAsync(usuario.correo, "Tu contraseña ha sido actualizada", body);

            return Ok(new { message = "Contraseña restablecida con éxito." });
        }


    }
}

