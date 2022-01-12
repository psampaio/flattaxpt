namespace FlatTaxPT;

public class TaxSummary
{
    public decimal BaseIncome { get; init; }
    public decimal Taxable { get; init; }
    public decimal Deduction { get; init; }
    public decimal Rate { get; set; }
    public decimal Retention => Taxable * Rate;
    public decimal NetIncome => BaseIncome - Retention;
}