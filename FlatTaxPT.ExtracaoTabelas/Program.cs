using System.Text.Json;
using System.Text.Json.Serialization;
using ClosedXML.Excel;
using FlatTaxPT.Domain;

var ficheiroTabelas = "TabelasRetencao.xlsx";

Console.WriteLine("FlatTaxPT: Extração de Tabelas de Retenção");

Console.WriteLine($"A abrir o ficheiro {ficheiroTabelas}...");

var workbook = new XLWorkbook(ficheiroTabelas);

var dados = new List<dynamic>();
foreach (var localizacao in Enum.GetValues<Localizacao>())
foreach (var categoria in Enum.GetValues<Categoria>())
foreach (var deficiente in new[] { false, true })
foreach (var situacao in Enum.GetValues<Situacao>())
    dados.Add(new
    {
        Localizacao = localizacao,
        Categoria = categoria,
        Situacao = situacao,
        Deficiente = deficiente
    });

var tabelas = new List<TabelaDeRetencao>();
for (var index = 0; index < dados.Count; index++)
{
    var d = dados[index];

    if (!workbook.TryGetWorksheet(d.Localizacao.ToString(), out IXLWorksheet worksheet))
        continue;

    var tabela = new TabelaDeRetencao
    {
        Location = d.Localizacao,
        Category = d.Categoria,
        Situation = d.Situacao,
        Handicaped = d.Deficiente
    };

    for (var row = 3; row < 39; row++)
    {
        var vencimento = worksheet.Cell(row, 1).GetValue<decimal>();
        var escalao = new Escalao
        {
            Vencimento = vencimento
        };
        for (var column = 6 * index + 2; column < 6 * index + 8; column++)
        {
            var taxa = worksheet.Cell(row, column).GetValue<decimal>();
            escalao.Taxas.Add(taxa);
        }

        tabela.Escaloes.Add(escalao);
    }

    tabelas.Add(tabela);
}

workbook.Dispose();

var options = new JsonSerializerOptions
{
    Converters =
    {
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    }
};

File.WriteAllText("tabelas_retencao.json", JsonSerializer.Serialize(tabelas, options));
