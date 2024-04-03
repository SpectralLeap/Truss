using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Domain.EventSourcing;
using Truss.Results;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

public sealed class EventHandlerAttribute : Attribute;

public sealed class Counter : EventSourcedAggregateRoot<Counter, CounterId>
{
    private int Number { get; set; }

    private Counter(CounterId id) : base(id)
    {

    }

    protected override void Configure(IEventSourcingConfigurationBuilder<Counter> builder)
    {
        builder.Handlers
            .AddHandler<CounterCreatedEvent>(CounterCreatedEventHandler)
            .AddHandler<NumberIncrementedEvent>(NumberUpdatedChangeHandler);
    }
    
    [EventHandler]
    private Result<Counter> CounterCreatedEventHandler(CounterCreatedEvent arg)
    {
        return Success();
    }

    public Counter() : this(new CounterId(Guid.NewGuid()))
    {
        RegisterChangeEvent(new CounterCreatedEvent(Id));
    }

    public Result<Counter> IncrementNumber()
    {
        return RegisterChangeEvent(new NumberIncrementedEvent(Id));
    }
    
    private Result<Counter> NumberUpdatedChangeHandler(NumberIncrementedEvent arg)
    {
        Number++;
 
        return Success();
    }

    public static Counter FromHistory(IEnumerable<ChangeEvent> events)
    {
        return Rehydrate(id => new Counter(new CounterId(id)), events).SuccessValue;
    }

}