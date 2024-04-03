using Microsoft.Extensions.DependencyInjection;
using Truss.Application.Abstractions.EventSourcing.Reading;
using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Application.Tests.EventSourcing.TestApplication;
using Truss.Tests.Infrastructure.InMemory;

namespace Truss.Application.Tests.EventSourcing;

public sealed class EventSourcingTests
{
    [Fact]
    public async Task writes_to_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();

        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);

        var val = await EventStore.Read(counter.Id);
        Assert.Equal(3, await val.SuccessValue.CountAsync());
    }

    [Fact]
    public async Task reads_from_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
    
        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);

        var events = await AggregateEventStreamReader.ReadEventStream(counter.Id);

        Assert.Equal(3, await events.SuccessValue.CountAsync());
    }

    [Fact]
    public async Task events_already_stored_are_not_restored()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
        
        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);
    
        var events = await AggregateEventStreamReader.ReadEventStream(counter.Id);

        var counterAgain = Counter.FromHistory(await events.SuccessValue.ToListAsync());
        counterAgain.IncrementNumber();
        counterAgain.IncrementNumber();

        await writer.WriteToStream(counterAgain);
        Assert.Equal(5, await (await AggregateEventStreamReader.ReadEventStream(counter.Id)).SuccessValue.CountAsync());
    }

    [Fact]
    public async Task events_are_ordered_by_sequence()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
            
        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);
        
        var events = await AggregateEventStreamReader.ReadEventStream(counter.Id);
    
        var counterAgain = Counter.FromHistory(await events.SuccessValue.ToListAsync());
        counterAgain.IncrementNumber();
        counterAgain.IncrementNumber();
    
        await writer.WriteToStream(counterAgain);
        var eventsAgain =
            await (await AggregateEventStreamReader.ReadEventStream(counter.Id)).SuccessValue.ToListAsync();

        Assert.Equal(eventsAgain.Count(), eventsAgain.DistinctBy(e => e.SequenceNumber).Count());
    }

    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
            .AddEventSourcing()
            .AddInMemoryInfrastructure()
            .BuildServiceProvider()
        ;

    private IEventStore EventStore => _serviceProvider.GetService<IEventStore>()!;
    private IAggregateEventStreamWriter AggregateEventStreamWriter => _serviceProvider.GetService<IAggregateEventStreamWriter>()!;
    private IAggregateEventStreamReader AggregateEventStreamReader => _serviceProvider.GetService<IAggregateEventStreamReader>()!;
}

