using Fluxor;

namespace FlatTaxPT.Store;

public static class Reducers
{
    private const decimal SocialSecurityRate = 0.11m;
    private const decimal CompanySocialSecurityRate = 0.2375m;

    //private const decimal IAS = 443.20m;
    //private const decimal BaseExemption = IAS * 1.5m * 14;
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
            Deductions = deduction,
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
        var taxable = Math.Max(0, action.Income - SpecificDeductions);

        Bracket? standardBracket = null;
        Bracket? averageBracket = null;
        foreach (var bracket in action.Brackets.OrderBy(b => b.Income).Reverse())
        {
            if (bracket.Income < taxable)
            {
                averageBracket = bracket;
                break;
            }

            standardBracket = bracket;
        }

        decimal rate;
        if (taxable == 0 || standardBracket == null)
            rate = 0;
        else if (averageBracket == null)
            rate = taxable * standardBracket.Rate / taxable;
        else
            rate = (averageBracket.Income * averageBracket.AverageRate +
                    (taxable - averageBracket.Income) * standardBracket.Rate) / taxable;

        int dependentDeductions = action.NumberOfDependents switch
        {
            0 => 0,
            1 => 600,
            _ => 600 + 900 * (action.NumberOfDependents - 1)
        };

        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Deductions = SpecificDeductions,
            TaxDeductions = action.Deductions + dependentDeductions,
            Taxable = taxable,
            Rate = rate
        };


        return new CalculatorState(true, state.FlatTaxes, summary, state.SocialSecurity,
            state.CompanyCost);
    }
}