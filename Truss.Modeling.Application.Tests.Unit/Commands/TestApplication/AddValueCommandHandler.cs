using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Tests.Unit.Commands.TestApplication;

public class AddValueCommandHandler : ICommandHandler<AddValueCommand, AddValueResult>
{
    public async Task<Result<AddValueResult>> Handle(AddValueCommand command, CancellationToken cancellationToken)
    {
        if (command.Value % 2 == 0)
        {
            return Result.Success(new AddValueResult(command.Value + 1));
        }

        return Result.Fail("No good");
    }
}

public class SubtractValueCommandHandler : ICommandHandler<SubtractValueCommand>
{
    public Task<Result<Nil>> Handle(SubtractValueCommand command, CancellationToken cancellationToken)
    {
        if (command.Value % 2 == 0)
        {
            return Task.FromResult(Result.Success());
        }
        
        return Task.FromResult(Result.Fail("value was odd"));
    }
}