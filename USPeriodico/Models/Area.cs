//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Area
    {
        public Area()
        {
            this.Estagio = new HashSet<Estagio>();
        }
    
        public int ID { get; set; }
        public string Nome { get; set; }
    
        public virtual Area Area1 { get; set; }
        public virtual Area Area2 { get; set; }
        public virtual ICollection<Estagio> Estagio { get; set; }
    }
}
