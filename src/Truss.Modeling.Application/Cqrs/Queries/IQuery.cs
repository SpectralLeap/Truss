using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Queries;

public interface IQuery<T> 
    : IRequest<Result<T>>;