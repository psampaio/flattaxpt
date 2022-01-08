namespace FlatTaxPT;

public interface ICalculadorImpostosFlat
{
    SumarioImpostos Calcular(decimal vencimento, int numeroDeDependentes);
}