namespace FlatTaxPT;

public class SumarioImpostos
{
    public decimal VencimentoBase { get; init; }
    public decimal Tributavel { get; init; }
    public decimal Taxa { get; init; }
    public decimal Retencao => Tributavel * Taxa;
    public decimal VencimentoLiquido => VencimentoBase - Retencao;
    public decimal Deducao { get; set; }
}