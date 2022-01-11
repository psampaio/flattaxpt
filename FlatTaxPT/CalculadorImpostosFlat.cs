namespace FlatTaxPT;

public class CalculadorImpostosFlat : ICalculadorImpostosFlat
{
    private const decimal IsencaoBase = 705;
    private const decimal IsencaoPorDependente = 200;
    private const decimal Taxa = 0.15m;
    private const decimal TaxaTransitoria = 0.28m;
    private const decimal LimiteTransitorio = 30000;

    public SumarioImpostos Calcular(decimal vencimento, int numeroDeDependentes, bool familiaMonoparental)
    {
        var deducao = numeroDeDependentes * IsencaoPorDependente;
        if (familiaMonoparental)
            deducao *= 2;

        var tributavel = Math.Max(0, vencimento - IsencaoBase - deducao);

        decimal vencimentoTransitorio = tributavel > LimiteTransitorio
            ? tributavel - LimiteTransitorio
            : 0;

        var taxa = (Math.Min(tributavel, LimiteTransitorio) * Taxa + vencimentoTransitorio * TaxaTransitoria) / tributavel;
        
        return new SumarioImpostos
        {
            VencimentoBase = vencimento,
            Deducao = deducao,
            Tributavel = tributavel,
            Taxa = taxa
        };
    }
}