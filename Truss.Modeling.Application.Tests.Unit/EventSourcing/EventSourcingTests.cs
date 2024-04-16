using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.MediatR;
using Truss.Modeling.Application.Tests.Unit.EventSourcing.TestApplication;
using Truss.Modeling.Domain.Events;
using Truss.Tests.Infrastructure.InMemory;

namespace Truss.Modeling.Application.Tests.Unit.EventSourcing;

public sealed class EventSourcingTests
{
    [Fact]
    public async Task writes_to_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();

        await DomainEventDispatcher.DispatchAndClearDomainEvents(counter, new CancellationToken());

        var val = eventReadStore.Read(counter.Id);
        Assert.Equal(3, await val.CountAsync());
    }

    [Fact]
    public async Task reads_from_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();

        await DomainEventDispatcher.DispatchAndClearDomainEvents(counter, new CancellationToken());
        
       var events = AggregateEventStreamReader.ReadEventStream(counter.Id);

        Assert.Equal(3, await events.SuccessValue.CountAsync());
    }

    [Fact]
    public async Task events_already_stored_are_not_restored()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
        
        await DomainEventDispatcher.DispatchAndClearDomainEvents(counter, new CancellationToken());
   
        var events = AggregateEventStreamReader.ReadEventStream(counter.Id).SuccessValue;

        var counterAgain = Counter.FromHistory(await events.ToListAsync());
        counterAgain.IncrementNumber();
        counterAgain.IncrementNumber();

        await DomainEventDispatcher.DispatchAndClearDomainEvents(counterAgain, new CancellationToken());
        
        Assert.Equal(5, await (AggregateEventStreamReader.ReadEventStream(counter.Id)).SuccessValue.CountAsync());
    }

    [Fact]
    public async Task events_are_ordered_by_sequence()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
        
        await DomainEventDispatcher.DispatchAndClearDomainEvents(counter, new CancellationToken());
        
        var events = AggregateEventStreamReader.ReadEventStream(counter.Id);
    
        var counterAgain = Counter.FromHistory(await events.SuccessValue.ToListAsync());
        counterAgain.IncrementNumber();
        counterAgain.IncrementNumber();
    
        var eventsAgain =
            await (AggregateEventStreamReader.ReadEventStream(counter.Id)).SuccessValue.ToListAsync();

        Assert.Equal(eventsAgain.Count(), eventsAgain.Select(e => e.EventSequenceNumber!.Value).Distinct().Count());
    }

    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
            .AddTrussWithMediatR([typeof(Counter).Assembly])
            .AddInMemoryInfrastructure()
            .BuildServiceProvider()
        ;

    private IEventReadStore eventReadStore => _serviceProvider.GetService<IEventReadStore>()!;
    private IAggregateEventStreamWriter AggregateEventStreamWriter => _serviceProvider.GetService<IAggregateEventStreamWriter>()!;
    private IAggregateEventStreamReader AggregateEventStreamReader => _serviceProvider.GetService<IAggregateEventStreamReader>()!;
    private IDomainEventDispatcher DomainEventDispatcher => _serviceProvider.GetService<IDomainEventDispatcher>()!;
}

