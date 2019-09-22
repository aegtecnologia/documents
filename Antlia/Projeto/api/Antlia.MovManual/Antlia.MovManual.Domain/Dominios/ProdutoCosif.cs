using System;
using System.Collections.Generic;
using System.Text;

namespace Antlia.MovManual.Domain.Dominios
{
    public class ProdutoCosif
    {
        public Produto_Cosif()
        {

        }

        public string CodigoProduto { get; set; }
        public string COD_COSIF { get; set; }
        public string COD_CLASSIFICACAO { get; set; }
        public string STA_STATUS { get; set; }
        public ICollection<Movimento_Manual> Contatos { get; set; }
    }
}
