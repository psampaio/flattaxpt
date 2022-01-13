using System.ComponentModel.DataAnnotations;

namespace FlatTaxPT;

public class CalculatorModel
{
    [Required(ErrorMessage = "Número de Dependentes é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "Número de dependentes não pode ser negativo")]

    public int NumberOfDependents { get; set; }

    public bool SingleParentFamily { get; set; }

    [Required(ErrorMessage = "Vencimento Base é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Vencimento Base tem de ser positivo")]
    public int Income { get; set; } = 22000;

    public int Deductions { get; set; }
}