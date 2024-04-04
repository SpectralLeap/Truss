using MediatR;

namespace Truss.Domain.Events;

/// <summary>
/// Marker interface to represent a domain event
/// </summary>
public interface IDomainEvent : INotification;