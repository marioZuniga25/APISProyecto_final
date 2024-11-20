using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAPI.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class PromocionesController : ControllerBase
 {
  private readonly ProyectoContext _context;
  private readonly EmailService _emailService;
  public PromocionesController(ProyectoContext context, EmailService emailService)
  {
   _context = context;
   _emailService = emailService;
  }

  [HttpPost("CreatePromocion")]
  public async Task<ActionResult<Promocion>> CreatePromocion([FromBody] PromocionDto promocionDto)
  {
   // Crear la instancia de Promocion con los datos recibidos
   var promocion = new Promocion
   {
    Nombre = promocionDto.Nombre,
    FechaInicio = promocionDto.FechaInicio,
    FechaFin = promocionDto.FechaFin,
   };

   // Guarda la promoción en la base de datos
   _context.Promociones.Add(promocion);
   await _context.SaveChangesAsync();

   // Agregar los productos seleccionados como DetallePromocion
   foreach (var idProducto in promocionDto.ProductosIds)
   {
    var producto = await _context.Producto.FindAsync(idProducto);
    if (producto != null)
    {
     // Cálculo del precio final basado en el descuento
     var precioFinal = producto.precio - (producto.precio * (promocionDto.Descuento / 100.0));

     var detallePromocion = new DetallePromocion
     {
      IdPromocion = promocion.IdPromocion,
      IdProducto = idProducto,
      PorcentajeDescuento = promocionDto.Descuento,
      PrecioFinal = precioFinal
     };

     _context.DetallePromocion.Add(detallePromocion);

     // Actualiza el campo EnPromocion en el producto
     producto.EnPromocion = 1; // 1 indica promoción activa
    }
   }

   await _context.SaveChangesAsync();

   return CreatedAtAction(nameof(GetPromocionById), new { id = promocion.IdPromocion }, promocion);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Promocion>> GetPromocionById(int id)
  {
   var promocion = await _context.Promociones
       .Include(p => p.Detalles) // Incluye los detalles de la promoción
       .ThenInclude(d => d.Producto) // Incluye los productos en cada detalle
       .FirstOrDefaultAsync(p => p.IdPromocion == id);

   if (promocion == null)
   {
    return NotFound();
   }
   return promocion;
  }

  [HttpPost("EnviarCorreoPromocion")]
  public async Task<IActionResult> EnviarCorreoPromocion(int promocionId, string recipientType)
  {
   Console.WriteLine($"Iniciando envío de correos para la promoción ID: {promocionId} a destinatarios de tipo: {recipientType}");

   try
   {
    var promocion = await _context.Promociones.Include(p => p.Detalles).ThenInclude(d => d.Producto)
                        .FirstOrDefaultAsync(p => p.IdPromocion == promocionId);

    if (promocion == null)
    {
     Console.WriteLine("Promoción no encontrada.");
     return NotFound("Promoción no encontrada");
    }

    List<Usuario> usuarios;
    if (recipientType == "frecuentes")
    {
     usuarios = await _context.Usuario.Where(u => u.loginCount > 5).ToListAsync();
    }
    else if (recipientType == "nuevos")
    {
     usuarios = await _context.Usuario.Where(u => u.loginCount <= 1).ToListAsync();
    }
    else
    {
     usuarios = await _context.Usuario.ToListAsync();
    }

    Console.WriteLine($"Usuarios encontrados: {usuarios.Count}");

    var subject = $"Nueva Promoción: {promocion.Nombre}";
    var body = $@"
                    <div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto;'>
                        <h2 style='text-al0ign: center; color: #4CAF50;'>Gracias por tu interés en nuestras promociones!</h2>
                        <p>Estamos emocionados de ofrecerte una nueva promoción en nuestros productos.</p>
                        <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                            <thead>
                                <tr>
                                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Imagen</th>
                                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Producto</th>
                                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Descuento</th>
                                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Precio Final</th>
                                </tr>
                            </thead>
                            <tbody>";

    foreach (var detalle in promocion.Detalles)
    {
     var imagenUrl = $"data:image/png;base64,{detalle.Producto.imagen}"; // Debe ser una URL pública
     var nombreProducto = detalle.Producto.nombreProducto;
     var porcentajeDescuento = detalle.PorcentajeDescuento;
     var precioFinal = detalle.PrecioFinal.ToString("C");

     body += $@"
                                <tr>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-ali gn: center;'>
                                        <img src='{imagenUrl}' alt='{nombreProducto}' style='width: 50px; height: auto; border-radius: 4px;'/>
                                    </td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{nombreProducto}</td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{porcentajeDescuento}%</td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{precioFinal}</td>
                                </tr>";
    }

    body += @"
                            </tbody>
                        </table>
                        <p style='text-align: center; margin-top: 20px;'>¡Aprovecha esta oportunidad antes de que termine!</p>
                    </div>";

    foreach (var usuario in usuarios)
    {
     try
     {
      await _emailService.SendEmailAsync(usuario.correo, subject, body);
      Console.WriteLine($"Correo enviado exitosamente a: {usuario.correo}");
     }
     catch (Exception ex)
     {
      Console.WriteLine($"Error al enviar correo a {usuario.correo}: {ex.Message}");
     }
    }

    return Ok("Correos enviados con éxito");
   }
   catch (Exception ex)
   {
    Console.WriteLine($"Error en EnviarCorreoPromocion: {ex.Message}");
    return StatusCode(500, "Error interno al enviar correos.");
   }
  }
 }
}