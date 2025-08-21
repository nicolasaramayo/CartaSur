using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Domain.Entities;
using VentasExamen.Application.Interfaces.Repositories;
using VentasExamen.Infrastructure.Data;

namespace VentasExamen.Infrastructure.Repositories
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(VentasDbContext context) : base(context)
        {
        }
        
    }
}
