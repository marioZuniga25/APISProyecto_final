using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {

        private readonly ProyectoContext _context;
        private readonly ILogger<VentasController> _logger; 
        private readonly IServiceProvider _serviceProvider;

        public VentasController(ProyectoContext context, ILogger<VentasController> logger, IServiceProvider serviceProvider)
        {
            _context = context;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        [HttpPost("AgregarVentaOnline")]
        public async Task<ActionResult> AddVentaOnline([FromBody] Venta request)
        {
            try
            {
                request.tipoVenta = "Online";
                await _context.Venta.AddAsync(request);
                await _context.SaveChangesAsync();
                int idVentaGenerado = request.idVenta;

                return Ok(idVentaGenerado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar venta online.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        [HttpPost("AgregarVentaFisica")]
        public async Task<ActionResult> AddVentaFisica([FromBody] Venta request)
        {
            try
            {
                request.tipoVenta = "Fisica";
                await _context.Venta.AddAsync(request);
                await _context.SaveChangesAsync();
                int idVentaGenerado = request.idVenta;

                return Ok(idVentaGenerado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar venta física.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        [HttpPost("AgregarDetalleVenta")]
        public async Task<ActionResult> AddDetallesVenta([FromBody] List<DetalleVenta> detallesVenta, [FromServices] EmailService emailService)
        {
            try
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
                _ = EnviarCorreoConDetallesAsync(detallesVenta.First().idVenta, emailService);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar detalles de la venta.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVentaById(int id)
        {
            try
            {
                var venta = await _context.Venta.FindAsync(id);
                if (venta == null)
                {
                    return NotFound();
                }
                return venta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la venta con ID: {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Nuevo Endpoint para obtener los detalles de una venta por ID de la venta
        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetalleVentaByVentaId(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de la venta con ID: {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("GetVentasByTipo/{tipoVenta}")]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasByTipo(string tipoVenta)
        {
            _logger.LogInformation("Iniciando consulta para ventas con tipo: {TipoVenta}", tipoVenta);  // Log de inicio de la consulta

            try
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
                                    }).FirstOrDefault()
                            }).ToList()
                    })
                    .ToListAsync();

                if (ventas == null || !ventas.Any())
                {
                    _logger.LogWarning("No se encontraron ventas del tipo '{TipoVenta}'.", tipoVenta);  // Log si no hay ventas
                    return NotFound($"No se encontraron ventas del tipo '{tipoVenta}'.");
                }

                _logger.LogInformation("Consulta completada, se encontraron {VentasCount} ventas.", ventas.Count);  // Log cuando se encuentra información

                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar la consulta de ventas para el tipo: {TipoVenta}", tipoVenta);  // Log de error
                return StatusCode(500, "Hubo un error al procesar la solicitud.");
            }
        }


        private async Task EnviarCorreoConDetallesAsync(int idVenta, EmailService emailService)
        {
            // Crear un nuevo contexto para esta función
            using var scope = _serviceProvider.CreateScope();
            var scopedContext = scope.ServiceProvider.GetRequiredService<ProyectoContext>();

            try
            {
                _logger.LogInformation("Iniciando envío de correo para la venta con ID: {IdVenta}...", idVenta);

                await Task.Delay(TimeSpan.FromSeconds(10));

                var ventaCompleta = await ObtenerVentaConDetallesAsync(idVenta, scopedContext);

                if (ventaCompleta == null || ventaCompleta.DetalleVentas == null || !ventaCompleta.DetalleVentas.Any())
                {
                    _logger.LogWarning("No se encontraron detalles de la venta con ID: {IdVenta}.", idVenta);
                    return;
                }

                _logger.LogInformation("Venta con ID {IdVenta} encontrada. Preparando detalles del correo...", idVenta);

                var body = System.IO.File.ReadAllText("Templates/OrdenConfirmationEmail.html")
                    .Replace("[LOGO_URL]", "https://i.imgur.com/EmvHFiH.png")
                    .Replace("[NOMBRE_USUARIO]", ventaCompleta.Usuario.NombreUsuario)
                    .Replace("[TOTAL_COMPRA]", ventaCompleta.Total.ToString("C", new System.Globalization.CultureInfo("es-MX")));

                var detalleProductos = new StringBuilder();
                foreach (var detalle in ventaCompleta.DetalleVentas)
                {
                    string imagenProducto = detalle.Producto.Imagen.StartsWith("data:image") ?
                        detalle.Producto.Imagen :
                        "data:image/jpeg;base64," + detalle.Producto.Imagen;

                    detalleProductos.Append($@"
                <tr>
                    <td><img alt='Imagen del Producto' style='width: 50px; height: 50px;' src='{imagenProducto}' /></td>
                    <td>{detalle.Producto.NombreProducto}</td>
                    <td>{detalle.Cantidad}</td>
                    <td>{detalle.PrecioUnitario.ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
                    <td>{(detalle.Cantidad * detalle.PrecioUnitario).ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
                </tr>");
                }

                body = body.Replace("[DETALLE_PRODUCTOS]", detalleProductos.ToString());

                await emailService.SendEmailAsync(ventaCompleta.Usuario.Correo, "Tu compra está en camino", body);
                _logger.LogInformation("Correo enviado correctamente a {CorreoUsuario} para la venta con ID: {IdVenta}.", ventaCompleta.Usuario.Correo, idVenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar el correo para la venta con ID: {IdVenta}.", idVenta);
            }
        }


        private async Task<VentaDto> ObtenerVentaConDetallesAsync(int idVenta, ProyectoContext context)
        {
            try
            {
                _logger.LogInformation("Iniciando la consulta para obtener la venta con ID: {IdVenta}", idVenta);

                // Obtiene la venta
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

                if (venta == null)
                {
                    _logger.LogWarning("No se encontró la venta con ID: {IdVenta}", idVenta);
                    return null;
                }

                _logger.LogInformation("Venta con ID: {IdVenta} encontrada. Ahora recuperando los detalles de la venta...", idVenta);

                // Obtiene los detalles de la venta
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

                if (detalles == null || !detalles.Any())
                {
                    _logger.LogWarning("No se encontraron detalles para la venta con ID: {IdVenta}", idVenta);
                }

                // Asigna los detalles a la venta
                venta.DetalleVentas = detalles;

                _logger.LogInformation("Detalles de la venta con ID: {IdVenta} recuperados correctamente.", idVenta);

                return venta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar obtener los detalles de la venta con ID: {IdVenta}", idVenta);
                throw; // Re-lanza la excepción para que se maneje en otro lugar si es necesario
            }
        }


    }
}
