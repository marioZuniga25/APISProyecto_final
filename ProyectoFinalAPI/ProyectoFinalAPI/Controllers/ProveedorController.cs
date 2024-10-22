using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Dto;
using ProyectoFinalAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAPI.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class ProveedoresController : ControllerBase
 {
  private readonly ProyectoContext _context;
 [Route("api/[controller]")]
 [ApiController]
 public class ProveedoresController : ControllerBase
 {
  private readonly ProyectoContext _context;

  public ProveedoresController(ProyectoContext context)
  {
   _context = context;
  }
  public ProveedoresController(ProyectoContext context)
  {
   _context = context;
  }

  // GET: api/Proveedores
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ProveedorDTO>>> GetProveedores()
  {
   var proveedores = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .ToListAsync();

   // Convertimos cada entidad Proveedor a ProveedorDTO
   var proveedoresDto = proveedores.Select(p => new ProveedorDTO
   {
    idProveedor = p.idProveedor,
    nombreProveedor = p.nombreProveedor,
    telefono = p.telefono,
    correo = p.correo,
    nombresMateriasPrimas = p.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()  // Solo nombres de materias primas
   }).ToList();

   return Ok(proveedoresDto);
  }
  // GET: api/Proveedores
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ProveedorDTO>>> GetProveedores()
  {
   var proveedores = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .ToListAsync();

   // Convertimos cada entidad Proveedor a ProveedorDTO
   var proveedoresDto = proveedores.Select(p => new ProveedorDTO
   {
    idProveedor = p.idProveedor,
    nombreProveedor = p.nombreProveedor,
    telefono = p.telefono,
    correo = p.correo,
    nombresMateriasPrimas = p.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()  // Solo nombres de materias primas
   }).ToList();

   return Ok(proveedoresDto);
  }

  // GET: api/Proveedores/5
  [HttpGet("{id}")]
  public async Task<ActionResult<ProveedorDTO>> GetProveedor(int id)
  {
   var proveedor = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .FirstOrDefaultAsync(p => p.idProveedor == id);
  // GET: api/Proveedores/5
  [HttpGet("{id}")]
  public async Task<ActionResult<ProveedorDTO>> GetProveedor(int id)
  {
   var proveedor = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .FirstOrDefaultAsync(p => p.idProveedor == id);

   if (proveedor == null)
   {
    return NotFound();
   }
   if (proveedor == null)
   {
    return NotFound();
   }

   var proveedorDto = new ProveedorDTO
   {
    idProveedor = proveedor.idProveedor,
    nombreProveedor = proveedor.nombreProveedor,
    telefono = proveedor.telefono,
    correo = proveedor.correo,
    nombresMateriasPrimas = proveedor.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()
   };

   return Ok(proveedorDto);
  }

  // POST: api/Proveedores
  // POST: api/Proveedores
  [HttpPost]
  public async Task<ActionResult<ProveedorDTO>> PostProveedor(Proveedor proveedor)
  {
   if (proveedor == null)
   {
    return BadRequest("Proveedor no puede ser nulo");
   }
   var proveedorDto = new ProveedorDTO
   {
    idProveedor = proveedor.idProveedor,
    nombreProveedor = proveedor.nombreProveedor,
    telefono = proveedor.telefono,
    correo = proveedor.correo,
    nombresMateriasPrimas = proveedor.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()
   };

   return Ok(proveedorDto);
  }

  // POST: api/Proveedores
  // POST: api/Proveedores
  [HttpPost]
  public async Task<ActionResult<ProveedorDTO>> PostProveedor(Proveedor proveedor)
  {
   if (proveedor == null)
   {
    return BadRequest("Proveedor no puede ser nulo");
   }

   // Inicializa MateriasPrimas si no est� inicializado
   proveedor.MateriasPrimas = proveedor.MateriasPrimas ?? new List<MateriaPrima>();

   // Agregar el proveedor al contexto
   _context.Proveedor.Add(proveedor);

   // Guardar cambios para generar el idProveedor antes de asociar MateriasPrimas
   await _context.SaveChangesAsync();
   // Inicializa MateriasPrimas si no est� inicializado
   proveedor.MateriasPrimas = proveedor.MateriasPrimas ?? new List<MateriaPrima>();

   // Agregar el proveedor al contexto
   _context.Proveedor.Add(proveedor);

   // Guardar cambios para generar el idProveedor antes de asociar MateriasPrimas
   await _context.SaveChangesAsync();

   // Verificar si hay MateriasPrimas asociadas
   if (proveedor.MateriasPrimas.Any())
   {

    // Guardar los cambios de las materias primas
    try
    {
     await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
     // Log ex.Message o ex.InnerException para obtener m�s detalles
     return BadRequest($"Error al guardar: {ex.Message}");
    }
   }

   // Crear DTO para devolver el proveedor con los nombres de las materias primas
   var proveedorDto = new ProveedorDTO
   {
    idProveedor = proveedor.idProveedor,
    nombreProveedor = proveedor.nombreProveedor,
    telefono = proveedor.telefono,
    correo = proveedor.correo,
    nombresMateriasPrimas = proveedor.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()
   };

   return CreatedAtAction("GetProveedor", new { id = proveedor.idProveedor }, proveedorDto);
  }




   // Verificar si hay MateriasPrimas asociadas
   if (proveedor.MateriasPrimas.Any())
   {

    // Guardar los cambios de las materias primas
    try
    {
     await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
     // Log ex.Message o ex.InnerException para obtener m�s detalles
     return BadRequest($"Error al guardar: {ex.Message}");
    }
   }

   // Crear DTO para devolver el proveedor con los nombres de las materias primas
   var proveedorDto = new ProveedorDTO
   {
    idProveedor = proveedor.idProveedor,
    nombreProveedor = proveedor.nombreProveedor,
    telefono = proveedor.telefono,
    correo = proveedor.correo,
    nombresMateriasPrimas = proveedor.MateriasPrimas.Select(mp => mp.nombreMateriaPrima).ToList()
   };

   return CreatedAtAction("GetProveedor", new { id = proveedor.idProveedor }, proveedorDto);
  }





  // DELETE: api/Proveedores/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProveedor(int id)
  {
   var proveedor = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .FirstOrDefaultAsync(p => p.idProveedor == id);

   if (proveedor == null)
   {
    return NotFound();
   }

   // Eliminar materias primas asociadas al proveedor antes de eliminar el proveedor
   if (proveedor.MateriasPrimas != null && proveedor.MateriasPrimas.Count > 0)
   {
    _context.MateriasPrimas.RemoveRange(proveedor.MateriasPrimas);
   }
  // DELETE: api/Proveedores/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProveedor(int id)
  {
   var proveedor = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .FirstOrDefaultAsync(p => p.idProveedor == id);

   if (proveedor == null)
   {
    return NotFound();
   }

   // Eliminar materias primas asociadas al proveedor antes de eliminar el proveedor
   if (proveedor.MateriasPrimas != null && proveedor.MateriasPrimas.Count > 0)
   {
    _context.MateriasPrimas.RemoveRange(proveedor.MateriasPrimas);
   }

   _context.Proveedor.Remove(proveedor);
   await _context.SaveChangesAsync();
   _context.Proveedor.Remove(proveedor);
   await _context.SaveChangesAsync();

   return NoContent();
  }

  // GET: api/Proveedores/{id}/materiasprimas
  // Obtener la lista de materias primas que un proveedor vende
  [HttpGet("{id}/materiasprimas")]
  public async Task<ActionResult<IEnumerable<MateriaPrimaDTO>>> GetMateriasPrimasPorProveedor(int id)
  {
   var proveedor = await _context.Proveedor
       .Include(p => p.MateriasPrimas)
       .FirstOrDefaultAsync(p => p.idProveedor == id);

   if (proveedor == null)
   {
    return NotFound("No se encontr� el proveedor");
   }

   // Convertimos las Materias Primas asociadas en MateriaPrimaDTO
   var materiasPrimasDto = proveedor.MateriasPrimas.Select(mp => new MateriaPrimaDTO
   {
    NombreMateriaPrima = mp.nombreMateriaPrima,
    Descripcion = mp.descripcion,
    Precio= mp.precio,
    Stock= mp.stock,
    idUnidad = mp.idUnidad
   }).ToList();

   return Ok(materiasPrimasDto);
  }

  private bool ProveedorExists(int id)
  {
   return _context.Proveedor.Any(e => e.idProveedor == id);
  }
 }
  private bool ProveedorExists(int id)
  {
   return _context.Proveedor.Any(e => e.idProveedor == id);
  }
 }
}
