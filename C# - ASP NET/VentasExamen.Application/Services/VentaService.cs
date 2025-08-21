using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;
using VentasExamen.Application.Interfaces;

namespace VentasExamen.Application.Services
{
    public class VentaService :IVentaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VentaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FechaConMasVentasDto?> ObtenerFechaConMasVentasAsync()
        {
            try
            {
                return await _unitOfWork.Ventas.ObtenerFechaConMasVentasAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("Error al obtener la fecha con más ventas", ex);
            }
        }

        public async Task<IEnumerable<VentaDto>> ObtenerVentasAsync()
        {
            try
            {
                var ventas = await _unitOfWork.Ventas.ObtenerVentasConDetallesAsync();

                return ventas.Select(v => new VentaDto
                {
                    IdVenta = v.IdVenta,
                    FechaVenta = v.FechaVenta,
                    ImporteTotal = v.ImporteTotal,
                    NombreEmpleado = v.Empleado.NombreCompleto,
                    NombreCliente = v.Cliente.NombreCompleto,
                    NombreSucursal = v.Sucursal.Nombre
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener las ventas", ex);
            }
        }

        public async Task<VentaDto?> ObtenerVentaPorIdAsync(int id)
        {
            try
            {
                var venta = await _unitOfWork.Ventas.GetByIdAsync(id);

                if (venta == null) return null;

                return new VentaDto
                {
                    IdVenta = venta.IdVenta,
                    FechaVenta = venta.FechaVenta,
                    ImporteTotal = venta.ImporteTotal,
                    NombreEmpleado = venta.Empleado?.NombreCompleto ?? "",
                    NombreCliente = venta.Cliente?.NombreCompleto ?? "",
                    NombreSucursal = venta.Sucursal?.Nombre ?? ""
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener la venta con ID {id}", ex);
            }
        }
    }
}
