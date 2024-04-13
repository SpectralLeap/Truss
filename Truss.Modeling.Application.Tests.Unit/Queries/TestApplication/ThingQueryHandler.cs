using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Tests.Unit.Queries.TestApplication;

public class ThingQueryHandler : IQueryHandler<ThingQuery, ThingQueryResult>
{
    private readonly ThingStore _store;

    public ThingQueryHandler(ThingStore store)
    {
        _store = store;
    }
    
    public Task<Result<ThingQueryResult>> Handle(ThingQuery query, CancellationToken cancellationToken)
    {
        return Result.Success(new ThingQueryResult(_store.GetThing(query.ThingToGet)));
    }
}