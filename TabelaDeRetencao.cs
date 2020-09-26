using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FlatTaxPT
{
    public class TabelaDeRetencao
    {
        private static readonly CultureInfo CultureInfo = new CultureInfo("pt-PT");

        private List<Escalao> escaloes;

        public Localizacao Localizacao { get; private set; }
        public Categoria Categoria { get; private set; }
        public Situacao Situacao { get; private set; }
        public bool Deficiente { get; private set; }

        public decimal ObterTaxa(decimal salary, in int numberOfDependents)
        {
            var escalao =
                escaloes.FirstOrDefault(l => salary < l.Vencimento)
                ?? escaloes.LastOrDefault();

            return escalao?.ObterTaxa(numberOfDependents) ?? 0m;
        }

        public static TabelaDeRetencao Processar(string textoTaxas, Localizacao localizacao, Categoria categoria, Situacao situacao,
            bool deficiente)
        {
            return new TabelaDeRetencao
            {
                escaloes = textoTaxas.Split(Environment.NewLine,
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(l => l.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    .Select(v =>
                    {
                        return new Escalao
                        {
                            Vencimento = Convert.ToDecimal(v.First(), CultureInfo),
                            Taxas = v.Skip(1).Select(r => Convert.ToDecimal(r, CultureInfo)).ToList()
                        };
                    }).ToList(),
                Localizacao = localizacao,
                Categoria = categoria,
                Situacao = situacao,
                Deficiente = deficiente
            };
        }
    }
}