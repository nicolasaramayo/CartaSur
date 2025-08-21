using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Domain.Entities;

namespace VentasExamen.Application.Interfaces.Repositories
{
    public interface IEmpleadoRepository : IBaseRepository<Empleado>
    {
        Task<IEnumerable<Empleado>> ObtenerPorEstadoAsync(string estado);
        Task<bool> ExisteEmpleadoAsync(int id);
    }
}
