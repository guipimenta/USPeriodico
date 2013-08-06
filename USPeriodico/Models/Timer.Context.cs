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
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class timerEntities : DbContext
    {
        public timerEntities()
            : base("name=timerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Timer> Timer { get; set; }
    
        public virtual int TimerDeletar(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TimerDeletar", idParameter);
        }
    
        public virtual int TimerInsert(Nullable<System.TimeSpan> horario, Nullable<int> frequencia)
        {
            var horarioParameter = horario.HasValue ?
                new ObjectParameter("horario", horario) :
                new ObjectParameter("horario", typeof(System.TimeSpan));
    
            var frequenciaParameter = frequencia.HasValue ?
                new ObjectParameter("frequencia", frequencia) :
                new ObjectParameter("frequencia", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TimerInsert", horarioParameter, frequenciaParameter);
        }
    
        public virtual int TimerUpdate(Nullable<int> id, Nullable<System.TimeSpan> horario, Nullable<int> frequencia)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var horarioParameter = horario.HasValue ?
                new ObjectParameter("horario", horario) :
                new ObjectParameter("horario", typeof(System.TimeSpan));
    
            var frequenciaParameter = frequencia.HasValue ?
                new ObjectParameter("frequencia", frequencia) :
                new ObjectParameter("frequencia", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TimerUpdate", idParameter, horarioParameter, frequenciaParameter);
        }
    }
}
