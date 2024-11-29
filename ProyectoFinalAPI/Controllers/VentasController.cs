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
                //_logger.LogInformation("Iniciando envío de correo para la venta con ID: {IdVenta}...", idVenta);

                await Task.Delay(TimeSpan.FromSeconds(10)); // Esto puede ser innecesario, es solo un retraso para simular procesamiento

                var ventaCompleta = await ObtenerVentaConDetallesAsync(idVenta, scopedContext);

                if (ventaCompleta == null || ventaCompleta.DetalleVentas == null || !ventaCompleta.DetalleVentas.Any())
                {
                    // _logger.LogWarning("No se encontraron detalles de la venta con ID: {IdVenta}.", idVenta);
                    return;
                }

                //_logger.LogInformation("Venta con ID {IdVenta} encontrada. Preparando detalles del correo...", idVenta);

                // Cargar la plantilla HTML y reemplazar placeholders básicos
                var body = System.IO.File.ReadAllText("Templates/OrdenConfirmationEmail.html")
                    .Replace("[LOGO_URL]", "https://i.imgur.com/EmvHFiH.png")
                    .Replace("[NOMBRE_USUARIO]", ventaCompleta.Usuario.NombreUsuario)
                    .Replace("[TOTAL_COMPRA]", ventaCompleta.Total.ToString("C", new System.Globalization.CultureInfo("es-MX")));

                // Detalle de productos comprados
                var detalleProductos = new StringBuilder();
                foreach (var detalle in ventaCompleta.DetalleVentas)
                {
                    string imagenProducto = detalle.Producto.Imagen.StartsWith("data:image") ?
                    detalle.Producto.Imagen :
                    "data:image/jpeg;base64," + detalle.Producto.Imagen;

                    // Construir el detalle de cada producto con su imagen Base64
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

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var primerDetalle = ventaCompleta.DetalleVentas.FirstOrDefault();
                var categoriaId = 0;

                if (primerDetalle != null)
                {
                    // Si necesitas acceder a la categoría del producto del primer detalle
                    categoriaId = primerDetalle.Producto.IdCategoria;
                    //_logger.LogInformation($"Categoría del producto: {categoriaId}");
                }
                else
                {
                    //_logger.LogWarning("No se encontraron detalles de venta.");
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var productosRelacionadosHtml = new StringBuilder();
                int productosMostrados = 0;
                // Obtener productos relacionados (Ejemplo: por categoría del primer producto comprado)
                var productosRelacionados = ObtenerProductosRelacionados(categoriaId, scopedContext);

                // Reemplazar los placeholders de productos relacionados
                foreach (var producto in productosRelacionados)
                {
                    // Limitar a un máximo de 3 productos
                    if (productosMostrados >= 3) break;

                    // Verificar si la imagen está en formato Base64 o URL
                    string imagenProducto = producto.imagen.StartsWith("data:image") ?
                        producto.imagen :
                        "data:image/jpeg;base64," + producto.imagen;  // Asumir que la imagen es Base64 si no tiene una URL válida

                    // Construir el HTML del producto relacionado
                    productosRelacionadosHtml.Append($@"
                    <div class='product-item'>
                        <img src='' alt='{producto.nombreProducto}' style='width: 100px; height: 100px;' />
                        <p>{producto.nombreProducto}</p>
                        <p>{producto.precio.ToString("C", new System.Globalization.CultureInfo("es-MX"))} MXN</p>
                        <a href='/productos/{producto.idProducto}'>Ver producto</a>
                    </div>
                    ");
                    productosMostrados++;
                }

                // Si se encontraron productos relacionados, reemplazar los marcadores en el cuerpo del correo
                if (productosMostrados > 0)
                {
                    body = body.Replace("[PRODUCTOS_RELACIONADOS]", productosRelacionadosHtml.ToString());
                }
                else
                {
                    body = body.Replace("[PRODUCTOS_RELACIONADOS]", "<p>No tenemos productos relacionados en este momento.</p>");
                }



                // Enviar el correo con los detalles ya reemplazados
                await emailService.SendEmailAsync(ventaCompleta.Usuario.Correo, "Tu compra está en camino", body);

                //_logger.LogInformation("Correo enviado correctamente a {CorreoUsuario} para la venta con ID: {IdVenta}.", ventaCompleta.Usuario.Correo, idVenta);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al enviar el correo para la venta con ID: {IdVenta}.", idVenta);
            }
        }



        private List<Producto> ObtenerProductosRelacionados(int categoriaId, ProyectoContext context)
        {
            // _logger.LogInformation("IDPRODUCTO1: "+categoriaId);
            var productos = context.Producto
            .Where(p => p.idCategoria == categoriaId)
            .OrderBy(p => p.idProducto)
            .Take(3)
            .ToList();



            return productos;
        }

        [HttpGet("GetProductosRelacionados/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosRelacionados(int categoriaId)
        {
            try
            {
                //_logger.LogInformation("Consultando productos relacionados para la categoría ID: {CategoriaId}", categoriaId);

                var productosRelacionados = await _context.Producto
                    .Where(p => p.idCategoria == categoriaId)
                    .OrderBy(p => p.idProducto)
                    .Take(4)
                    .ToListAsync();

                if (productosRelacionados == null || !productosRelacionados.Any())
                {
                    //_logger.LogWarning("No se encontraron productos relacionados para la categoría ID: {CategoriaId}", categoriaId);
                    return NotFound("No se encontraron productos relacionados.");
                }

                //_logger.LogInformation("Se encontraron {Count} productos relacionados.", productosRelacionados.Count);
                return Ok(productosRelacionados);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al obtener productos relacionados para la categoría ID: {CategoriaId}", categoriaId);
                return StatusCode(500, "Error interno del servidor.");
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
                            Imagen = p.imagen,
                            IdCategoria = p.idCategoria
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            venta.DetalleVentas = detalles;

            return venta;
        }

    }
}
