using System.ComponentModel.DataAnnotations;

namespace FlatTaxPT
{
    public class DadosCalculo
    {
        public Localizacao Localizacao { get; set; } = Localizacao.Continente;
        public Situacao Situacao { get; set; } = Situacao.NaoCasado;

        [Required(ErrorMessage = "Número de Dependentes é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Número de dependentes não pode ser negativo")]
        public int Dependentes { get; set; } = 0;

        [Required(ErrorMessage = "Vencimento Base é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Vencimento Base tem de ser positivo")]
        public int VencimentoBase { get; set; } = 900;
        public decimal TaxaSegurancaSocial { get; set; } = 0.11m;
        public decimal TaxaSegurancaSocialEmpresa { get; set; } = 0.2375m;
    }
}