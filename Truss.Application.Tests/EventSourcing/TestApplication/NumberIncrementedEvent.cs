using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Tests.EventSourcing.TestApplication;

internal sealed record NumberIncrementedEvent : ChangeEvent
{
    public NumberIncrementedEvent(CounterId aggregateId) : base(aggregateId.Value)
    {
    }
}