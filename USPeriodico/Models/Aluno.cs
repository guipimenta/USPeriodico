//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPeriodico.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Aluno
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int NUSP { get; set; }
        public string Telefone { get; set; }
        public int Curso { get; set; }
        public string Unidade { get; set; }
    
        public virtual Curso Curso1 { get; set; }
    }
}
