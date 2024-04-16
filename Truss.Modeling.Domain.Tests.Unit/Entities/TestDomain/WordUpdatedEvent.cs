using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public sealed record WordUpdatedEvent(string Word) : DomainEvent;

