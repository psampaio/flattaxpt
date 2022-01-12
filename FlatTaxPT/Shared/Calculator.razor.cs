using FlatTaxPT.Store;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace FlatTaxPT.Shared;

public partial class Calculator
{
    private readonly CalculatorModel calculatorModel = new();

    [Inject] private IState<CalculatorState> CalculadorState { get; set; } = null!;
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;

    private bool IsWarningVisible => CalculadorState.Value.IsWarningVisible;
    private bool IsSummaryVisible => CalculadorState.Value.IsSummaryVisible;
    private TaxSummary TaxSummaryFlat => CalculadorState.Value.FlatTaxes;
    private TaxSummary TaxSummaryProgressivos => CalculadorState.Value.ProgressiveTaxes;
    public decimal SocialSecurity => CalculadorState.Value.SocialSecurity;
    public decimal CompanyCost => CalculadorState.Value.CompanyCost;
    public decimal RaiseInEuros => TaxSummaryFlat.NetIncome - TaxSummaryProgressivos.NetIncome;

    public decimal RaiseInPercentage =>
        TaxSummaryProgressivos.NetIncome == 0 ? 0 : RaiseInEuros / TaxSummaryProgressivos.NetIncome;

    public string SocialSharingMessage => $"Com a Flat Tax da Iniciativa Liberal, vou ganhar mais {RaiseInPercentage:P2} por mês do que com os atuais escalões de IRS";

    private void Calcular()
    {
        this.Analytics.TrackEvent("calculate", "general", eventValue: this.calculatorModel.Income);

        var action = new CalculateTaxesAction(this.calculatorModel.Income, this.calculatorModel.NumberOfDependents,
            this.calculatorModel.Location, this.calculatorModel.Category, this.calculatorModel.Situation,
            this.calculatorModel.Handicapped, this.calculatorModel.SingleParentFamily);
        Dispatcher.Dispatch(action);
    }
}