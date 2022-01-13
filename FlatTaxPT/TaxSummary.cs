namespace FlatTaxPT;

public class TaxSummary
{
    public decimal BaseIncome { get; init; }
    public decimal Taxable { get; init; }
    public decimal Deductions { get; init; }
    public decimal Rate { get; init; }
    public decimal NetIncome => BaseIncome - Taxes + TaxDeductions;
    public decimal EffectiveRate => BaseIncome == 0 ? 0 : 1 - NetIncome / BaseIncome;
    public decimal TaxDeductions { get; init; }
    public decimal EffectiveTaxes => Math.Max(0, Taxes - TaxDeductions);
    private decimal Taxes => Math.Floor(Taxable * Rate);
}