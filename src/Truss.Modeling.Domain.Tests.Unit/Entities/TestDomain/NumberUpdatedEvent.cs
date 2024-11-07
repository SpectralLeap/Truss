using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public sealed record NumberUpdatedEvent(int Number) : IDomainEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int Number { get; } = Number;
}