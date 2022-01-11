namespace FlatTaxPT.Domain;

public class TabelaDeRetencao
{
    public Localizacao Location { get; set; }
    public Categoria Category { get; set; }
    public Situacao Situation { get; set; }
    public bool Handicaped { get; set; }
    public List<Escalao> Escaloes { get; set; } = new();

    public decimal GetRate(decimal salary, in int numberOfDependents)
    {
        var escalao =
            Escaloes.FirstOrDefault(l => salary < l.Vencimento)
            ?? Escaloes.LastOrDefault();

        return escalao?.ObterTaxa(numberOfDependents) ?? 0m;
    }
}