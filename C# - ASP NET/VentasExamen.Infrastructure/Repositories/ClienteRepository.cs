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
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(VentasDbContext context) : base(context)
        {
        }

        public async Task<Cliente?> ObtenerPorDniAsync(string dni)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Dni == dni);
        }
    }
}
