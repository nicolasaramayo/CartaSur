using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Domain.Entities
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Direccion { get; set; } = string.Empty;

        // Propiedades de navegación
        public virtual List<Venta> Ventas { get; set; } = new List<Venta>();
    }
}

