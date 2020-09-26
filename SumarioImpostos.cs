namespace FlatTaxPT
{
    public class SumarioImpostos
    {
        public decimal VencimentoBase { get; set; }
        public decimal Tributavel { get; set; }
        public decimal Taxa { get; set; }
        public decimal Retencao => Tributavel * Taxa;

        public decimal VencimentoLiquido => VencimentoBase - Retencao;
    }
}