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


    }
}
