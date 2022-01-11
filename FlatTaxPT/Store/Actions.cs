using FlatTaxPT.Domain;

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
    public CalculateTaxesAction(decimal income, int numberOfDependents, Location location, Category category,
        Situation situation, bool handicapped, bool singleParentFamily)
    {
        Income = income;
        NumberOfDependents = numberOfDependents;
        Location = location;
        Category = category;
        Situation = situation;
        Handicapped = handicapped;
        SingleParentFamily = singleParentFamily;
    }

    public decimal Income { get; }
    public int NumberOfDependents { get; }
    public Location Location { get; }
    public Category Category { get; }
    public Situation Situation { get; }
    public bool Handicapped { get; }
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
    public CalculateProgressiveTaxesAction(decimal income, int numberOfDependents,
        IEnumerable<RetentionTable> retentionTables, Location location, Category category, Situation situation,
        bool handicaped)
        : base(income, numberOfDependents)
    {
        RetentionTables = retentionTables;
        Location = location;
        Category = category;
        Situation = situation;
        Handicaped = handicaped;
    }

    public IEnumerable<RetentionTable> RetentionTables { get; }
    public Location Location { get; }
    public Category Category { get; }
    public Situation Situation { get; }
    public bool Handicaped { get; }
}