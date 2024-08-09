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
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetListadoProductos()
        {
            var productos = await _context.Producto
                .Join(_context.Categorias,
                    producto => producto.idCategoria,
                    categoria => categoria.idCategoria,
                    (producto, categoria) => new ProductoDto
                    {
                        IdProducto = producto.idProducto,
                        NombreProducto = producto.nombreProducto,
                        Descripcion = producto.descripcion,
                        Precio = producto.precio,
                        Stock = producto.stock,
                        NombreCategoria = categoria.nombreCategoria,
                        IdInventario = producto.idInventario,
                        Imagen = producto.imagen
                    })
                .ToListAsync();

            return Ok(productos);
        }

        [HttpPost("Agregar")]
        public async Task<ActionResult> AgregarProducto([FromBody] Producto request)
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
        public async Task<ActionResult<IEnumerable<ProductoDto>>> FiltrarProductos([FromQuery] string term = null)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var productos = await _context.Producto
                    .Join(_context.Categorias,
                          producto => producto.idCategoria,
                          categoria => categoria.idCategoria,
                          (producto, categoria) => new ProductoDto
                          {
                              IdProducto = producto.idProducto,
                              NombreProducto = producto.nombreProducto,
                              Descripcion = producto.descripcion,
                              Precio = producto.precio,
                              Stock = producto.stock,
                              NombreCategoria = categoria.nombreCategoria,
                              IdInventario = producto.idInventario,
                              Imagen = producto.imagen
                          })
                    .ToListAsync();

                return Ok(productos);
            }

            var searchTerm = term.ToLower();

            var filteredProductos = await _context.Producto
                .Join(_context.Categorias,
                      producto => producto.idCategoria,
                      categoria => categoria.idCategoria,
                      (producto, categoria) => new { producto, categoria })
                .Where(pc => pc.producto.nombreProducto.ToLower().Contains(searchTerm) ||
                             pc.producto.idProducto.ToString().Contains(searchTerm))
                .Select(pc => new ProductoDto
                {
                    IdProducto = pc.producto.idProducto,
                    NombreProducto = pc.producto.nombreProducto,
                    Descripcion = pc.producto.descripcion,
                    Precio = pc.producto.precio,
                    Stock = pc.producto.stock,
                    NombreCategoria = pc.categoria.nombreCategoria,
                    IdInventario = pc.producto.idInventario,
                    Imagen = pc.producto.imagen
                })
                .ToListAsync();

            return Ok(filteredProductos);
        }


    }
}
