using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {

        private readonly ProyectoContext _context;
        public ProductoController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet("ListadoProductos")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetListadoProductos()
        {
            
            return await _context.Producto.ToListAsync();

        }

        [HttpPost("Agregar")]
        public async Task<ActionResult> AgregarProducto([FromBody]Producto request)
        {

            var newProducto = new Producto
            {
                idProducto = 0,
                idCategoria = request.idCategoria,
                idInventario = request.idInventario,
                nombreProducto = request.nombreProducto,
                descripcion = request.descripcion,
                precio = request.precio,
                stock = request.stock
            };

            await _context.Producto.AddAsync(newProducto);
            await _context.SaveChangesAsync();

            return Ok(newProducto);

        }


        [HttpPost("Modificar")]
        public async Task<ActionResult> ModificarProducto(int id, [FromBody] Producto request)
        {

            var productoModificar = await _context.Producto.FindAsync(id);
            
            if (productoModificar == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            productoModificar.nombreProducto = request.nombreProducto;
            productoModificar.descripcion = request.descripcion;
            productoModificar.precio = request.precio;
            productoModificar.stock = request.stock;
            productoModificar.idInventario = request.idInventario;
            productoModificar.idCategoria = request.idCategoria;
            

            await _context.SaveChangesAsync();

            return Ok(request);

        }

        [HttpGet("FiltrarProductos")]
        public async Task<ActionResult<IEnumerable<Producto>>> FiltrarProductos(
            [FromQuery] string term = null)
        {
            
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Producto.ToListAsync();
            }

            var searchTerm = term.ToLower();

            
            var productos = await _context.Producto
                .Where(p => p.nombreProducto.ToLower().Contains(searchTerm) || p.idProducto.ToString().Contains(searchTerm))  
                .ToListAsync();

            return productos;
        }




    }
}
