using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public PedidoController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet("GetPedidos/{filtro}")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleDto>>> GetPedidos(string filtro)
        {
            var result = await (from pedido in _context.Pedidos
                                join detalleVenta in _context.DetalleVenta on pedido.idVenta equals detalleVenta.idVenta
                                join producto in _context.Producto on detalleVenta.idProducto equals producto.idProducto
                                where string.IsNullOrEmpty(filtro) || pedido.estatus.Contains(filtro)
                                select new
                                {
                                    pedido,
                                    detalleVenta,
                                    producto
                                })
                               .GroupBy(g => new
                               {
                                   g.pedido.idPedido,
                                   g.pedido.idVenta,
                                   g.pedido.idUsuario,
                                   g.pedido.idTarjeta,
                                   g.pedido.nombre,
                                   g.pedido.apellidos,
                                   g.pedido.telefono,
                                   g.pedido.correo,
                                   g.pedido.calle,
                                   g.pedido.numero,
                                   g.pedido.colonia,
                                   g.pedido.ciudad,
                                   g.pedido.estado,
                                   g.pedido.codigoPostal,
                                   g.pedido.estatus
                               })
                               .Select(group => new PedidoDetalleDto
                               {
                                   idPedido = group.Key.idPedido,
                                   idVenta = group.Key.idVenta,
                                   idUsuario = group.Key.idUsuario,
                                   idTarjeta = group.Key.idTarjeta,
                                   nombre = group.Key.nombre,
                                   apellidos = group.Key.apellidos,
                                   telefono = group.Key.telefono,
                                   correo = group.Key.correo,
                                   calle = group.Key.calle,
                                   numero = group.Key.numero,
                                   colonia = group.Key.colonia,
                                   ciudad = group.Key.ciudad,
                                   estado = group.Key.estado,
                                   codigoPostal = group.Key.codigoPostal,
                                   estatus = group.Key.estatus,
                                   Productos = group.Select(item => new DetalleProducto
                                   {
                                       idProducto = item.producto.idProducto,
                                       nombreProducto = item.producto.nombreProducto,
                                       descripcion = item.producto.descripcion,
                                       precioUnitario = item.detalleVenta.precioUnitario,
                                       cantidad = item.detalleVenta.cantidad,
                                       imagen = item.producto.imagen
                                   }).ToList()
                               })
                               .ToListAsync();

            return result;
        }

        [HttpGet("GetPedidosPorUsuario{idUsuario}")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleDto>>> GetPedidosPorUsuario(int idUsuario)
        {
            var result = await (from pedido in _context.Pedidos
                                join detalleVenta in _context.DetalleVenta on pedido.idVenta equals detalleVenta.idVenta
                                join producto in _context.Producto on detalleVenta.idProducto equals producto.idProducto
                                join venta in _context.Venta on pedido.idVenta equals venta.idVenta
                                where pedido.idUsuario == idUsuario // Filtro por idUsuario
                                select new
                                {
                                    pedido,
                                    detalleVenta,
                                    producto,
                                    venta
                                })
                               .GroupBy(g => new
                               {
                                   g.pedido.idPedido,
                                   g.pedido.idVenta,
                                   g.pedido.nombre,
                                   g.pedido.apellidos,
                                   g.pedido.telefono,
                                   g.pedido.correo,
                                   g.pedido.calle,
                                   g.pedido.numero,
                                   g.pedido.colonia,
                                   g.pedido.ciudad,
                                   g.pedido.estado,
                                   g.pedido.codigoPostal,
                                   g.pedido.estatus,
                                   g.venta.fechaVenta
                               })
                               .OrderByDescending(group => group.Key.fechaVenta)
                               .Select(group => new PedidoDetalleDto
                               {
                                   idPedido = group.Key.idPedido,
                                   idVenta = group.Key.idVenta,
                                   nombre = group.Key.nombre,
                                   apellidos = group.Key.apellidos,
                                   telefono = group.Key.telefono,
                                   correo = group.Key.correo,
                                   calle = group.Key.calle,
                                   numero = group.Key.numero,
                                   colonia = group.Key.colonia,
                                   ciudad = group.Key.ciudad,
                                   estado = group.Key.estado,
                                   codigoPostal = group.Key.codigoPostal,
                                   estatus = group.Key.estatus,
                                   fechaVenta = group.Key.fechaVenta,
                                   Productos = group.Select(item => new DetalleProducto
                                   {
                                       idProducto = item.producto.idProducto,
                                       nombreProducto = item.producto.nombreProducto,
                                       descripcion = item.producto.descripcion,
                                       precioUnitario = item.detalleVenta.precioUnitario,
                                       cantidad = item.detalleVenta.cantidad,
                                       //imagen = item.producto.imagen
                                   }).ToList()
                               })
                               .ToListAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedidos>> GetPedidoById(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }


        // Agregar un nuevo pedido
        [HttpPost]
        public async Task<ActionResult<Pedidos>> AddPedido(Pedidos pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.idPedido }, pedido);

        }

        // Actualizar el estatus de un pedido
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, Pedidos pedido)
        {
            if (id != pedido.idPedido)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(e => e.idPedido == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
