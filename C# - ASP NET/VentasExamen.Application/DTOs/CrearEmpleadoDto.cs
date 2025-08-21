using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasExamen.Application.DTOs
{
    public class CrearEmpleadoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Estado { get; set; } = "activo";
    }
}
