using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;
using VentasExamen.Domain.Entities;

namespace VentasExamen.Application.Interfaces.Repositories
{
    public interface IVentaRepository : IBaseRepository<Venta>
    {
        Task<FechaConMasVentasDto?> ObtenerFechaConMasVentasAsync();
        Task<IEnumerable<Venta>> ObtenerVentasConDetallesAsync();
    }
}
