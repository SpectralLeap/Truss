using Truss.Modeling.Domain.Events;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;

public sealed record NumberIncrementedEvent : IDomainEvent;