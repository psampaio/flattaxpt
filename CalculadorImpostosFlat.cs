namespace FlatTaxPT
{
    public class CalculadorImpostosFlat : ICalculadorImpostosFlat
    {
        private const decimal Isencao = 650;
        private const decimal Taxa = 0.15m;

        public SumarioImpostos Calcular(decimal vencimento)
        {
            return new SumarioImpostos
            {
                VencimentoBase = vencimento,
                Tributavel = vencimento - Isencao,
                Taxa = Taxa
            };
        }
    }
}