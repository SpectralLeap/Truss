using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRWrappedQueryHandlerAdapter<TQuery, TResult> 
    : IRequestHandler<MediatRQueryWrapper<TQuery, TResult>, Result<TResult>>
    where TQuery : Query<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _internalHandler;

    public MediatRWrappedQueryHandlerAdapter(IQueryHandler<TQuery, TResult> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task<Result<TResult>> Handle(MediatRQueryWrapper<TQuery, TResult> queryWrapper, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(queryWrapper.Query, cancellationToken);
    }
}