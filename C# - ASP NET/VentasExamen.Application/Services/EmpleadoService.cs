using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Application.DTOs;
using VentasExamen.Application.Interfaces;
using VentasExamen.Domain.Entities;

namespace VentasExamen.Application.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpleadoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmpleadoDto>> ObtenerTodosAsync()
        {
            try
            {
                var empleados = await _unitOfWork.Empleados.GetAllAsync();

                return empleados.Select(e => new EmpleadoDto
                {
                    IdEmpleado = e.IdEmpleado,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Estado = e.Estado,
                    NombreCompleto = e.NombreCompleto
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los empleados", ex);
            }
        }

        public async Task<EmpleadoDto?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var empleado = await _unitOfWork.Empleados.GetByIdAsync(id);

                if (empleado == null) return null;

                return new EmpleadoDto
                {
                    IdEmpleado = empleado.IdEmpleado,
                    Nombre = empleado.Nombre,
                    Apellido = empleado.Apellido,
                    Estado = empleado.Estado,
                    NombreCompleto = empleado.NombreCompleto
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el empleado con ID {id}", ex);
            }
        }

        public async Task<EmpleadoDto> CrearAsync(CrearEmpleadoDto empleadoDto)
        {
            try
            {
                var empleado = new Empleado
                {
                    Nombre = empleadoDto.Nombre.Trim(),
                    Apellido = empleadoDto.Apellido.Trim(),
                    Estado = empleadoDto.Estado.ToLower()
                };

                // Validaciones
                if (string.IsNullOrWhiteSpace(empleado.Nombre))
                    throw new ArgumentException("El nombre es requerido");

                if (string.IsNullOrWhiteSpace(empleado.Apellido))
                    throw new ArgumentException("El apellido es requerido");

                if (!new[] { "activo", "inactivo" }.Contains(empleado.Estado))
                    throw new ArgumentException("El estado debe ser 'activo' o 'inactivo'");

                var empleadoCreado = await _unitOfWork.Empleados.AddAsync(empleado);
                await _unitOfWork.SaveChangesAsync();

                return new EmpleadoDto
                {
                    IdEmpleado = empleadoCreado.IdEmpleado,
                    Nombre = empleadoCreado.Nombre,
                    Apellido = empleadoCreado.Apellido,
                    Estado = empleadoCreado.Estado,
                    NombreCompleto = empleadoCreado.NombreCompleto
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al crear el empleado", ex);
            }
        }

        public async Task<EmpleadoDto?> ActualizarAsync(int id, CrearEmpleadoDto empleadoDto)
        {
            try
            {
                var empleadoExistente = await _unitOfWork.Empleados.GetByIdAsync(id);

                if (empleadoExistente == null) return null;

                // Actualizar propiedades
                empleadoExistente.Nombre = empleadoDto.Nombre.Trim();
                empleadoExistente.Apellido = empleadoDto.Apellido.Trim();
                empleadoExistente.Estado = empleadoDto.Estado.ToLower();

                // Validaciones
                if (string.IsNullOrWhiteSpace(empleadoExistente.Nombre))
                    throw new ArgumentException("El nombre es requerido");

                if (string.IsNullOrWhiteSpace(empleadoExistente.Apellido))
                    throw new ArgumentException("El apellido es requerido");

                if (!new[] { "activo", "inactivo" }.Contains(empleadoExistente.Estado))
                    throw new ArgumentException("El estado debe ser 'activo' o 'inactivo'");

                var empleadoActualizado = await _unitOfWork.Empleados.UpdateAsync(empleadoExistente);
                await _unitOfWork.SaveChangesAsync();

                return empleadoActualizado != null ? new EmpleadoDto
                {
                    IdEmpleado = empleadoActualizado.IdEmpleado,
                    Nombre = empleadoActualizado.Nombre,
                    Apellido = empleadoActualizado.Apellido,
                    Estado = empleadoActualizado.Estado,
                    NombreCompleto = empleadoActualizado.NombreCompleto
                } : null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar el empleado con ID {id}", ex);
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var resultado = await _unitOfWork.Empleados.DeleteAsync(id);
                if (resultado)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar el empleado con ID {id}", ex);
            }
        }

        public async Task<IEnumerable<EmpleadoDto>> ObtenerPorEstadoAsync(string estado)
        {
            try
            {
                var empleados = await _unitOfWork.Empleados.ObtenerPorEstadoAsync(estado.ToLower());

                return empleados.Select(e => new EmpleadoDto
                {
                    IdEmpleado = e.IdEmpleado,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Estado = e.Estado,
                    NombreCompleto = e.NombreCompleto
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener empleados con estado {estado}", ex);
            }
        }
    }
}
