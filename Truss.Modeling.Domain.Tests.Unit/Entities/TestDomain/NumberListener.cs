namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public sealed class NumberListener
{
    public int Number { get; set; }
    public Stack<int> Numbers { get; } = new();
}