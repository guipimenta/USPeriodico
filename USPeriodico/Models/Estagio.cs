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
    
    public partial class Estagio
    {
        public int ID { get; set; }
        public int EmpresaID { get; set; }
        public string Descricao { get; set; }
        public string BreveDescricao { get; set; }
        public System.DateTime DataInicio { get; set; }
        public Nullable<int> Duracao { get; set; }
        public string Bolsa { get; set; }
        public int Area { get; set; }
        public bool Valido { get; set; }
        public byte[] UltimaAlteracao { get; set; }
        public string ImageLink { get; set; }
    }
}
