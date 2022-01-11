using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlatTaxPT.Domain;

namespace FlatTaxPT;

public class CalculadorImpostosProgressivos : ICalculadorImpostosProgressivos
{
    private readonly HttpClient httpClient;

    private List<TabelaDeRetencao> tabelasRetencao = new();

    public CalculadorImpostosProgressivos(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public SumarioImpostos Calcular(decimal vencimento, Localizacao localizacao, Categoria categoria,
        Situacao situacao, bool deficiente, int numeroDeDependentes)
    {
        var tabelaDeRetencao = this.tabelasRetencao.FirstOrDefault(t =>
            t.Localizacao == localizacao && t.Categoria == categoria && t.Situacao == situacao &&
            t.Deficiente == deficiente);

        if (tabelaDeRetencao == null) return new SumarioImpostos();

        var taxa = tabelaDeRetencao.ObterTaxa(vencimento, numeroDeDependentes);
        return new SumarioImpostos
        {
            VencimentoBase = vencimento,
            Tributavel = vencimento,
            Taxa = taxa
        };
    }

    public async Task CarregaTabelasRetencao()
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        this.tabelasRetencao =
            await this.httpClient.GetFromJsonAsync<List<TabelaDeRetencao>>("data/tabelas_retencao.json", options) ??
            new List<TabelaDeRetencao>();
    }
}