using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.MediatR.Buses;

internal sealed class QueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<TResult>> SendQuery<TQuery, TResult>(TQuery query) where TQuery 
        : Query<TResult>
    {
        var wrappedQuery = new QueryWrapper<TQuery, TResult>(query);
        
        var result = await _mediator.Send(wrappedQuery).ConfigureAwait(false);

        return result;
    }
}