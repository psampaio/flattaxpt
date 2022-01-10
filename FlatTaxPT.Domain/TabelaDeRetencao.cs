namespace FlatTaxPT.Domain;

public class TabelaDeRetencao
{
    public Localizacao Localizacao { get; set; }
    public Categoria Categoria { get; set; }
    public Situacao Situacao { get; set; }
    public bool Deficiente { get; set; }
    public List<Escalao> Escaloes { get; set; } = new();

    public decimal ObterTaxa(decimal salary, in int numberOfDependents)
    {
        var escalao =
            Escaloes.FirstOrDefault(l => salary < l.Vencimento)
            ?? Escaloes.LastOrDefault();

        return escalao?.ObterTaxa(numberOfDependents) ?? 0m;
    }
}