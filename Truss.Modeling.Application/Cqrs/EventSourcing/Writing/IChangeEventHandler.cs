using MediatR;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// For receiving and acting on change events
/// </summary>
/// <typeparam name="TChangeEvent"></typeparam>
public interface IChangeEventHandler<in TChangeEvent> 
    : INotificationHandler<TChangeEvent>
    where TChangeEvent : ChangeEvent;