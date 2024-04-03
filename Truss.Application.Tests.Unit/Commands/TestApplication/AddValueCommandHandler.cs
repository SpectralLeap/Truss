using Truss.Application.Abstractions.Commands;
using Truss.Results;

namespace Truss.Application.Tests.Unit.Commands.TestApplication;

public class AddValueCommandHandler : ICommandHandler<AddValueCommand, AddValueResult>
{
    public async Task<Result<AddValueResult>> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Result.Success(new AddValueResult(request.Value + 1));
        }

        return Result.Fail("No good");
    }
}

public class SubtractValueCommandHandler : ICommandHandler<SubtractValueCommand>
{
    public Task<Result<None>> Handle(SubtractValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(Result.Success());
        }
        
        return Task.FromResult(Result.Fail("value was odd"));
    }
}