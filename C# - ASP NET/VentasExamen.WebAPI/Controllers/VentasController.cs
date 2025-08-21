using Microsoft.AspNetCore.Mvc;
using VentasExamen.Application.Interfaces;

namespace VentasExamen.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly ILogger<VentasController> _logger;

        public VentasController(IVentaService ventaService, ILogger<VentasController> logger)
        {
            _ventaService = ventaService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la fecha con más ventas (Punto 2 del examen)
        /// </summary>
        /// <returns>Fecha con mayor cantidad de ventas</returns>
        [HttpGet("fecha-mas-ventas")]
        public async Task<IActionResult> ObtenerFechaConMasVentas()
        {
            try
            {
                _logger.LogInformation("Obteniendo fecha con más ventas");

                var fechaConMasVentas = await _ventaService.ObtenerFechaConMasVentasAsync();

                if (fechaConMasVentas == null)
                {
                    _logger.LogWarning("No se encontraron ventas en la base de datos");
                    return NotFound(new { mensaje = "No se encontraron ventas registradas" });
                }

                _logger.LogInformation("Fecha con más ventas obtenida exitosamente: {Fecha} con {Cantidad} ventas",
                    fechaConMasVentas.Fecha, fechaConMasVentas.CantidadVentas);

                return Ok(new
                {
                    fecha = fechaConMasVentas.Fecha.ToString("yyyy-MM-dd"),
                    cantidadVentas = fechaConMasVentas.CantidadVentas,
                    mensaje = $"La fecha con más ventas es {fechaConMasVentas.Fecha:dd/MM/yyyy} con {fechaConMasVentas.CantidadVentas} ventas"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la fecha con más ventas");
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene todas las ventas
        /// </summary>
        /// <returns>Lista de todas las ventas</returns>
        [HttpGet]
        public async Task<IActionResult> ObtenerVentas()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las ventas");

                var ventas = await _ventaService.ObtenerVentasAsync();

                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ventas");
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una venta por ID
        /// </summary>
        /// <param name="id">ID de la venta</param>
        /// <returns>Venta específica</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVentaPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "El ID debe ser mayor a 0" });
                }

                _logger.LogInformation("Obteniendo venta con ID {Id}", id);

                var venta = await _ventaService.ObtenerVentaPorIdAsync(id);

                if (venta == null)
                {
                    _logger.LogWarning("No se encontró la venta con ID {Id}", id);
                    return NotFound(new { mensaje = $"No se encontró la venta con ID {id}" });
                }

                return Ok(venta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la venta con ID {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
