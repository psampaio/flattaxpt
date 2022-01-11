using System.ComponentModel.DataAnnotations;
using FlatTaxPT.Domain;

namespace FlatTaxPT;

public class CalculatorModel
{
    public Location Location { get; set; } = Location.Continente;
    public Situation Situation { get; set; } = Situation.NaoCasado;
    public Category Category { get; }

    [Required(ErrorMessage = "Número de Dependentes é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "Número de dependentes não pode ser negativo")]

    public int NumberOfDependents { get; set; }

    public bool Handicapped { get; }
    public bool SingleParentFamily { get; set; }

    [Required(ErrorMessage = "Vencimento Base é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Vencimento Base tem de ser positivo")]
    public int Income { get; set; } = 900;
}