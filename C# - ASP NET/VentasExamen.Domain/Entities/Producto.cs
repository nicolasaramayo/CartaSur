using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal PrecioUnitario { get; set; }

        // Propiedades de navegación
        public virtual List<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}

