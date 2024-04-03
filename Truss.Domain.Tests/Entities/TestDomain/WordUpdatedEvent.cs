using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.Entities.TestDomain;

public sealed record WordUpdatedEvent(string Word) : DomainEvent;