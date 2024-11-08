using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MermaController : ControllerBase
    {
        private readonly ProyectoContext _context;
        private readonly ILogger<MermaController> _logger;

        
        public MermaController(ProyectoContext context, ILogger<MermaController> logger)
        {
            _context = context;
            _logger = logger;
        }




        [HttpGet("listaMermas")]
        public async Task<ActionResult<IEnumerable<Merma>>> GetMermas()
        {
            try
            {
                _logger.LogInformation("Inicio de la consulta de mermas con fecha mayor a una semana.");

                var fechaLimite = DateTime.Now.AddDays(-7);
                _logger.LogInformation("Fecha límite para la consulta: {FechaLimite}", fechaLimite);

                var mermaContext = await _context.Merma
                    .Where(m => m.fechaMerma >= fechaLimite)
                    .ToListAsync();

                if (mermaContext == null || !mermaContext.Any())
                {
                    _logger.LogWarning("No se encontraron mermas en la base de datos después de {FechaLimite}.", fechaLimite);
                    return NotFound("No se encontraron mermas.");
                }

                // Convertimos cada entidad Merma a MermaDTO
                var mermas = mermaContext.Select(m => new Merma
                {
                    IdMerma = m.IdMerma,
                    Nombre = m.Nombre,
                    fechaMerma = m.fechaMerma,
                    cantidad = m.cantidad,
                    unidadMedida = m.unidadMedida,
                    causa = m.causa,
                    comentarios = m.comentarios,
                    idUsuario = m.idUsuario,
                    idMateria = m.idMateria
                }).ToList();

                _logger.LogInformation("Se obtuvieron {NumeroMermas} mermas.", mermas.Count);

                return Ok(mermas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las mermas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        [HttpGet("filtrarMermas")]
        public async Task<ActionResult<IEnumerable<Merma>>> FiltrarMermas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation("Inicio de la consulta de mermas entre {FechaInicio} y {FechaFin}.", fechaInicio, fechaFin);

                // Validación de fechas
                if (fechaInicio > fechaFin)
                {
                    _logger.LogWarning("La fecha de inicio ({FechaInicio}) es mayor que la fecha de fin ({FechaFin}).", fechaInicio, fechaFin);
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                fechaFin = fechaFin.AddDays(1).AddTicks(-1); // Asegura que el filtro sea hasta el final del día
                _logger.LogInformation("Fecha de fin ajustada a: {FechaFin}", fechaFin);

                // Consulta de las mermas filtradas
                var mermaContext = await _context.Merma
                    .Where(m => m.fechaMerma >= fechaInicio && m.fechaMerma <= fechaFin)
                    .ToListAsync();

                if (mermaContext == null || !mermaContext.Any())
                {
                    _logger.LogWarning("No se encontraron mermas entre {FechaInicio} y {FechaFin}.", fechaInicio, fechaFin);
                    return NotFound("No se encontraron mermas para el rango de fechas especificado.");
                }

                var mermasFiltradas = mermaContext.Select(m => new Merma
                {
                    IdMerma = m.IdMerma,
                    Nombre = m.Nombre,
                    fechaMerma = m.fechaMerma,
                    idMateria = m.idMateria,
                    cantidad = m.cantidad,
                    unidadMedida = m.unidadMedida,
                    causa = m.causa,
                    comentarios = m.comentarios,
                    idUsuario = m.idUsuario
                }).ToList();

                _logger.LogInformation("Se obtuvieron {NumeroMermas} mermas filtradas entre {FechaInicio} y {FechaFin}.", mermasFiltradas.Count, fechaInicio, fechaFin);

                return Ok(mermasFiltradas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar las mermas entre {FechaInicio} y {FechaFin}.", fechaInicio, fechaFin);
                return StatusCode(500, "Error interno del servidor.");
            }
        }




        [HttpPost("Agregar")]
        public async Task<ActionResult> AgregarMerma([FromBody] Merma request)
        {
            try
            {
                _logger.LogInformation("Iniciando la adición de una nueva merma: {MermaId} - {Nombre}.", request.IdMerma, request.Nombre);

                await _context.Merma.AddAsync(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Merma {MermaId} - {Nombre} agregada correctamente.", request.IdMerma, request.Nombre);

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la merma {MermaId} - {Nombre}.", request.IdMerma, request.Nombre);
                return StatusCode(500, "Ocurrió un error al agregar la merma.");
            }
        }



        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarMerma(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando la eliminación de la merma con ID: {MermaId}.", id);

                var merma = await _context.Merma.FindAsync(id);

                if (merma == null)
                {
                    _logger.LogWarning("Merma con ID: {MermaId} no encontrada.", id);
                    return NotFound(new { mensaje = "Merma no encontrada" });
                }

                _context.Merma.Remove(merma);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Merma con ID: {MermaId} eliminada correctamente.", id);

                return Ok(new { mensaje = "Merma eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar eliminar la merma con ID: {MermaId}.", id);
                return StatusCode(500, "Ocurrió un error al eliminar la merma.");
            }
        }






    }
}
