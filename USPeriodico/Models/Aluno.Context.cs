﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPeriodico.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class alunoEntities : DbContext
    {
        public alunoEntities()
            : base("name=alunoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
