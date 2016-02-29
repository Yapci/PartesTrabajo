using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartesTrabajo.Models
{
    public class parte_trabajo_admin
    {
        public string Empleado { get; set; }
        public string Cliente { get; set; }
        public string Proyecto { get; set; }
        public string Trabajo { get; set; }
        public string Nota { get; set; }
        public long FechaInicial { get; set; }
        public long FechaFinal { get; set; }
        public double Horas { get; set; }
    }
}