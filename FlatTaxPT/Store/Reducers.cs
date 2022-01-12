﻿using FlatTaxPT.Domain;
using Fluxor;

namespace FlatTaxPT.Store;

public static class Reducers
{
    private const decimal SocialSecurityRate = 0.11m;
    private const decimal CompanySocialSecurityRate = 0.2375m;

    //private const decimal IAS = 443.20m;
    //private const decimal BaseExemption = IAS * 1.5m;
    private const decimal BaseExemption = 705;
    private const decimal ExemptionPerDependent = 200;
    private const decimal StandardRate = 0.15m;
    private const decimal TransitionRate = 0.28m;
    private const decimal TransitionIncomeLimit = 30000m / 14;


    [ReducerMethod]
    public static CalculatorState CalculateSocialSecurityAction(CalculatorState state,
        CalculateSocialSecurityCostsAction action)
    {
        var socialSecurity = action.Income * SocialSecurityRate;
        var companyCost = action.Income + action.Income * CompanySocialSecurityRate;

        return new CalculatorState(state.IsWarningVisible, state.IsSummaryVisible, state.FlatTaxes,
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

        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Deduction = deduction,
            Taxable = action.Income - exemption,
            Rate = (standardIncome * StandardRate + transitionIncome * TransitionRate) /
                   (standardIncome + transitionIncome)
        };

        return new CalculatorState(state.IsWarningVisible, true, summary, state.ProgressiveTaxes, state.SocialSecurity,
            state.CompanyCost);
    }

    [ReducerMethod]
    public static CalculatorState CalculateProgressiveTaxesAction(CalculatorState state,
        CalculateProgressiveTaxesAction action)
    {
        var retentionTable = action.RetentionTables.FirstOrDefault(t =>
            t.Location == action.Location && t.Category == action.Category && t.Situation == action.Situation &&
            t.Handicaped == action.Handicaped) ?? new RetentionTable();

        var rate = retentionTable.GetRate(action.Income, action.NumberOfDependents);
        var summary = new TaxSummary
        {
            BaseIncome = action.Income,
            Taxable = action.Income,
            Rate = rate
        };

        var isWarningVisible = action.Location == Location.Acores || action.Location == Location.Madeira;
        var isSummaryVisible = action.Location == Location.Continente;

        return new CalculatorState(isWarningVisible, isSummaryVisible, state.FlatTaxes, summary, state.SocialSecurity,
            state.CompanyCost);
    }
}