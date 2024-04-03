using MediatR;

namespace Truss.Application.Abstractions.Domain;

/// <summary>
/// Marker interface to represent a domain event
/// </summary>
public interface IDomainEvent : INotification;