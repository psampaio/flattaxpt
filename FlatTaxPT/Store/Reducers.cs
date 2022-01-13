using Fluxor;

namespace FlatTaxPT.Store;

public static class Reducers
{
    private const decimal SocialSecurityRate = 0.11m;
    private const decimal CompanySocialSecurityRate = 0.2375m;

    //private const decimal IAS = 443.20m;
    //private const decimal BaseExemption = IAS * 1.5m;
    private const decimal BaseExemption = 705 * 14;
    private const decimal ExemptionPerDependent = 200 * 14;
    private const decimal StandardRate = 0.15m;
    private const decimal TransitionRate = 0.28m;
    private const decimal TransitionIncomeLimit = 30000m;

    private const decimal SpecificDeductions = 4104m;

    [ReducerMethod]
    public static CalculatorState CalculateSocialSecurityAction(CalculatorState state,
        CalculateSocialSecurityCostsAction action)
    {
        var socialSecurity = action.Income * SocialSecurityRate;
        var companyCost = action.Income + action.Income * CompanySocialSecurityRate;

        return new CalculatorState(state.IsSummaryVisible, state.FlatTaxes,
            state.ProgressiveTaxes, socialSecurity, companyCost);
    }

    [ReducerMethod]
    public static CalculatorState CalculateFlatTaxesAction(CalculatorState state, CalculateFlatTaxesAction action)
    {
        var deduction = action.NumberOfDependents * ExemptionPerDependent;
        if (action.SingleParentFamily)
            deduction *= 2;

        var exemption = BaseExemption + deduction;

        var transitionIncome = Math.Max(action.Income - TransitionIncomeLimit, 0);
        var standardIncome = Math.Max(action.Income - transitionIncome - exemption, 0);
        var totalIncome = standardIncome + transitionIncome;

        var rate = totalIncome == 0
            ? 0
            : (standardIncome * StandardRate + transitionIncome * TransitionRate) / totalIncome;

        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Deduction = deduction,
            Taxable = totalIncome,
            Rate = rate
        };

        return new CalculatorState(true, summary, state.ProgressiveTaxes, state.SocialSecurity,
            state.CompanyCost);
    }

    [ReducerMethod]
    public static CalculatorState CalculateProgressiveTaxesAction(CalculatorState state,
        CalculateProgressiveTaxesAction action)
    {
        var totalDeductions = SpecificDeductions + action.Deductions;
        var taxable = Math.Max(0, action.Income - totalDeductions);

        Bracket standardBraket = null;
        Bracket averageBraket = null;
        foreach (var bracket in action.Brackets.OrderBy(b => b.Income).Reverse())
        {
            if (bracket.Income < taxable)
            {
                averageBraket = bracket;
                break;
            }

            standardBraket = bracket;
        }

        var averageRateIncome = averageBraket.Income;
        var standardRateIncome = taxable - averageRateIncome;

        var rate = taxable == 0
            ? 0
            : (averageRateIncome * averageBraket.AverageRate + standardRateIncome * standardBraket.Rate) / taxable;

        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Deduction = totalDeductions,
            Taxable = taxable,
            Rate = rate
        };

        return new CalculatorState(true, state.FlatTaxes, summary, state.SocialSecurity,
            state.CompanyCost);
    }
}