using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAPI.Models;

namespace ProyectoFinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public DashboardController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet("ventas-totales")]
        public async Task<IActionResult> GetTotalVentas()
        {
            var totalVentas = await _context.Venta.SumAsync(v => v.total);
            return Ok(new { TotalVentas = totalVentas });
        }

        [HttpGet("productos-mas-vendidos")]
        public async Task<IActionResult> GetProductosMasVendidos()
        {
            var productosMasVendidos = await _context.DetalleVenta
                .GroupBy(dv => dv.idProducto)
                .Select(g => new 
                { 
                    ProductoId = g.Key, 
                    TotalVendidos = g.Sum(dv => dv.cantidad) 
                })
                .OrderByDescending(g => g.TotalVendidos)
                .Take(5)
                .ToListAsync();

            var productosInfo = await _context.Producto
                .Where(p => productosMasVendidos.Select(pm => pm.ProductoId).Contains(p.idProducto))
                .Select(p => new 
                { 
                    p.idProducto, 
                    p.nombreProducto 
                })
                .ToListAsync();

            var result = productosMasVendidos.Join(productosInfo,
                pm => pm.ProductoId,
                pi => pi.idProducto,
                (pm, pi) => new 
                {
                    pi.nombreProducto, 
                    pm.TotalVendidos
                });

            return Ok(result);
        }

        [HttpGet("usuarios-activos")]
        public async Task<IActionResult> GetUsuariosActivos()
        {
            var usuariosActivos = await _context.Usuario.CountAsync();
            return Ok(new { UsuariosActivos = usuariosActivos });
        }

        [HttpGet("productos-por-categoria")]
        public async Task<IActionResult> GetProductosPorCategoria()
        {
            var productosPorCategoria = await _context.Producto
                .GroupBy(p => p.idCategoria)
                .Select(g => new 
                { 
                    CategoriaId = g.Key, 
                    TotalProductos = g.Count() 
                })
                .ToListAsync();

            var categoriasInfo = await _context.Categorias
                .Where(c => productosPorCategoria.Select(pc => pc.CategoriaId).Contains(c.idCategoria))
                .Select(c => new 
                { 
                    c.idCategoria, 
                    c.nombreCategoria 
                })
                .ToListAsync();

            var result = productosPorCategoria.Join(categoriasInfo,
                pc => pc.CategoriaId,
                ci => ci.idCategoria,
                (pc, ci) => new 
                {
                    ci.nombreCategoria, 
                    pc.TotalProductos
                });

            return Ok(result);
        }
    }
}
