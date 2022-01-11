using FlatTaxPT.Domain;
using FlatTaxPT.Store;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace FlatTaxPT.Shared;

public partial class Calculador
{
    private readonly CalculatorModel calculatorModel = new();
    private bool avisoVisivel = false;
    private decimal custoParaEmpresa;

    private decimal segurancaSocial;

    private bool sumarioVisivel = false;

    [Inject] private IState<CalculatorState> CalculadorState { get; set; } = null!;
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;

    private TaxSummary TaxSummaryFlat => CalculadorState.Value.FlatTaxes;
    private TaxSummary TaxSummaryProgressivos => CalculadorState.Value.ProgressiveTaxes;

    private void Calcular()
    {
        this.Analytics.TrackEvent("comparar", new { vencimento = this.calculatorModel.Income.ToString() });

        if (this.calculatorModel.Location == Localizacao.Continente)
        {
            this.sumarioVisivel = true;
            this.avisoVisivel = false;

            var action = new CalculateTaxesAction(this.calculatorModel.Income, this.calculatorModel.NumberOfDependents,
                this.calculatorModel.Location, this.calculatorModel.Category, this.calculatorModel.Situation,
                this.calculatorModel.Handicapped, this.calculatorModel.SingleParentFamily);
            Dispatcher.Dispatch(action);

            this.segurancaSocial = this.calculatorModel.Income * this.calculatorModel.TaxaSegurancaSocial;
            this.custoParaEmpresa = this.calculatorModel.Income +
                                    this.calculatorModel.Income * this.calculatorModel.TaxaSegurancaSocialEmpresa;
        }
        else
        {
            this.sumarioVisivel = false;
            this.avisoVisivel = true;
        }
    }
}