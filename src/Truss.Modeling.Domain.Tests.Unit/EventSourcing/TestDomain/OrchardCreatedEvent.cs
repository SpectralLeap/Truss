using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record OrchardCreatedEvent(OrchardId? aggregateId, string? Name) : CreationEvent<OrchardId>(aggregateId)
{
    public OrchardId? aggregateId { get; } = aggregateId;
    public string? Name { get; } = Name;
}