using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;

namespace VentasExamen.Application.Interfaces
{
    public interface IVentaService
    {
        Task<FechaConMasVentasDto?> ObtenerFechaConMasVentasAsync();
        Task<IEnumerable<VentaDto>> ObtenerVentasAsync();
        Task<VentaDto?> ObtenerVentaPorIdAsync(int id);
    }
}
