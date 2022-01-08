namespace FlatTaxPT;

public interface ICalculadorImpostosProgressivos
{
    SumarioImpostos Calcular(decimal vencimento, Localizacao localizacao, Categoria categoria, Situacao situacao,
        bool deficiente, int numeroDeDependentes);
}