using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class Venta
    {
        public int IdVenta { get; set; }

        [Required]
        public DateTime FechaVenta { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdSucursal { get; set; }

        [Required]
        public decimal ImporteTotal { get; set; }

        // Propiedades de navegación
        public virtual Empleado Empleado { get; set; } = null!;
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}
