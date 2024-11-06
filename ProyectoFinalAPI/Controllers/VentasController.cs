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
    public class VentasController : ControllerBase
    {

        private readonly ProyectoContext _context;
        private readonly IServiceProvider _serviceProvider;

        public VentasController(ProyectoContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }


        [HttpPost("AgregarVentaOnline")]
        public async Task<ActionResult> AddVentaOnline([FromBody] Venta request, [FromServices] EmailService emailService)
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
        public async Task<ActionResult> AddDetallesVenta([FromBody] List<DetalleVenta> detallesVenta, [FromServices] EmailService emailService)
        {
            foreach (var detalle in detallesVenta)
            {
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

            // Llama a la función de envío de correo después de guardar los detalles
            _ = EnviarCorreoConDetallesAsync(detallesVenta.First().idVenta, emailService);

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

        [HttpGet("GetVentasByTipo/{tipoVenta}")]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasByTipo(string tipoVenta)
        {
            var ventas = await _context.Venta
                .Where(v => v.tipoVenta == tipoVenta)
                .OrderByDescending(v => v.fechaVenta)
                .Select(v => new VentaDto
                {
                    IdVenta = v.idVenta,
                    Total = (decimal)v.total,
                    FechaVenta = v.fechaVenta,
                    TipoVenta = v.tipoVenta,
                    Usuario = _context.Usuario
                        .Where(u => u.idUsuario == v.idUsuario)
                        .Select(u => new UsuarioDto
                        {
                            IdUsuario = u.idUsuario,
                            NombreUsuario = u.nombreUsuario,
                            Correo = u.correo
                        }).FirstOrDefault(),
                    DetalleVentas = _context.DetalleVenta
                        .Where(dv => dv.idVenta == v.idVenta)
                        .Select(dv => new DetalleVentaDto
                        {
                            IdDetalleVenta = dv.idDetalleVenta,
                            Cantidad = dv.cantidad,
                            PrecioUnitario = (decimal)dv.precioUnitario,
                            Producto = _context.Producto
                                .Where(p => p.idProducto == dv.idProducto)
                                .Select(p => new ProductoDto
                                {
                                    IdProducto = p.idProducto,
                                    NombreProducto = p.nombreProducto,
                                    Descripcion = p.descripcion,
                                    Precio = p.precio,
                                    Stock = p.stock,
                                    NombreCategoria = p.NombreCategoria,
                                    IdInventario = p.idInventario,
                                    IdCategoria = p.idCategoria,
                                    //Imagen = p.imagen
                                }).FirstOrDefault()
                        }).ToList()
                })
                .ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                return NotFound($"No se encontraron ventas del tipo '{tipoVenta}'.");
            }

            return Ok(ventas);
        }

        private async Task EnviarCorreoConDetallesAsync(int idVenta, EmailService emailService)
        {
            // Crear un nuevo contexto para esta función
            using var scope = _serviceProvider.CreateScope();
            var scopedContext = scope.ServiceProvider.GetRequiredService<ProyectoContext>();

            try
            {
                Console.WriteLine("Iniciando envío de correo...");
                await Task.Delay(TimeSpan.FromSeconds(10));

                var ventaCompleta = await ObtenerVentaConDetallesAsync(idVenta, scopedContext);

                if (ventaCompleta == null || ventaCompleta.DetalleVentas == null || !ventaCompleta.DetalleVentas.Any())
                {
                    Console.WriteLine("No se encontraron detalles de la venta.");
                    return;
                }

                var body = System.IO.File.ReadAllText("Templates/OrdenConfirmationEmail.html")
                .Replace("[LOGO_URL]", "https://i.imgur.com/EmvHFiH.png")
                .Replace("[NOMBRE_USUARIO]", ventaCompleta.Usuario.NombreUsuario)
                .Replace("[TOTAL_COMPRA]", ventaCompleta.Total.ToString("C", new System.Globalization.CultureInfo("es-MX")));


                var detalleProductos = new StringBuilder();
                foreach (var detalle in ventaCompleta.DetalleVentas)
                {
                    // Verifica si el prefijo ya está incluido en la imagen
                    string imagenProducto = detalle.Producto.Imagen.StartsWith("data:image") ?
                        detalle.Producto.Imagen :
                        "data:image/jpeg;base64," + detalle.Producto.Imagen;

                    detalleProductos.Append($@"
            <tr>
                <td><img alt='Imagen del Producto' style='width: 50px; height: 50px;' /></td>
                <td>{detalle.Producto.NombreProducto}</td>
                <td>{detalle.Cantidad}</td>
                <td>{detalle.PrecioUnitario.ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
                <td>{(detalle.Cantidad * detalle.PrecioUnitario).ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
            </tr>");
                }

                body = body.Replace("[DETALLE_PRODUCTOS]", detalleProductos.ToString());

                await emailService.SendEmailAsync(ventaCompleta.Usuario.Correo, "Tu compra está en camino", body);
                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }

        private async Task<VentaDto> ObtenerVentaConDetallesAsync(int idVenta, ProyectoContext context)
        {
            // Usa el nuevo contexto pasado como parámetro
            var venta = await context.Venta
                .Where(v => v.idVenta == idVenta)
                .Select(v => new VentaDto
                {
                    IdVenta = v.idVenta,
                    Total = (decimal)v.total,
                    FechaVenta = v.fechaVenta,
                    TipoVenta = v.tipoVenta,
                    Usuario = new UsuarioDto
                    {
                        IdUsuario = v.idUsuario,
                        NombreUsuario = context.Usuario
                            .Where(u => u.idUsuario == v.idUsuario)
                            .Select(u => u.nombreUsuario)
                            .FirstOrDefault(),
                        Correo = context.Usuario
                            .Where(u => u.idUsuario == v.idUsuario)
                            .Select(u => u.correo)
                            .FirstOrDefault()
                    }
                })
                .FirstOrDefaultAsync();

            if (venta == null) return null;

            var detalles = await context.DetalleVenta
                .Where(dv => dv.idVenta == idVenta)
                .Select(dv => new DetalleVentaDto
                {
                    IdDetalleVenta = dv.idDetalleVenta,
                    Cantidad = dv.cantidad,
                    PrecioUnitario = (decimal)dv.precioUnitario,
                    Producto = context.Producto
                        .Where(p => p.idProducto == dv.idProducto)
                        .Select(p => new ProductoDto
                        {
                            IdProducto = p.idProducto,
                            NombreProducto = p.nombreProducto,
                            Imagen = p.imagen
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            venta.DetalleVentas = detalles;

            return venta;
        }

    }
}
