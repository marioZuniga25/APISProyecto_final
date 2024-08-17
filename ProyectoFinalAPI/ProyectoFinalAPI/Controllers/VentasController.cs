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
            
                foreach (var detalle in detallesVenta)
                {
                    // Buscar el producto
                    var producto = await _context.Producto
                        .FirstOrDefaultAsync(p => p.idProducto == detalle.idProducto);

                    if (producto == null)
                    {
                        return NotFound($"El producto con ID {detalle.idProducto} no se encontró.");
                    }

                    
                    if (producto.stock < detalle.cantidad)
                    {
                        return BadRequest($"No hay suficiente stock del producto '{producto.nombreProducto}'. Stock disponible: {producto.stock}");
                    }

                    
                    producto.stock -= detalle.cantidad;

                    
                    _context.DetalleVenta.Add(detalle);
                }

                
                


            await _context.SaveChangesAsync();

            return Ok();
        }


       
    }
}