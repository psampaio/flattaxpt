using System.Globalization;

namespace FlatTaxPT;

public class TabelaDeRetencao
{
//    private static CultureInfo cultureInfo = CultureInfo.GetCultureInfo("pt-PT");
    private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("es-ES"); //TODO: parsing not working in pt?

    private readonly List<Escalao> escaloes;

    public TabelaDeRetencao(string textoTaxas, Localizacao localizacao, Categoria categoria,
        Situacao situacao,
        bool deficiente)
    {

        this.escaloes = textoTaxas.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(l => l.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(v =>
            {
                return new Escalao
                {
                    Vencimento = Convert.ToDecimal(v.First(), cultureInfo),
                    Taxas = v.Skip(1).Select(r => Convert.ToDecimal(r, cultureInfo)).ToList()
                };
            }).ToList();
        Localizacao = localizacao;
        Categoria = categoria;
        Situacao = situacao;
        Deficiente = deficiente;
    }

    public Localizacao Localizacao { get; }
    public Categoria Categoria { get; }
    public Situacao Situacao { get; }
    public bool Deficiente { get; }

    public decimal ObterTaxa(decimal salary, in int numberOfDependents)
    {
        var escalao =
            this.escaloes.FirstOrDefault(l => salary < l.Vencimento)
            ?? this.escaloes.LastOrDefault();

        return escalao?.ObterTaxa(numberOfDependents) ?? 0m;
    }
}