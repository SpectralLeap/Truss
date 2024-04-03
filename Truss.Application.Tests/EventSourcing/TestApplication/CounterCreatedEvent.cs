using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Tests.EventSourcing.TestApplication;

internal sealed record CounterCreatedEvent(CounterId aggregateId) : CreationEvent<CounterId>(aggregateId);