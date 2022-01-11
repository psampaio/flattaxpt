using Fluxor;

namespace FlatTaxPT.Store;

[FeatureState]
public class CalculatorState
{
    private CalculatorState()
    {
        FlatTaxes = new TaxSummary();
        ProgressiveTaxes = new TaxSummary();
    }

    public CalculatorState(bool isWarningVisible, bool isSummaryVisible, TaxSummary flatTaxes,
        TaxSummary progressiveTaxes, decimal socialSecurity,
        decimal companyCost)
    {
        FlatTaxes = flatTaxes;
        ProgressiveTaxes = progressiveTaxes;
        SocialSecurity = socialSecurity;
        CompanyCost = companyCost;
        IsWarningVisible = isWarningVisible;
        IsSummaryVisible = isSummaryVisible;
    }

    public bool IsWarningVisible { get; }
    public bool IsSummaryVisible { get; }
    public TaxSummary FlatTaxes { get; }
    public TaxSummary ProgressiveTaxes { get; }
    public decimal SocialSecurity { get; }
    public decimal CompanyCost { get; }
}