using ClosedXML.Excel;

const string ficheiroTabelas = "TabelasRetencao.xlsx";

Console.WriteLine("FlatTaxPT: Extração de Tabelas de Retenção");

Console.WriteLine("A abrir o ficheiro...");

var workbook = new XLWorkbook(ficheiroTabelas);

//var worksheet = workbook.Worksheet("Data");

workbook.Dispose();