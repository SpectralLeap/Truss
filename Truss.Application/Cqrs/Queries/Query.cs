using MediatR;
using Truss.Results;

namespace Truss.Application.Abstractions.Queries;

/// <summary>
/// A type for reading information
/// </summary>
/// <typeparam name="TQueryResult"></typeparam>
public record Query<TQueryResult>
    : IRequest<Result<TQueryResult>>;
    
    