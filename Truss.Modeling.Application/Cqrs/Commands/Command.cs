using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Commands;

/// <summary>
/// Command for CQRS that returns a <see cref="Result"/>
/// </summary>
public abstract record Command 
    : IRequest<Result<Nil>>;

/// <summary>
/// Command for CQRS that returns a specific result type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract record Command<TResult> 
    : IRequest<Result<TResult>>;