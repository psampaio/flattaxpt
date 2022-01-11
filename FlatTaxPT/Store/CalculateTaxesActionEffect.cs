using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlatTaxPT.Domain;
using Fluxor;

namespace FlatTaxPT.Store;

public class CalculateTaxesActionEffect : Effect<CalculateTaxesAction>
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    private List<TabelaDeRetencao>? tabelasRetencao;

    public CalculateTaxesActionEffect(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public override async Task HandleAsync(CalculateTaxesAction action, IDispatcher dispatcher)
    {
        this.tabelasRetencao ??=
            await this.httpClient.GetFromJsonAsync<List<TabelaDeRetencao>>("data/tabelas_retencao.json", this.options);

        var calculateFlatTaxesAction =
            new CalculateFlatTaxesAction(action.Income, action.NumberOfDependents, action.SingleParentFamily);
        dispatcher.Dispatch(calculateFlatTaxesAction);

        var calculateProgressiveTaxesAction = new CalculateProgressiveTaxesAction(action.Income,
            action.NumberOfDependents, this.tabelasRetencao!, action.Location, action.Category, action.Situation,
            action.Handicapped);
        dispatcher.Dispatch(calculateProgressiveTaxesAction);
    }
}