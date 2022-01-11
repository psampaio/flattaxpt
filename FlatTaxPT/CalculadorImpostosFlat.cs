namespace FlatTaxPT;

public class CalculadorImpostosFlat : ICalculadorImpostosFlat
{
    private const decimal IsencaoBase = 705;
    private const decimal IsencaoPorDependente = 200;
    private const decimal Taxa = 0.15m;

    public SumarioImpostos Calcular(decimal vencimento, int numeroDeDependentes, bool familiaMonoparental)
    {
        var deducao = numeroDeDependentes * IsencaoPorDependente;
        if (familiaMonoparental)
            deducao *= 2;

        return new SumarioImpostos
        {
            VencimentoBase = vencimento,
            Deducao = deducao,
            Tributavel = Math.Max(0, vencimento - IsencaoBase - deducao),
            Taxa = Taxa
        };
    }
}