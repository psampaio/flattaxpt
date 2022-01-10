namespace FlatTaxPT.Domain;

public class Escalao
{
    public decimal Vencimento { get; set; }
    public IList<decimal> Taxas { get; init; } = new List<decimal>();

    public decimal ObterTaxa(in int numeroDeDependentes)
    {
        return numeroDeDependentes >= Taxas.Count()
            ? Taxas.LastOrDefault()
            : Taxas.ElementAtOrDefault(numeroDeDependentes);
    }
}