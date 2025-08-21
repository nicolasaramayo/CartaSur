using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;
using VentasExamen.Application.Interfaces.Repositories;
using VentasExamen.Domain.Entities;
using VentasExamen.Infrastructure.Data;

namespace VentasExamen.Infrastructure.Repositories
{
    public class VentaRepository : BaseRepository<Venta>, IVentaRepository
    {
        public VentaRepository(VentasDbContext context) : base(context)
        {
        }

        public async Task<FechaConMasVentasDto?> ObtenerFechaConMasVentasAsync()
        {
            var resultado = await _context.Ventas
                .GroupBy(v => v.FechaVenta.Date)
                .Select(g => new FechaConMasVentasDto
                {
                    Fecha = g.Key,
                    CantidadVentas = g.Count()
                })
                .OrderByDescending(r => r.CantidadVentas)
                .FirstOrDefaultAsync();

            return resultado;
        }

        public async Task<IEnumerable<Venta>> ObtenerVentasConDetallesAsync()
        {
            return await _context.Ventas
                .Include(v => v.Empleado)
                .Include(v => v.Cliente)
                .Include(v => v.Sucursal)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Producto)
                .ToListAsync();
        }

        public override async Task<Venta?> GetByIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Empleado)
                .Include(v => v.Cliente)
                .Include(v => v.Sucursal)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id);
        }
    }
    
}
