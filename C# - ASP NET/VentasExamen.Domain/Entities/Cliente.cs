using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required]
        [MaxLength(10)]
        public string Dni { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DireccionEnvio { get; set; } = string.Empty;

        // Propiedades de navegación
        public virtual List<Venta> Ventas { get; set; } = new List<Venta>();

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
