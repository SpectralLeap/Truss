using Truss.Modeling.Application.Cqrs.Commands;

namespace Truss.Modeling.Application.Tests.Unit.Commands.TestApplication;

public sealed record SubtractValueCommand(int Value) : ICommand;