using Truss.Application.Cqrs.Commands;

namespace Truss.Application.Tests.Unit.Commands.TestApplication;

public sealed record AddValueCommand(int Value) : Command<AddValueResult>;

public sealed record SubtractValueCommand(int Value) : Command;