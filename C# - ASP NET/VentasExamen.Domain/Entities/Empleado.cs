using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Estado { get; set; } = "activo"; // activo/inactivo

        
        public virtual List<Venta> Ventas { get; set; } = new List<Venta>();

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
