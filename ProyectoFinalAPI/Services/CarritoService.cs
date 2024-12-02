using ProyectoFinalAPI.Models; 
using ProyectoFinalAPI; 
using System.Text; 
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
        .Where(c => c.FechaCreacion < DateTime.Now.AddMinutes(-24))
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
        // Verifica si la imagen del producto existe y agrega la imagen al correo
        string imageUrl = detalle.Producto.imagen ?? "URL_POR_DEFECTO"; // Asegúrate de tener un campo ImagenUrl

        detalleProductos.Append($@"
            <tr>
                <td style='text-align: center;'>{detalle.Producto.nombreProducto}</td>
                <td style='text-align: center;'>{detalle.Cantidad}</td>
                <td style='text-align: center;'>{detalle.PrecioUnitario.ToString("C", new System.Globalization.CultureInfo("es-MX"))}</td>
            </tr>");
    }

    return $@"
    <!DOCTYPE html>
    <html lang='es'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Recordatorio de Carrito</title>
        <style>
            body {{
                font-family: 'Arial', sans-serif;
                background-color: #f4f4f4;
                color: #333333;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 50px auto;
                background-color: #ffffff;
                padding: 30px;
                border-radius: 12px;
                text-align: center;
                box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
            }}
            .header img {{
                max-width: 150px;
                margin-bottom: 20px;
            }}
            .content h2 {{
                color: #4CAF50;
            }}
            .content p {{
                margin: 15px 0;
                font-size: 16px;
                color: #666666;
            }}
            .button a {{
                background-color: #4CAF50;
                color: white;
                padding: 12px 20px;
                text-decoration: none;
                font-size: 16px;
                border-radius: 5px;
                transition: background-color 0.3s;
            }}
            .button a:hover {{
                background-color: #45a049;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 12px;
                color: #999999;
            }}
            table {{
                width: 100%;
                border-collapse: collapse;
                margin-top: 20px;
            }}
            th, td {{
                padding: 10px;
                border: 1px solid #ddd;
                text-align: center;
            }}
            th {{
                background-color: #4CAF50;
                color: white;
            }}
            img {{
                max-width: 80px;
                height: auto;
                border-radius: 8px;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <img src='[LOGO_URL]' alt='Logo'>
            </div>
            <div class='content'>
                <h2>¡Hola, {usuario.nombreUsuario}!</h2>
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
                </table>
            </div>
            <div class='footer'>
                &copy; 2024 - Tu Empresa. Todos los derechos reservados.
            </div>
        </div>
    </body>
    </html>";
}

}