using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;

namespace VentasExamen.Application.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> ObtenerTodosAsync();
        Task<EmpleadoDto?> ObtenerPorIdAsync(int id);
        Task<EmpleadoDto> CrearAsync(CrearEmpleadoDto empleadoDto);
        Task<EmpleadoDto?> ActualizarAsync(int id, CrearEmpleadoDto empleadoDto);
        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<EmpleadoDto>> ObtenerPorEstadoAsync(string estado);
    }
}
