using Microsoft.AspNetCore.Mvc;
using ProyectoFinalAPI.Models;
using ProyectoFinalAPI;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraController : ControllerBase
{
 private readonly ProyectoContext _context;

 public OrdenCompraController(ProyectoContext context)
 {
  _context = context;
 }

 // POST: api/OrdenCompra/CrearOrdenCompra
 [HttpPost("CrearOrdenCompra")]
 public async Task<ActionResult> CrearOrdenCompra([FromBody] OrdenCompra request)
 {
  // Validar que el proveedor existe
  if (request.idProveedor <= 0)
  {
   return BadRequest("El proveedor es requerido.");
  }

  var proveedor = await _context.Proveedor.FindAsync(request.idProveedor);
  if (proveedor == null)
  {
   return BadRequest("El proveedor no existe.");
  }

  // Validar que la orden de compra tiene detalles
  if (request.Detalles == null || !request.Detalles.Any())
  {
   return BadRequest("La orden de compra debe tener al menos un detalle.");
  }

  // Crear la orden de compra
  OrdenCompra nuevaOrden = new OrdenCompra
  {
   idProveedor = request.idProveedor,
   fechaCompra = DateTime.Now,
   Detalles = new List<DetalleOrdenCompra>()
  };

  // Procesar cada detalle
  foreach (var detalleRequest in request.Detalles)
  {
   var materiaPrima = await _context.MateriasPrimas.FindAsync(detalleRequest.idMateriaPrima);
   if (materiaPrima == null)
   {
    return BadRequest($"La materia prima con ID {detalleRequest.idMateriaPrima} no existe.");
   }

   var detalleOrden = new DetalleOrdenCompra
   {
    idMateriaPrima = detalleRequest.idMateriaPrima,
    cantidad = detalleRequest.cantidad,
    precioUnitario = materiaPrima.precio
   };

   nuevaOrden.Detalles.Add(detalleOrden);
  }

  // Guardar la orden de compra
  await _context.OrdenesCompra.AddAsync(nuevaOrden);
  await _context.SaveChangesAsync();

  return Ok(nuevaOrden);
 }


 // GET: api/OrdenCompra/ListadoOrdenes
 [HttpGet("ListadoOrdenes")]
 public async Task<ActionResult<IEnumerable<OrdenCompra>>> GetListadoOrdenesCompra()
 {
  var ordenesCompra = await _context.OrdenesCompra
      .Include(o => o.idProveedor)
      .Include(o => o.Detalles)
          .ThenInclude(d => d.MateriaPrima)
      .ToListAsync();

  return Ok(ordenesCompra);
 }
}
