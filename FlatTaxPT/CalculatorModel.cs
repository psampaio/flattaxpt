using System.ComponentModel.DataAnnotations;
using FlatTaxPT.Domain;

namespace FlatTaxPT;

public class CalculatorModel
{
    public Localizacao Location { get; set; } = Localizacao.Continente;
    public Situacao Situation { get; set; } = Situacao.NaoCasado;
    public Categoria Category { get; }

    [Required(ErrorMessage = "Número de Dependentes é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "Número de dependentes não pode ser negativo")]

    public int NumberOfDependents { get; set; }
    public bool Handicapped { get; }
    public bool SingleParentFamily { get; set; }

    [Required(ErrorMessage = "Vencimento Base é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Vencimento Base tem de ser positivo")]
    public int Income { get; set; } = 900;

    public decimal TaxaSegurancaSocial { get; set; } = 0.11m;
    public decimal TaxaSegurancaSocialEmpresa { get; set; } = 0.2375m;
}