using Truss.Application.Cqrs.Queries;
using Truss.Results;

namespace Truss.Application.Tests.Unit.Queries.TestApplication;

public class ThingQueryHandler : IQueryHandler<ThingQuery, ThingQueryResult>
{
    private readonly ThingStore _store;

    public ThingQueryHandler(ThingStore store)
    {
        _store = store;
    }
    
    public Task<Result<ThingQueryResult>> Handle(ThingQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(new ThingQueryResult(_store.GetThing(request.ThingToGet)));
    }
}