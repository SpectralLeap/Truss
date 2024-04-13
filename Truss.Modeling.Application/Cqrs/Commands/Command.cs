using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Commands;

public interface ICommand<T>;

/// <summary>
/// Command for CQRS that returns a <see cref="Result"/>
/// </summary>
public abstract record Command 
    : ICommand<Result<Nil>>;

/// <summary>
/// Command for CQRS that returns a specific result type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract record Command<TResult> 
    : ICommand<Result<TResult>>;