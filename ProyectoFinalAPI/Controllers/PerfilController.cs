using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : ControllerBase
    {
        private readonly ProyectoContext _context;
        private readonly ILogger<PerfilController> _logger;

        public PerfilController(ProyectoContext context, ILogger<PerfilController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Obtener detalles de la persona junto con direcciones
        [HttpGet("Detalles/{id:int}")]
        public async Task<ActionResult<Persona>> GetDetallesPersona(int id)
        {
            try
            {
                _logger.LogInformation("Buscando los detalles de la persona con ID: {PersonaId}.", id);

                var persona = await _context.Personas
                    .Include(p => p.DireccionesEnvio)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (persona == null)
                {
                    _logger.LogWarning("Persona con ID: {PersonaId} no encontrada.", id);
                    return NotFound();
                }

                _logger.LogInformation("Detalles de la persona con ID: {PersonaId} encontrados correctamente.", id);
                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar obtener los detalles de la persona con ID: {PersonaId}.", id);
                return StatusCode(500, "Ocurrió un error al obtener los detalles de la persona.");
            }
        }


        [HttpPost("AgregarPerfil")]
        public async Task<IActionResult> AgregarPerfil([FromBody] Persona persona)
        {
            try
            {
                if (persona == null || persona.UsuarioId <= 0)
                {
                    _logger.LogWarning("Se intentó agregar un perfil con datos inválidos: {Persona}.", persona);
                    return BadRequest("Los datos del perfil no son válidos.");
                }

                _logger.LogInformation("Iniciando proceso de agregar perfil con UsuarioId: {UsuarioId}.", persona.UsuarioId);

                await _context.Personas.AddAsync(persona);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Perfil agregado correctamente con ID: {PersonaId}.", persona.Id);

                return CreatedAtAction(nameof(GetDetallesPersona), new { id = persona.Id }, persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el perfil con UsuarioId: {UsuarioId}.", persona.UsuarioId);
                return StatusCode(500, "Ocurrió un error al agregar el perfil.");
            }
        }




        // Modificar datos de perfil existente
        [HttpPut("ModificarPerfil/{id:int}")]
        public async Task<IActionResult> ModificarPerfil(int id, [FromBody] Persona persona)
        {
            try
            {
                _logger.LogInformation("Inicio de modificación del perfil con ID: {Id}.", id);

                if (id != persona.Id)
                {
                    _logger.LogWarning("El ID del perfil no coincide. ID esperado: {Id}, ID recibido: {PersonaId}.", id, persona.Id);
                    return BadRequest("El ID de la persona no coincide.");
                }

                var personaExistente = await _context.Personas.FindAsync(id);
                if (personaExistente == null)
                {
                    _logger.LogWarning("Perfil no encontrado para el ID: {Id}.", id);
                    return NotFound("Perfil no encontrado.");
                }

                // Log de la actualización
                _logger.LogInformation("Modificando perfil para el ID: {Id}.", id);

                // Actualiza los datos de perfil
                personaExistente.Nombre = persona.Nombre;
                personaExistente.Apellidos = persona.Apellidos;
                personaExistente.Telefono = persona.Telefono;
                personaExistente.Correo = persona.Correo;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Perfil modificado correctamente con ID: {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar el perfil con ID: {Id}.", id);
                return StatusCode(500, "Ocurrió un error al modificar el perfil.");
            }
        }


        // Endpoint para agregar dirección a una persona existente
        [HttpPost("AgregarDireccion/{personaId:int}")]
        public async Task<IActionResult> AgregarDireccion(int personaId, [FromBody] DireccionEnvio direccionEnvio)
        {
            try
            {
                _logger.LogInformation("Inicio de agregar dirección de envío para la persona con ID: {PersonaId}.", personaId);

                if (direccionEnvio == null)
                {
                    _logger.LogWarning("Se intentó agregar una dirección nula para la persona con ID: {PersonaId}.", personaId);
                    return BadRequest("La dirección de envío no puede ser nula.");
                }

                direccionEnvio.PersonaId = personaId; // Asigna el ID de la persona a la dirección

                _logger.LogInformation("Se asignó el ID de la persona {PersonaId} a la dirección de envío.", personaId);

                await _context.DireccionesEnvio.AddAsync(direccionEnvio);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Dirección de envío agregada correctamente para la persona con ID: {PersonaId}.", personaId);

                return CreatedAtAction(nameof(GetDetallesPersona), new { id = personaId }, direccionEnvio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la dirección de envío para la persona con ID: {PersonaId}.", personaId);
                return StatusCode(500, "Ocurrió un error al agregar la dirección de envío.");
            }
        }



        [HttpPut("ModificarDireccion/{id:int}")]
        public async Task<IActionResult> ModificarDireccion(int id, [FromBody] DireccionEnvio direccionEnvio)
        {
            try
            {
                _logger.LogInformation("Inicio de modificación de dirección de envío con ID: {DireccionId}.", id);

                if (id != direccionEnvio.Id)
                {
                    _logger.LogWarning("ID de la dirección de envío no coincide con el ID proporcionado. ID esperado: {DireccionId}, ID recibido: {DireccionEnvioId}.", id, direccionEnvio.Id);
                    return BadRequest("El ID de la dirección no coincide.");
                }

                var direccionExistente = await _context.DireccionesEnvio.FindAsync(id);
                if (direccionExistente == null)
                {
                    _logger.LogWarning("No se encontró la dirección de envío con ID: {DireccionId}.", id);
                    return NotFound("Dirección de envío no encontrada.");
                }

                // Actualizando los campos de la dirección existente
                direccionExistente.NombreDireccion = direccionEnvio.NombreDireccion;
                direccionExistente.EsPredeterminada = direccionEnvio.EsPredeterminada;
                direccionExistente.Calle = direccionEnvio.Calle;
                direccionExistente.Numero = direccionEnvio.Numero;
                direccionExistente.Colonia = direccionEnvio.Colonia;
                direccionExistente.Ciudad = direccionEnvio.Ciudad;
                direccionExistente.Estado = direccionEnvio.Estado;
                direccionExistente.CodigoPostal = direccionEnvio.CodigoPostal;

                // Guardar cambios
                await _context.SaveChangesAsync();

                _logger.LogInformation("Dirección de envío con ID: {DireccionId} modificada correctamente.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar la dirección de envío con ID: {DireccionId}.", id);
                return StatusCode(500, "Ocurrió un error al modificar la dirección.");
            }
        }




        // Eliminar dirección
        [HttpDelete("EliminarDireccion/{id:int}")]
        public async Task<IActionResult> EliminarDireccion(int id)
        {
            try
            {
                _logger.LogInformation("Inicio de la eliminación de dirección de envío con ID: {DireccionId}.", id);

                var direccion = await _context.DireccionesEnvio.FindAsync(id);
                if (direccion == null)
                {
                    _logger.LogWarning("No se encontró la dirección de envío con ID: {DireccionId}.", id);
                    return NotFound("Dirección de envío no encontrada.");
                }

                _context.DireccionesEnvio.Remove(direccion);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Dirección de envío con ID: {DireccionId} eliminada correctamente.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la dirección de envío con ID: {DireccionId}.", id);
                return StatusCode(500, "Ocurrió un error al eliminar la dirección.");
            }
        }


        // Endpoint para obtener las direcciones de una persona por ID
        [HttpGet("Direcciones/{personaId:int}")]
        public async Task<ActionResult<IEnumerable<DireccionEnvio>>> GetDireccionesPorPersona(int personaId)
        {
            try
            {
                _logger.LogInformation("Inicio de la solicitud para obtener direcciones de la persona con ID: {PersonaId}.", personaId);

                var direcciones = await _context.DireccionesEnvio
                    .Where(d => d.PersonaId == personaId)
                    .ToListAsync();

                if (direcciones == null || !direcciones.Any())
                {
                    _logger.LogWarning("No se encontraron direcciones para la persona con ID: {PersonaId}.", personaId);
                    return NotFound("No se encontraron direcciones para esta persona.");
                }

                _logger.LogInformation("Se encontraron {DireccionCount} direcciones para la persona con ID: {PersonaId}.", direcciones.Count, personaId);

                return Ok(direcciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las direcciones de la persona con ID: {PersonaId}.", personaId);
                return StatusCode(500, "Ocurrió un error al obtener las direcciones.");
            }
        }


    }
}
