using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace ExampleApplication.WebApi.Services;

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
            Greeting = $"Hello {command.Subject}"
        });
    }
}