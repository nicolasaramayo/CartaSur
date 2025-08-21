using Microsoft.AspNetCore.Mvc;
using VentasExamen.Application.DTOs;
using VentasExamen.Application.Interfaces;

namespace VentasExamen.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(IEmpleadoService empleadoService, ILogger<EmpleadosController> logger)
        {
            _empleadoService = empleadoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los empleados
        /// </summary>
        /// <returns>Lista de empleados</returns>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los empleados");

                var empleados = await _empleadoService.ObtenerTodosAsync();

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los empleados");
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene empleados por estado (activo/inactivo)
        /// </summary>
        /// <param name="estado">Estado del empleado</param>
        /// <returns>Lista de empleados filtrada por estado</returns>
        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> ObtenerPorEstado(string estado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(estado))
                {
                    return BadRequest(new { mensaje = "El estado es requerido" });
                }

                if (!new[] { "activo", "inactivo" }.Contains(estado.ToLower()))
                {
                    return BadRequest(new { mensaje = "El estado debe ser 'activo' o 'inactivo'" });
                }

                _logger.LogInformation("Obteniendo empleados con estado {Estado}", estado);

                var empleados = await _empleadoService.ObtenerPorEstadoAsync(estado);

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados por estado {Estado}", estado);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un empleado por ID
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <returns>Empleado específico</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "El ID debe ser mayor a 0" });
                }

                _logger.LogInformation("Obteniendo empleado con ID {Id}", id);

                var empleado = await _empleadoService.ObtenerPorIdAsync(id);

                if (empleado == null)
                {
                    _logger.LogWarning("No se encontró el empleado con ID {Id}", id);
                    return NotFound(new { mensaje = $"No se encontró el empleado con ID {id}" });
                }

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el empleado con ID {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo empleado
        /// </summary>
        /// <param name="empleadoDto">Datos del empleado a crear</param>
        /// <returns>Empleado creado</returns>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEmpleadoDto empleadoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creando nuevo empleado: {Nombre} {Apellido}",
                    empleadoDto.Nombre, empleadoDto.Apellido);

                var empleadoCreado = await _empleadoService.CrearAsync(empleadoDto);

                _logger.LogInformation("Empleado creado exitosamente con ID {Id}", empleadoCreado.IdEmpleado);

                return CreatedAtAction(nameof(ObtenerPorId),
                    new { id = empleadoCreado.IdEmpleado }, empleadoCreado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear empleado");
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un empleado existente
        /// </summary>
        /// <param name="id">ID del empleado a actualizar</param>
        /// <param name="empleadoDto">Nuevos datos del empleado</param>
        /// <returns>Empleado actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CrearEmpleadoDto empleadoDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "El ID debe ser mayor a 0" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Actualizando empleado con ID {Id}", id);

                var empleadoActualizado = await _empleadoService.ActualizarAsync(id, empleadoDto);

                if (empleadoActualizado == null)
                {
                    _logger.LogWarning("No se encontró el empleado con ID {Id} para actualizar", id);
                    return NotFound(new { mensaje = $"No se encontró el empleado con ID {id}" });
                }

                _logger.LogInformation("Empleado con ID {Id} actualizado exitosamente", id);

                return Ok(empleadoActualizado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar empleado con ID {Id}", id);
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empleado con ID {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un empleado
        /// </summary>
        /// <param name="id">ID del empleado a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "El ID debe ser mayor a 0" });
                }

                _logger.LogInformation("Eliminando empleado con ID {Id}", id);

                var eliminado = await _empleadoService.EliminarAsync(id);

                if (!eliminado)
                {
                    _logger.LogWarning("No se encontró el empleado con ID {Id} para eliminar", id);
                    return NotFound(new { mensaje = $"No se encontró el empleado con ID {id}" });
                }

                _logger.LogInformation("Empleado con ID {Id} eliminado exitosamente", id);

                return Ok(new { mensaje = $"Empleado con ID {id} eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empleado con ID {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
