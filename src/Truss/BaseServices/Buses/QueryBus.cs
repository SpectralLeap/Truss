using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.BaseServices.Buses;

internal sealed class QueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<Result<TResult>> SendQuery<TQuery, TResult>(
        TQuery query,
        CancellationToken cancellationToken
    ) where TQuery : IQuery<TResult>
    {
        return await _mediator.Send(query, cancellationToken);
    }
}