using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRQueryWrapper<TQuery, TResult>(TQuery query) : IRequest<Result<TResult>>
    where TQuery : Query<TResult>
{
    public TQuery Query { get; } = query;
}