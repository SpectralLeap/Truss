using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record OrchardCreatedEvent(OrchardId? aggregateId, string? Name) : CreationEvent<OrchardId>(aggregateId);