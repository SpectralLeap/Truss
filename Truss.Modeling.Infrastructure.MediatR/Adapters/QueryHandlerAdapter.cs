using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.MediatR.Adapters;

internal sealed class QueryHandlerAdapter<TQuery, TResult> 
    : IRequestHandler<QueryWrapper<TQuery, TResult>, Result<TResult>>
    where TQuery : Query<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _internalHandler;

    public QueryHandlerAdapter(IQueryHandler<TQuery, TResult> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task<Result<TResult>> Handle(QueryWrapper<TQuery, TResult> queryWrapper, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(queryWrapper.Query, cancellationToken);
    }
}