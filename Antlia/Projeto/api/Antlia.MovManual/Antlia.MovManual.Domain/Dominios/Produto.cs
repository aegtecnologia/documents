using System;
using System.Collections.Generic;
using System.Text;

namespace Antlia.MovManual.Domain.Dominios
{
    public class Produto
    {
        public Produto()
        {

        }

        public string Codigo { get; set; }
        public string Descritao { get; set; }
        public string Status { get; set; }
        public ICollection<Produto_Cosif> Produto_Cosifs { get; set; }

        
    }
}
