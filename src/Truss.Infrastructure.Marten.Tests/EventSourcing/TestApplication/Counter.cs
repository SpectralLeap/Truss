using Truss.Modeling.Domain.Entities;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;

public sealed class Counter : Aggregate<Guid>
{
    public int Number { get; private set; }

    private Counter() { }

    public void IncrementNumber()
    {
        var @event = new NumberIncrementedEvent();

        ApplyAndAddPendingEvent(@event);
    }
    
    private void Apply(NumberIncrementedEvent @event)
    {
        Number++;
    }

    public static Counter New()
    {
        return new Counter
        {
            Id = Guid.NewGuid()
        };
    }
}