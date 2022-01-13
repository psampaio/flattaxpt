namespace FlatTaxPT.Store;

public class CalculateSocialSecurityCostsAction
{
    public CalculateSocialSecurityCostsAction(decimal income)
    {
        Income = income;
    }

    public decimal Income { get; }
}

public class CalculateTaxesAction
{
    public CalculateTaxesAction(decimal income, int deductions, int numberOfDependents, bool singleParentFamily)
    {
        Income = income;
        Deductions = deductions;
        NumberOfDependents = numberOfDependents;
        SingleParentFamily = singleParentFamily;
    }

    public decimal Income { get; }
    public int Deductions { get; }
    public int NumberOfDependents { get; }
    public bool SingleParentFamily { get; }
}

public abstract class CalculateSpecificTaxesActionBase
{
    protected CalculateSpecificTaxesActionBase(decimal income, int numberOfDependents)
    {
        Income = income;
        NumberOfDependents = numberOfDependents;
    }

    public decimal Income { get; }
    public int NumberOfDependents { get; set; }
}

public class CalculateFlatTaxesAction : CalculateSpecificTaxesActionBase
{
    public CalculateFlatTaxesAction(decimal income, int numberOfDependents, bool singleParentFamily) : base(income,
        numberOfDependents)
    {
        SingleParentFamily = singleParentFamily;
    }

    public bool SingleParentFamily { get; }
}

public class CalculateProgressiveTaxesAction : CalculateSpecificTaxesActionBase
{
    public CalculateProgressiveTaxesAction(decimal income, int deductions, int numberOfDependents,
        List<Bracket> brackets)
        : base(income, numberOfDependents)
    {
        Deductions = deductions;
        Brackets = brackets;
    }

    public int Deductions { get; }
    public IEnumerable<Bracket> Brackets { get; }
}