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

    public CalculatorState(TaxSummary flatTaxes, TaxSummary progressiveTaxes)
    {
        FlatTaxes = flatTaxes;
        ProgressiveTaxes = progressiveTaxes;
    }

    public TaxSummary FlatTaxes { get; }

    public TaxSummary ProgressiveTaxes { get; }
}