using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Infrastructure.Data;
using VentasExamen.Application.Interfaces.Repositories;
using VentasExamen.Domain.Entities;

namespace VentasExamen.Infrastructure.Repositories
{
    public class SucursalRepository : BaseRepository<Sucursal>, ISucursalRepository
    {
        public SucursalRepository(VentasDbContext context) : base(context)
        {
        }
    }
}
