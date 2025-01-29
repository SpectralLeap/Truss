using FluentValidation;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace ExampleApplication.Module1;

public sealed record DoThingCommand
    : ICommand<DoThingCommandResult>
{
    public required string Thing { get; init; }
}

public sealed class DoThingCommandResult
{
    public required string Thing { get; init; }
}

public sealed class DoThingValidator : AbstractValidator<DoThingCommand>
{
    public DoThingValidator()
    {
        RuleFor(x => x.Thing)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("Thing must be a guid obviously");
    }
}


// This is registered with a mediator
// ReSharper disable once UnusedType.Global
internal sealed class DoThingCommandHandler
    : ICommandHandler<DoThingCommand, DoThingCommandResult>
{

    public async Task<Result<DoThingCommandResult>> Handle(
        DoThingCommand command,
        CancellationToken ct
    )
    {
        return Result.Fail($"Not implemented");
    }
}