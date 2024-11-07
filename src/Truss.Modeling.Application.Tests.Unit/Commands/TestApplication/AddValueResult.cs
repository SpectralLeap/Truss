namespace Truss.Modeling.Application.Tests.Unit.Commands.TestApplication;

public sealed record AddValueResult(int I)
{
    public int I { get; } = I;
}