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
    
    public partial class Usuarios
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }

        public virtual Roles Roles { get; set; }
        public virtual Aluno Aluno { get; set; }
    }
}
