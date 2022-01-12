namespace FlatTaxPT.Domain;

public class RetentionTable
{
    public Location Location { get; set; }
    public Category Category { get; set; }
    public Situation Situation { get; set; }
    public bool Handicaped { get; set; }
    public List<Bracket> Brackets { get; set; } = new();

    public decimal GetRate(decimal income, in int numberOfDependents)
    {
        var bracket =
            Brackets.FirstOrDefault(l => income <= l.Vencimento)
            ?? Brackets.LastOrDefault();

        return bracket?.GetRate(numberOfDependents) ?? 0m;
    }
}