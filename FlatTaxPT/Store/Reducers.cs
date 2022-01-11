using Fluxor;

namespace FlatTaxPT.Store;

public static class Reducers
{
    private const decimal BaseExemption = 705;
    private const decimal ExemptionPerDependent = 200;
    private const decimal StandardRate = 0.15m;
    private const decimal TransitionRate = 0.28m;
    private const decimal TransitionIncomeLimit = 30000;

    [ReducerMethod]
    public static CalculatorState CalculateFlatTaxesAction(CalculatorState state, CalculateFlatTaxesAction action)
    {
        var deduction = action.NumberOfDependents * ExemptionPerDependent;
        if (action.SingleParentFamily)
            deduction *= 2;

        var taxable = Math.Max(0, action.Income - BaseExemption - deduction);

        var transitionIncome = taxable > TransitionIncomeLimit
            ? taxable - TransitionIncomeLimit
            : 0;

        var effectiveRate =
            (Math.Min(taxable, TransitionIncomeLimit) * StandardRate + transitionIncome * TransitionRate) / taxable;

        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Deduction = deduction,
            Taxable = taxable,
            Rate = effectiveRate
        };

        return new CalculatorState(summary, state.ProgressiveTaxes);
    }

    [ReducerMethod]
    public static CalculatorState CalculateProgressiveTaxesAction(CalculatorState state,
        CalculateProgressiveTaxesAction action)
    {
        var retentionTable = action.RetentionTables.FirstOrDefault(t =>
            t.Location == action.Location && t.Category == action.Category && t.Situation == action.Situation &&
            t.Handicaped == action.Handicaped);

        if (retentionTable == null) return new CalculatorState(state.FlatTaxes, new TaxSummary());

        var rate = retentionTable.GetRate(action.Income, action.NumberOfDependents);
        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Taxable = action.Income,
            Rate = rate
        };

        return new CalculatorState(state.FlatTaxes, summary);
    }
}