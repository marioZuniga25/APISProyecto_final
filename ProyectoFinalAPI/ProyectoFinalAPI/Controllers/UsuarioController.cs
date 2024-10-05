using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;
using System.Threading;

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


    }

    

}

