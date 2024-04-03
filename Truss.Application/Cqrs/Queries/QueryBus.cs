using MediatR;
using Truss.Application.Abstractions.Queries;
using Truss.Results;

namespace Truss.Application.Cqrs.Queries;

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
        return await _mediator.Send(query).ConfigureAwait(false);
    }
}