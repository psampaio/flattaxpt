using System.Collections.Generic;
using System.Linq;
using FlatTaxPT.Properties;

namespace FlatTaxPT
{
    public class CalculadorImpostosProgressivos : ICalculadorImpostosProgressivos
    {
        private readonly List<TabelaDeRetencao> retentionTables;

        public CalculadorImpostosProgressivos()
        {
            retentionTables = new List<TabelaDeRetencao>
            {
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_NaoCasado, Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.NaoCasado, false),
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_CasadoUnico, Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.CasadoUnico, false),
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_CasadoDois, Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.CasadoDois, false),
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_NaoCasado_Deficiente, Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.NaoCasado, true),
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_CasadoUnico_Deficiente,
                    Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.CasadoUnico, true),
                TabelaDeRetencao.Processar(Resources.Continente_Dependente_CasadoDois_Deficiente,
                    Localizacao.Continente,
                    Categoria.TrabalhadorDependente, Situacao.CasadoDois, true),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_NaoCasado, Localizacao.Açores,
                    Categoria.TrabalhadorDependente,
                    Situacao.NaoCasado, false),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_CasadoUnico, Localizacao.Açores,
                    Categoria.TrabalhadorDependente,
                    Situacao.CasadoUnico, false),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_CasadoDois, Localizacao.Açores,
                    Categoria.TrabalhadorDependente,
                    Situacao.CasadoDois, false),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_NaoCasado_Deficiente, Localizacao.Açores,
                    Categoria.TrabalhadorDependente, Situacao.NaoCasado, true),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_CasadoUnico_Deficiente, Localizacao.Açores,
                    Categoria.TrabalhadorDependente, Situacao.CasadoUnico, true),
                TabelaDeRetencao.Processar(Resources.Acores_Dependente_CasadoDois_Deficiente, Localizacao.Açores,
                    Categoria.TrabalhadorDependente, Situacao.CasadoDois, true),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_NaoCasado, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente,
                    Situacao.NaoCasado, false),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_CasadoUnico, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente, Situacao.CasadoUnico, false),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_CasadoDois, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente, Situacao.CasadoDois, false),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_NaoCasado_Deficiente, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente, Situacao.NaoCasado, true),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_CasadoUnico_Deficiente, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente, Situacao.CasadoUnico, true),
                TabelaDeRetencao.Processar(Resources.Madeira_Dependente_CasadoDois_Deficiente, Localizacao.Madeira,
                    Categoria.TrabalhadorDependente, Situacao.CasadoDois, true)
            };
        }

        public SumarioImpostos Calcular(decimal vencimento, Localizacao localizacao, Categoria categoria,
            Situacao situacao, bool deficiente, int numeroDeDependentes)
        {
            var tabelaDeRetencao = retentionTables.FirstOrDefault(t =>
                t.Localizacao == localizacao && t.Categoria == categoria && t.Situacao == situacao &&
                t.Deficiente == deficiente);

            if (tabelaDeRetencao == null) return null;

            var taxa = tabelaDeRetencao.ObterTaxa(vencimento, numeroDeDependentes);
            return new SumarioImpostos
            {
                VencimentoBase = vencimento,
                Tributavel = vencimento,
                Taxa = taxa
            };
        }
    }
}