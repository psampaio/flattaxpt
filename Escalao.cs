namespace FlatTaxPT;

public class Escalao
{
    public decimal Vencimento { get; init; }
    public IEnumerable<decimal> Taxas { get; init; } = Enumerable.Empty<decimal>();

    public decimal ObterTaxa(in int numeroDeDependentes)
    {
        return numeroDeDependentes >= Taxas.Count()
            ? Taxas.LastOrDefault()
            : Taxas.ElementAtOrDefault(numeroDeDependentes);
    }
}