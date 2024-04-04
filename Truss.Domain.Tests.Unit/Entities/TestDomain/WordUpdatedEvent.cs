using Truss.Domain.Events;

namespace Truss.Domain.Tests.Unit.Entities.TestDomain;

public sealed record WordUpdatedEvent(string Word) : DomainEvent;