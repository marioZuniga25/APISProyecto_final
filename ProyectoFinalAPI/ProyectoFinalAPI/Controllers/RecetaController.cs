using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public RecetaController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet("ListadoRecetas")]
        public async Task<ActionResult<IEnumerable<Receta>>> GetListadoRecetas()
        {
            var recetas = await _context.Recetas
                .Include(r => r.Producto)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.MateriaPrima)
                .ToListAsync();

            // Verificar que no se esté incluyendo una receta con valores por defecto
            if (recetas == null || recetas.Count == 0)
            {
                return NotFound("No se encontraron recetas");
            }

            return Ok(recetas);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Receta>> GetRecetaById(int id)
        {
            var receta = await _context.Recetas
                .Include(r => r.Producto)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.MateriaPrima)
                .FirstOrDefaultAsync(r => r.idReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            return Ok(receta);
        }

        [HttpPost("Agregar")]
public async Task<ActionResult> AgregarReceta([FromBody] RecetaDto request)
{
    var recetaExistente = await _context.Recetas
        .FirstOrDefaultAsync(r => r.idProducto == request.IdProducto);

    if (recetaExistente != null)
    {
        return BadRequest("Ya existe una receta para este producto.");
    }

    var producto = await _context.Producto.FindAsync(request.IdProducto);
    if (producto == null)
    {
        return BadRequest("Producto no encontrado");
    }

    var detalles = new List<RecetaDetalle>();
    foreach (var detalleDto in request.Detalles)
    {
        var materiaPrima = await _context.MateriasPrimas.FindAsync(detalleDto.IdMateriaPrima);
        if (materiaPrima == null)
        {
            return BadRequest($"Materia Prima con ID {detalleDto.IdMateriaPrima} no encontrada");
        }

        detalles.Add(new RecetaDetalle
        {
            idMateriaPrima = detalleDto.IdMateriaPrima,
            cantidad = detalleDto.Cantidad,
        });
    }

    var newReceta = new Receta
    {
        idProducto = request.IdProducto,
        Detalles = detalles
    };

    await _context.Recetas.AddAsync(newReceta);
    await _context.SaveChangesAsync();

    return Ok(newReceta);
}


        [HttpPut("Modificar/{id}")]
        public async Task<ActionResult> ModificarReceta(int id, [FromBody] Receta request)
        {
            var recetaModificar = await _context.Recetas
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.idReceta == id);

            if (recetaModificar == null)
            {
                return NotFound("Receta no encontrada");
            }

            recetaModificar.Detalles = request.Detalles;

            _context.Entry(recetaModificar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(recetaModificar);
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarReceta(int id)
        {
            var recetaEliminar = await _context.Recetas.FindAsync(id);
            if (recetaEliminar == null)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }

            _context.Recetas.Remove(recetaEliminar);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Receta eliminada correctamente" });
        }

        [HttpPost("ProcesarReceta/{id}/{cantidad}")]
        public async Task<ActionResult> ProcesarReceta(int id, int cantidad)
        {
            // Obtener la receta por ID, incluyendo detalles y materia prima
            var receta = await _context.Recetas
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.MateriaPrima)
                .Include(r => r.Producto) // Incluir el producto asociado a la receta
                .FirstOrDefaultAsync(r => r.idReceta == id);

            if (receta == null)
            {
                return NotFound("Receta no encontrada");
            }

            // Validar que haya suficiente inventario para producir la cantidad solicitada
            foreach (var detalle in receta.Detalles)
            {
                var inventarioItem = await _context.Inventarios
                    .FirstOrDefaultAsync(i => i.idInventario == detalle.MateriaPrima.idInventario);

                if (inventarioItem == null)
                {
                    return BadRequest($"No se encontró la materia prima con ID de inventario {detalle.MateriaPrima.idInventario}");
                }

                var totalRequerido = detalle.cantidad * cantidad;
                if (inventarioItem.cantidad < totalRequerido)
                {
                    return BadRequest($"Cantidad insuficiente en inventario para la materia prima {detalle.MateriaPrima.nombreMateriaPrima}. Cantidad disponible: {inventarioItem.cantidad}, Cantidad requerida: {totalRequerido}");
                }
            }

            // Si hay suficiente inventario, proceder a descontar las cantidades
            foreach (var detalle in receta.Detalles)
            {
                var inventarioItem = await _context.Inventarios
                    .FirstOrDefaultAsync(i => i.idInventario == detalle.MateriaPrima.idInventario);

                inventarioItem.cantidad -= detalle.cantidad * cantidad;
            }

            // Actualizar el stock del producto
            var producto = await _context.Producto
                .FirstOrDefaultAsync(p => p.idProducto == receta.idProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado para esta receta");
            }

            producto.stock += cantidad; // Aumenta el stock del producto por la cantidad producida

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Receta procesada y cantidades descontadas del inventario correctamente" });
        }

    }
}
