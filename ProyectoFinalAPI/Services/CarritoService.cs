using ProyectoFinalAPI.Models; // Asegúrate de que este espacio de nombres contiene Carrito y DetalleCarrito
using ProyectoFinalAPI; // Asegúrate de que este espacio de nombres contiene ProyectoContext
using System.Text; // Para usar StringBuilder
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
public class CarritoService
{
    private readonly ProyectoContext _context;
    private readonly EmailService _emailService;

    public CarritoService(ProyectoContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task EnviarRecordatoriosCarritosAsync()
{
    var carritosInactivos = _context.Carrito
        .Where(c => c.FechaCreacion < DateTime.Now.AddMinutes(-15))
        .Select(c => new
        {
            c.IdCarrito,
            c.FechaCreacion,
            c.IdUsuario,
            Detalles = _context.DetalleCarrito
                .Where(dc => dc.IdCarrito == c.IdCarrito)
                .Select(dc => new
                {
                    dc.IdDetalleCarrito,
                    dc.IdProducto,
                    dc.Cantidad,
                    dc.PrecioUnitario,
                    Producto = _context.Producto.FirstOrDefault(p => p.idProducto == dc.IdProducto)
                }).ToList()
        }).ToList();

    foreach (var carrito in carritosInactivos)
    {
        if (carrito.Detalles.Any())
        {
            var usuario = _context.Usuario
                .Where(u => u.idUsuario == carrito.IdUsuario)
                .Select(u => new
                {
                    u.nombreUsuario,
                    u.correo
                }).FirstOrDefault();

            if (usuario != null)
            {
                var body = GenerarCuerpoCorreo(carrito, usuario);
                await _emailService.SendEmailAsync(usuario.correo, "No olvides tu carrito", body);
            }
        }
    }
}
private string GenerarCuerpoCorreo(dynamic carrito, dynamic usuario)
{
    var detalleProductos = new StringBuilder();
    foreach (var detalle in carrito.Detalles)
    {
        detalleProductos.Append($@"
            <tr>
                <td>{detalle.Producto.nombreProducto}</td>
                <td>{detalle.Cantidad}</td>
                <td>{detalle.PrecioUnitario.ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
            </tr>");
    }

    return $@"
        <h1>Recordatorio de Carrito</h1>
        <p>Hola {usuario.nombreUsuario},</p>
        <p>No olvides completar tu compra. Aquí están los productos en tu carrito:</p>
        <table>
            <thead>
                <tr>
                    <th>Producto</th>
                    <th>Cantidad</th>
                    <th>Precio</th>
                </tr>
            </thead>
            <tbody>
                {detalleProductos}
            </tbody>
        </table>";
}

}
