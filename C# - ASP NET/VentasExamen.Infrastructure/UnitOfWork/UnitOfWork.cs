using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.Interfaces;
using VentasExamen.Application.Interfaces.Repositories;
using VentasExamen.Infrastructure.Data;
using VentasExamen.Infrastructure.Repositories;

namespace VentasExamen.Infrastructure.UnitOfWork
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly VentasDbContext _context;
        private IDbContextTransaction? _transaction;

        private IVentaRepository? _ventas;
        private IEmpleadoRepository? _empleados;
        private IClienteRepository? _clientes;
        private ISucursalRepository? _sucursales;
        private IProductoRepository? _productos;

        public UnitOfWork(VentasDbContext context)
        {
            _context = context;
        }

        public IVentaRepository Ventas =>
            _ventas ??= new VentaRepository(_context);

        public IEmpleadoRepository Empleados =>
            _empleados ??= new EmpleadoRepository(_context);

        public IClienteRepository Clientes =>
            _clientes ??= new ClienteRepository(_context);

        public ISucursalRepository Sucursales =>
            _sucursales ??= new SucursalRepository(_context);

        public IProductoRepository Productos =>
            _productos ??= new ProductoRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
