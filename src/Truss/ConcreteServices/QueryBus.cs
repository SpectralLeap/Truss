using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.ConcreteServices;

internal sealed class QueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<Result<TResult>> SendQuery<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(query, cancellationToken);
    }
}