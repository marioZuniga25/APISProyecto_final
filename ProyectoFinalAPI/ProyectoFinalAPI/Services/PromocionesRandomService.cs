using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Services
{
 public class PromocionesRandomService
 {
  private readonly ProyectoContext _context;

  public PromocionesRandomService(ProyectoContext context)
  {
   _context = context;
  }

  public async Task EjecutarPromocionesAleatorias()
  {
   // Limpia el estado de promoción de productos que estaban en promoción aleatoria antes
   var productosEnPromocionRandom = await _context.Producto.Where(p => p.EnPromocion == 2).ToListAsync();
   foreach (var producto in productosEnPromocionRandom)
   {
    producto.EnPromocion = 0;
   }

   // Elige 5 productos aleatorios
   var productos = await _context.Producto.ToListAsync();
   var random = new Random();
   var productosAleatorios = productos.OrderBy(x => random.Next()).Take(5).ToList();

   // Crea una nueva promoción aleatoria
   var promocionRandom = new PromocionesRandom
   {
    Codigo = Guid.NewGuid().ToString(),
    Productos = productosAleatorios.Select(p => p.idProducto).ToArray(),
    FechaCreacion = DateTime.Now,
    FechaFin = DateTime.Now.AddHours(1) // Promociones válidas por 1 hora
   };

   // Asigna EnPromocion = 2 a los productos seleccionados
   foreach (var producto in productosAleatorios)
   {
    producto.EnPromocion = 2;
   }

   _context.PromocionesRandom.Add(promocionRandom);
   await _context.SaveChangesAsync();
  }
 }
}
