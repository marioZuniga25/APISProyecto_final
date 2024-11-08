// Controllers/PromocionesRandomController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class PromocionesRandomController : ControllerBase
    {
        private readonly ProyectoContext _context;
        private readonly ILogger<PromocionesRandomController> _logger;

        public PromocionesRandomController(ProyectoContext context, ILogger<PromocionesRandomController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromocionesRandom>>> GetPromocionesRandom()
        {
            _logger.LogInformation("Solicitud recibida para obtener promociones aleatorias.");

            try
            {
                var promociones = await _context.PromocionesRandom.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} promociones aleatorias exitosamente.", promociones.Count);

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las promociones aleatorias.");
                return StatusCode(500, "Ocurrió un error al obtener las promociones.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PromocionesRandom>> CreatePromocionRandom(PromocionesRandom promocionesRandom)
        {
            _logger.LogInformation("Iniciando la creación de una nueva promoción random.");

            try
            {
                _context.PromocionesRandom.Add(promocionesRandom);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Promoción random creada con éxito. ID: {IdPromocionRandom}", promocionesRandom.IdPromocionRandom);

                return CreatedAtAction(nameof(GetPromocionesRandom), new { id = promocionesRandom.IdPromocionRandom }, promocionesRandom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la promoción random.");
                return StatusCode(500, "Ocurrió un error al crear la promoción random.");
            }
        }

    }
}
