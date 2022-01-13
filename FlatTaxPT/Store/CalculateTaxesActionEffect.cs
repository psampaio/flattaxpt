using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    private List<Bracket>? brakets;

    public CalculateTaxesActionEffect(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public override async Task HandleAsync(CalculateTaxesAction action, IDispatcher dispatcher)
    {
        var calculateSocialSecurityAction = new CalculateSocialSecurityCostsAction(action.Income);
        dispatcher.Dispatch(calculateSocialSecurityAction);

        var calculateFlatTaxesAction =
            new CalculateFlatTaxesAction(action.Income, action.NumberOfDependents, action.SingleParentFamily);
        dispatcher.Dispatch(calculateFlatTaxesAction);

        this.brakets ??= await this.httpClient.GetFromJsonAsync<List<Bracket>>("data/brackets.json", this.options);
        var calculateProgressiveTaxesAction =
            new CalculateProgressiveTaxesAction(action.Income, action.Deductions, action.NumberOfDependents,
                this.brakets!);
        dispatcher.Dispatch(calculateProgressiveTaxesAction);
    }
}