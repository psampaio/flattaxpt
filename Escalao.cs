using System.Collections.Generic;
using System.Linq;

namespace FlatTaxPT
{
    public class Escalao
    {
        public decimal Vencimento { get; set; }
        public List<decimal> Taxas { get; set; }

        public decimal ObterTaxa(in int numeroDeDependentes)
        {
            return Taxas.ElementAtOrDefault(numeroDeDependentes);
        }
    }
}