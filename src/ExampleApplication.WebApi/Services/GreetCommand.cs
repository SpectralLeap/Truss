using Truss.Modeling.Application.Cqrs.Commands;

namespace ExampleApplication.WebApi.Services;

public sealed record GreetCommand
    : ICommand<GreetResult>
{
    public string Subject { get; init; }
}

// This is registered with a mediator
// ReSharper disable once UnusedType.Global