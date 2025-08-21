using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }

        [Required]
        public int IdVenta { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        public virtual Venta Venta { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
