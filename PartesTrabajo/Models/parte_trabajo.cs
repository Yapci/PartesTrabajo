using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartesTrabajo.Models
{
    public class parte_trabajo
    {
        public string Cliente { get; set; }
        public string Proyecto { get; set; }
        public string Trabajo { get; set; }
        public string Nota { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public double Horas { get; set; }
    }
}