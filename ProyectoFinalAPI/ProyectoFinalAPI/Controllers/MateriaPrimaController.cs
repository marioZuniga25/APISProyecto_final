using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.DTOs;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly ProyectoContext _context;
        public MateriaPrimaController(ProyectoContext context)
        {
            _context = context;
        }


        [HttpGet("ListadoMateriasP")]
        public async Task<ActionResult<IEnumerable<MateriaPrimaDto>>> GetListadoMateriasPrimas()
        {
            var materiasPrimas = await _context.MateriasPrimas
                .Include(mp => mp.Inventario) // Asegúrate de que la relación está correctamente configurada
                .ToListAsync();

            // Mapea las materias primas a DTOs
            var resultado = materiasPrimas.Select(mp => new MateriaPrimaDto
            {
                idMateriaPrima = mp.idMateriaPrima,
                nombreMateriaPrima = mp.nombreMateriaPrima,
                descripcion = mp.descripcion,
                idInventario = mp.idInventario,
                cantidad = mp.Inventario.cantidad // Incluye la cantidad de inventario
            });

            return Ok(resultado);
        }


        [HttpGet("BuscarMateriaP")]
        public async Task<ActionResult<IEnumerable<MateriaPrima>>> SearchMateriaPrima(string materia)
        {
            return await _context.MateriasPrimas.Where(u => u.nombreMateriaPrima.Contains(materia)).ToListAsync();
        }

        [HttpPost("AgregarMateriaP")]
        public async Task<ActionResult> AddMateriaPrima([FromBody] MateriaPrima request)
        {

            await _context.MateriasPrimas.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }


        [HttpPut("ModificarMateriaP")]
        public async Task<IActionResult> updateMateriaPrima(int id, [FromBody] MateriaPrima request)
        {
            var materiaModificar = await _context.MateriasPrimas.FindAsync(id);

            if (materiaModificar == null)
            {
                return BadRequest("No existe la Materia Prima");
            }

            materiaModificar.nombreMateriaPrima = request.nombreMateriaPrima;
            materiaModificar.descripcion = request.descripcion;
            materiaModificar.idInventario = request.idInventario;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();

            }

            return Ok();
        }

        [HttpDelete]
        [Route("EliminarMateriaP/{id:int}")]
        public async Task<IActionResult> deleteUsuario(int id)
        {
            var materiaEliminar = await _context.MateriasPrimas.FindAsync(id);

            if (materiaEliminar == null)
            {
                return BadRequest("No se encontro la Materia Prima.");
            }
            _context.MateriasPrimas.Remove(materiaEliminar);

            await _context.SaveChangesAsync();

            return Ok();

        }

    }
}
