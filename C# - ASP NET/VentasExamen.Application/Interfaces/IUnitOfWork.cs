using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.Interfaces.Repositories;

namespace VentasExamen.Application.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IVentaRepository Ventas { get; }
        IEmpleadoRepository Empleados { get; }
        IClienteRepository Clientes { get; }
        ISucursalRepository Sucursales { get; }
        IProductoRepository Productos { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
