// Controllers/PromocionesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class PromocionesController : ControllerBase
 {
        private readonly ProyectoContext _context;
        private readonly ILogger<PromocionesController> _logger;

        public PromocionesController(ProyectoContext context, ILogger<PromocionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promocion>>> GetPromociones()
        {
            try
            {
                _logger.LogInformation("Inicio de la solicitud para obtener todas las promociones.");

                var promociones = await _context.Promociones.ToListAsync();

                _logger.LogInformation("Se obtuvieron {PromocionCount} promociones.", promociones.Count);

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las promociones.");
                return StatusCode(500, "Ocurrió un error al obtener las promociones.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Promocion>> CreatePromocion(Promocion promocion)
        {
            try
            {
                _logger.LogInformation("Inicio de la solicitud para crear una nueva promoción.");

                _context.Promociones.Add(promocion);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Promoción creada exitosamente con ID: {PromocionId}", promocion.IdPromocion);

                return CreatedAtAction(nameof(GetPromociones), new { id = promocion.IdPromocion }, promocion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la promoción.");
                return StatusCode(500, "Ocurrió un error al crear la promoción.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromocion(int id, Promocion promocion)
        {
            if (id != promocion.IdPromocion)
            {
                _logger.LogWarning("El ID proporcionado ({IdProporcionado}) no coincide con el ID de la promoción ({IdPromocion}).", id, promocion.IdPromocion);
                return BadRequest("El ID de la promoción no coincide con el parámetro de la solicitud.");
            }

            _context.Entry(promocion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Promoción con ID {PromocionId} actualizada exitosamente.", promocion.IdPromocion);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PromocionExists(id))
                {
                    _logger.LogWarning("Intento de actualización fallido: la promoción con ID {PromocionId} no existe.", id);
                    return NotFound();
                }

                _logger.LogError(ex, "Error de concurrencia al actualizar la promoción con ID {PromocionId}.", promocion.IdPromocion);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la promoción con ID {PromocionId}.", promocion.IdPromocion);
                return StatusCode(500, "Ocurrió un error al actualizar la promoción.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromocion(int id)
        {
            _logger.LogInformation("Solicitud para eliminar la promoción con ID {PromocionId} recibida.", id);

            var promocion = await _context.Promociones.FindAsync(id);
            if (promocion == null)
            {
                _logger.LogWarning("La promoción con ID {PromocionId} no fue encontrada.", id);
                return NotFound(new { mensaje = "Promoción no encontrada" });
            }

            try
            {
                _context.Promociones.Remove(promocion);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Promoción con ID {PromocionId} eliminada exitosamente.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar eliminar la promoción con ID {PromocionId}.", id);
                return StatusCode(500, "Ocurrió un error al eliminar la promoción.");
            }
        }


        private bool PromocionExists(int id)
        {
            _logger.LogInformation("Verificando si la promoción con ID {PromocionId} existe.", id);

            bool exists = _context.Promociones.Any(e => e.IdPromocion == id);

            if (exists)
            {
                _logger.LogInformation("La promoción con ID {PromocionId} existe.", id);
            }
            else
            {
                _logger.LogWarning("La promoción con ID {PromocionId} no existe.", id);
            }

            return exists;
        }

    }
}
