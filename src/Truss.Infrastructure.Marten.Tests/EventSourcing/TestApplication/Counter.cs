using Truss.Modeling.Domain.Entities;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;

public sealed class Counter : AggregateRoot<Guid>
{
    public int Number { get; private set; }

    private Counter() : base(Guid.NewGuid()) { }

    public void IncrementNumber()
    {
        ApplyAndAddPendingEvent(new NumberIncrementedEvent());
    }
    
    private void Apply(NumberIncrementedEvent @event)
    {
        Number++;
    }

    public static Counter New()
    {
        return new Counter();
    }
}