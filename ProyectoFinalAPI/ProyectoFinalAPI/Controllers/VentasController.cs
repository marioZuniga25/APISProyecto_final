using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {

        private readonly ProyectoContext _context;
        public VentasController(ProyectoContext context)
        {
            _context = context;
        }




        [HttpPost("AgregarVenta")]
        public async Task<ActionResult> AddVenta([FromBody] Venta request)
        {


            await _context.Venta.AddAsync(request);
            await _context.SaveChangesAsync();
            int idVentaGenerado = request.idVenta;

            return Ok(idVentaGenerado);
        }



        [HttpPost("AgregarDetalleVenta")]
        public async Task<ActionResult> AddDetallesVenta([FromBody] List<DetalleVenta> detallesVenta)
        {
            if (detallesVenta == null || !detallesVenta.Any())
            {
                return BadRequest("La lista de detalles de venta no puede estar vacía.");
            }

            // Agregar lógica para guardar los detalles de venta
            foreach (var detalle in detallesVenta)
            {
                _context.DetalleVenta.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


       
    }
}
