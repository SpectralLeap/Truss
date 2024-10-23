using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace ExampleApplication.WebApi.Services;

public sealed record GreetCommand
    : ICommand<GreetResult>
{
    public required string Subject { get; init; }
}

public sealed class GreetResult
{
    public required string Greeting { get; init; }
}

// This is registered with a mediator
// ReSharper disable once UnusedType.Global
internal sealed class GreetHandler
    : ICommandHandler<GreetCommand, GreetResult>
{

    public async Task<Result<GreetResult>> Handle(
        GreetCommand command,
        CancellationToken ct
    )
    {
        return Result.Success(new GreetResult
        {
            Greeting = $"Hello {command.Subject}",
        });
    }
}