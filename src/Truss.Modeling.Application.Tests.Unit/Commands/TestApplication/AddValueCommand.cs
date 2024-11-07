using Truss.Modeling.Application.Cqrs.Commands;

namespace Truss.Modeling.Application.Tests.Unit.Commands.TestApplication;

public sealed record AddValueCommand(int Value) : ICommand<AddValueResult>
{
    public int Value { get; } = Value;
}