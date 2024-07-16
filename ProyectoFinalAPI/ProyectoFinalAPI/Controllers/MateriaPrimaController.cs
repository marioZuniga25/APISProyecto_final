using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<MateriaPrima>>> GetListadoMateriasPrimas()
        {

            return await _context.MateriasPrimas.ToListAsync();

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

    }
}
