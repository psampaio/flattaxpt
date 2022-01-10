namespace FlatTaxPT;

public class CalculadorImpostosFlat : ICalculadorImpostosFlat
{
    private const decimal IsencaoBase = 650;
    private const decimal IsencaoPorDependente = 200;
    private const decimal Taxa = 0.15m;

    public SumarioImpostos Calcular(decimal vencimento, int numeroDeDependentes)
    {
        return new SumarioImpostos
        {
            VencimentoBase = vencimento,
            Tributavel = Math.Max(0, vencimento - IsencaoBase - numeroDeDependentes * IsencaoPorDependente),
            Taxa = Taxa
        };
    }
}