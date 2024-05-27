// ReSharper disable UnusedMember.Global

using MediatR;
using Truss.Monads.Results;

#pragma warning disable CS0108, CS0114

namespace Truss.Modeling.Application.Cqrs.Queries;

/// <summary>
/// Handles a query of the specified query type
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IQueryHandler<in TQuery, TResult>
    : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>
{
    
    public Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}