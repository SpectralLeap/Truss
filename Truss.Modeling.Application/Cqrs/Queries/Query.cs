namespace Truss.Modeling.Application.Cqrs.Queries;


/// <summary>
/// A type for reading information
/// </summary>
/// <typeparam name="TQueryResult"></typeparam>
public record Query<TQueryResult> 
    : IQuery<TQueryResult>;