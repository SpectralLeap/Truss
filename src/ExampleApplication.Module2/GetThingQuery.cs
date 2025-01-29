using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace ExampleApplication.Module2;

public sealed record GetThingQuery
    : IQuery<GetThingQueryResult>
{
    public required Guid ThingId { get; init; }
}

public sealed class GetThingQueryResult
{
    public required string Thing { get; init; }
}

// This is registered with a mediator
// ReSharper disable once UnusedType.Global
internal sealed class GetThingQueryHandler
    : IQueryHandler<GetThingQuery, GetThingQueryResult>
{

    public async Task<Result<GetThingQueryResult>> Handle(
        GetThingQuery query,
        CancellationToken ct
    )
    {
        return Result.Fail($"Not implemented");
    }
}