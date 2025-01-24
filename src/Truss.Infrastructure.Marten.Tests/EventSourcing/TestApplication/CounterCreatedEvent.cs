using Truss.Modeling.Domain.Events;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;

public sealed record CounterCreatedEvent : IDomainEvent
{
    public required Guid Id { get; init; }
};