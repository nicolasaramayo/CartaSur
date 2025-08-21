using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Application.DTOs
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal ImporteTotal { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string NombreSucursal { get; set; } = string.Empty;
    }
}
