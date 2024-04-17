using MediatR;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.MediatR.Wrappers;

internal sealed class QueryWrapper<TQuery, TResult>(TQuery query) : IRequest<Result<TResult>>
    where TQuery : Query<TResult>
{
    public TQuery Query { get; } = query;
}