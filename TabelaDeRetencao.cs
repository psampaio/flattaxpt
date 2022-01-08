using System.Globalization;

namespace FlatTaxPT
{
    public class TabelaDeRetencao
    {
        private List<Escalao> Escaloes;

        public Localizacao Localizacao { get; private set; }
        public Categoria Categoria { get; private set; }
        public Situacao Situacao { get; private set; }
        public bool Deficiente { get; private set; }

        public decimal ObterTaxa(decimal salary, in int numberOfDependents)
        {
            var escalao =
                this.Escaloes.FirstOrDefault(l => salary < l.Vencimento)
                ?? this.Escaloes.LastOrDefault();

            return escalao?.ObterTaxa(numberOfDependents) ?? 0m;
        }

        public static TabelaDeRetencao Processar(string textoTaxas, Localizacao localizacao, Categoria categoria, Situacao situacao,
            bool deficiente)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("pt-PT");
            cultureInfo = CultureInfo.GetCultureInfo("es-ES");

            var tabelaDeRetencao = new TabelaDeRetencao
            {
                Escaloes = textoTaxas.Split(Environment.NewLine,
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(l => l.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    .Select(v =>
                    {
                        return new Escalao
                        {
                            Vencimento = Convert.ToDecimal(v.First(), cultureInfo),
                            Taxas = v.Skip(1).Select(r => Convert.ToDecimal(r, cultureInfo)).ToList()
                        };
                    }).ToList(),
                Localizacao = localizacao,
                Categoria = categoria,
                Situacao = situacao,
                Deficiente = deficiente
            };

            return tabelaDeRetencao;
        }
    }
}