//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PartesTrabajo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parte_Trabajo
    {
        public int IdParte_Trabajo { get; set; }
        public Nullable<int> IdParte { get; set; }
        public Nullable<int> IdProyecto { get; set; }
        public Nullable<System.DateTime> FechaInicial { get; set; }
        public Nullable<System.DateTime> FechaFinal { get; set; }
        public string Nota { get; set; }
        public Nullable<int> IdTrabajo { get; set; }
        public Nullable<int> IdCliente { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Parte Parte { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual Trabajo Trabajo { get; set; }
    }
}
