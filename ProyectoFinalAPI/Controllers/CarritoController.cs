using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ProyectoContext _context;
        private readonly IServiceProvider _serviceProvider;

        public CarritoController(ProyectoContext context, IServiceProvider serviceProvider, EmailService emailService)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        [HttpPost("{idUsuario}/agregar")]
        public IActionResult AgregarAlCarrito(int idUsuario, [FromBody] DetalleCarrito detalle)
        {
            var carrito = _context.Carrito.FirstOrDefault(c => c.IdUsuario == idUsuario);
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    IdUsuario = idUsuario,
                    FechaCreacion = DateTime.Now
                };
                _context.Carrito.Add(carrito);
                _context.SaveChanges();
            }
            var productoEnCarrito = _context.DetalleCarrito
                .FirstOrDefault(dc => dc.IdCarrito == carrito.IdCarrito && dc.IdProducto == detalle.IdProducto);

            if (productoEnCarrito != null)
            {
                productoEnCarrito.Cantidad += detalle.Cantidad;
            }
            else
            {
                var nuevoDetalle = new DetalleCarrito
                {
                    IdCarrito = carrito.IdCarrito,
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    FechaAgregado = detalle.FechaAgregado ?? DateTime.Now
                };
                _context.DetalleCarrito.Add(nuevoDetalle);
            }

            _context.SaveChanges();
            return Ok(new { Message = "Producto agregado correctamente" });
        }
       [HttpGet("{idUsuario}/carrito")]
        public IActionResult ObtenerCarrito(int idUsuario)
        {
            var carrito = _context.Carrito
                .FirstOrDefault(c => c.IdUsuario == idUsuario);

            if (carrito == null)
            {
                return Ok(new { Message = "Debes iniciar sesión para ver tu carrito." });
            }
            var detalles = _context.DetalleCarrito
                .Where(dc => dc.IdCarrito == carrito.IdCarrito)
                .Select(dc => new DetalleCarritoDTO
                {
                    IdDetalleCarrito = dc.IdDetalleCarrito,
                    idProducto = dc.IdProducto,
                    nombreProducto = _context.Producto.FirstOrDefault(p => p.idProducto == dc.IdProducto).nombreProducto,
                    cantidad = dc.Cantidad,
                    precioUnitario = dc.PrecioUnitario,
                    EnPromocion = _context.Producto.FirstOrDefault(p => p.idProducto == dc.IdProducto).EnPromocion,
                    imagen = _context.Producto.FirstOrDefault(p => p.idProducto == dc.IdProducto).imagen,
                    descripcion = _context.Producto.FirstOrDefault(p => p.idProducto == dc.IdProducto).descripcion,
                    fechaAgregado = dc.FechaAgregado ?? DateTime.Now
                }).ToList();
            if (!detalles.Any())
            {
                return Ok(new 
                { 
                    Message = "No tienes productos agregados en el carrito.",
                    Carrito = carrito
                });
            }
            var usuario = _context.Usuario
                .Where(u => u.idUsuario == idUsuario)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.idUsuario,
                    NombreUsuario = u.nombreUsuario,
                    Correo = u.correo
                }).FirstOrDefault();
            var carritoDTO = new CarritoDTO
            {
                IdCarrito = carrito.IdCarrito,
                total = carrito.Total,
                IdUsuario = carrito.IdUsuario,
                FechaCreacion = carrito.FechaCreacion,
                Detalles = detalles
            };
            return Ok(new 
            { 
                Usuario = usuario,
                Carrito = carritoDTO
            });
        }

        [HttpPut("incrementar/{idDetalleCarrito}")]
        public IActionResult IncrementarCantidad(int idDetalleCarrito)
        {
            var detalle = _context.DetalleCarrito.Find(idDetalleCarrito);
            if (detalle == null) return NotFound();

            detalle.Cantidad++;
            _context.SaveChanges();
            return Ok(detalle);
        }

        [HttpPut("decrementar/{idDetalleCarrito}")]
        public IActionResult DecrementarCantidad(int idDetalleCarrito)
        {
            var detalle = _context.DetalleCarrito.Find(idDetalleCarrito);
            if (detalle == null) return NotFound();

            if (detalle.Cantidad > 1)
            {
                detalle.Cantidad--;
                _context.SaveChanges();
            }
            return Ok(detalle);
        }
        [HttpDelete("eliminar/{idDetalleCarrito}")]
        public IActionResult EliminarProducto(int idDetalleCarrito)
        {
            var detalle = _context.DetalleCarrito.Find(idDetalleCarrito);
            if (detalle == null) return NotFound();

            _context.DetalleCarrito.Remove(detalle);
            _context.SaveChanges();
            return Ok(new { message = "Producto eliminado del carrito" });
        }
        [HttpDelete("{idUsuario}/limpiar")]
        public IActionResult LimpiarCarrito(int idUsuario)
        {
            var carrito = _context.Carrito.FirstOrDefault(c => c.IdUsuario == idUsuario);
            
            if (carrito == null)
            {
                return NotFound(new { Message = "Carrito no encontrado para el usuario especificado." });
            }
            var detalles = _context.DetalleCarrito.Where(dc => dc.IdCarrito == carrito.IdCarrito).ToList();
            
            if (!detalles.Any())
            {
                return Ok(new { Message = "El carrito ya está vacío." });
            }

            _context.DetalleCarrito.RemoveRange(detalles);
            _context.Carrito.RemoveRange(carrito);
            _context.SaveChanges(); 

            return Ok(new { Message = "Carrito limpiado exitosamente." });
        }
       
    }
}
