using MediatR;
using Truss.Results;

// ReSharper disable UnusedMember.Global
#pragma warning disable CS0108, CS0114

namespace Truss.Application.Cqrs.Queries;

/// <summary>
/// Handles a query of the specified query type
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IQueryHandler<in TQuery, TResult>
    : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : Query<TResult>;