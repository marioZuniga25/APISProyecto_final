using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public CarritoController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: api/Carrito
        [HttpGet("Obtener")]
        public async Task<ActionResult<IEnumerable<CarritoItem>>> GetCarrito()
        {
            return await _context.CarritoItems.ToListAsync();
        }

        // POST: api/Carrito
        [HttpPost("Agregar")]
        public async Task<ActionResult<CarritoItem>> AddToCarrito(CarritoItem carritoItem)
        {
            var producto = await _context.Producto.FindAsync(carritoItem.productoId);

            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            if (producto.stock < carritoItem.cantidad)
            {
                return BadRequest("No hay suficiente stock disponible");
            }
            producto.stock -= carritoItem.cantidad;
            carritoItem.id = 0;
              carritoItem.imagen = producto.imagen; 
            _context.CarritoItems.Add(carritoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarrito), new { id = carritoItem.id }, carritoItem);
        }

        // PUT: api/Carrito/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarritoItem(int id, CarritoItem carritoItem)
        {
            if (id != carritoItem.id)
            {
                return BadRequest();
            }

            var existingItem = await _context.CarritoItems.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(existingItem.productoId);

            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            int stockChange = carritoItem.cantidad - existingItem.cantidad;

            if (producto.stock < stockChange)
            {
                return BadRequest("No hay suficiente stock disponible");
            }
            producto.stock -= stockChange;
            existingItem.cantidad = carritoItem.cantidad;
            existingItem.precio = carritoItem.precio;

            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carrito/{id}
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> RemoveFromCarrito(int id)
        {
            var carritoItem = await _context.CarritoItems.FindAsync(id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(carritoItem.productoId);
            if (producto != null)
            {
                producto.stock += carritoItem.cantidad;
            }

            _context.CarritoItems.Remove(carritoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // // POST: api/Carrito/ConfirmarCompra
        // [HttpPost("ConfirmarCompra")]
        // public async Task<IActionResult> ConfirmarCompra()
        // {
        //     var carritoItems = await _context.CarritoItems.ToListAsync();

        //     if (carritoItems == null || carritoItems.Count == 0)
        //     {
        //         return BadRequest("El carrito está vacío");
        //     }

        //     // Aquí es donde procesarías la compra, generarías órdenes, etc.

        //     // Vaciar el carrito después de confirmar la compra
        //     _context.CarritoItems.RemoveRange(carritoItems);
        //     await _context.SaveChangesAsync();

        //     return Ok("Compra confirmada");
        // }
    }
}
