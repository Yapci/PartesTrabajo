﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PartesEntities : DbContext
    {
        public PartesEntities()
            : base("name=PartesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleadoes { get; set; }
        public DbSet<Parte> Partes { get; set; }
        public DbSet<Parte_Trabajo> Parte_Trabajo { get; set; }
        public DbSet<Proyecto> Proyectoes { get; set; }
        public DbSet<Trabajo> Trabajoes { get; set; }
    }
}