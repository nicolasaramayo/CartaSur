using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Domain.Entities;
using VentasExamen.Infrastructure.Data;
using VentasExamen.Application.Interfaces.Repositories;

namespace VentasExamen.Infrastructure.Repositories
{
    public class EmpleadoRepository :BaseRepository<Empleado>, IEmpleadoRepository
    {
        public EmpleadoRepository(VentasDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Empleado>> ObtenerPorEstadoAsync(string estado)
        {
            return await _context.Empleados
                .Where(e => e.Estado == estado)
                .ToListAsync();
        }

        public async Task<bool> ExisteEmpleadoAsync(int id)
        {
            return await _context.Empleados
                .AnyAsync(e => e.IdEmpleado == id);
        }
    }
}
