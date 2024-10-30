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


        [HttpPost("AgregarVentaOnline")]
        public async Task<ActionResult> AddVentaOnline([FromBody] Venta request)
        {
            request.tipoVenta = "Online";
            await _context.Venta.AddAsync(request);
            await _context.SaveChangesAsync();
            int idVentaGenerado = request.idVenta;

            return Ok(idVentaGenerado);
        }
        [HttpPost("AgregarVentaFisica")]
        public async Task<ActionResult> AddVentaFisica([FromBody] Venta request)
        {
            request.tipoVenta = "Fisica";
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


       [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVentaById(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return venta;
        }

        // Nuevo Endpoint para obtener los detalles de una venta por ID de la venta
        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetalleVentaByVentaId(int id)
        {
            var detallesVenta = await _context.DetalleVenta
                .Where(dv => dv.idVenta == id)
                .ToListAsync();

            if (detallesVenta == null || !detallesVenta.Any())
            {
                return NotFound();
            }

            return detallesVenta;
        }
    }
}