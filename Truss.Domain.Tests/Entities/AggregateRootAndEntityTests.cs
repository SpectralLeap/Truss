using Microsoft.Extensions.DependencyInjection;
using Truss.Application.Abstractions.Events;
using Truss.Application;
using Truss.Domain.Tests.Entities.TestDomain;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Truss.Domain.Tests.Entities;

public sealed class AggregateRootAndEntityTests
{
   
    [Fact]
    public void SameGuidAreEqual()
    {
        var guid = Guid.NewGuid();
        var a = new WordAggregate(new WordAggregateId(guid));
        var b = new WordAggregate(new WordAggregateId(guid));

        Assert.Equal(a, b);
    }
    
    [Fact]
    public void DifferentGuidAreNotEqual()
    {
        var a = new WordAggregate(new WordAggregateId(Guid.NewGuid()));
        var b = new WordAggregate(new WordAggregateId(Guid.NewGuid()));
    
        Assert.NotEqual(a, b);
    }
    
    [Fact]
    public void SameGuidAndSameTypeHaveSameHashCode()
    {
        var guid = Guid.NewGuid();
        var a = new WordAggregate(new WordAggregateId(guid)).GetHashCode();
        var b = new WordAggregate(new WordAggregateId(guid)).GetHashCode();
        
        Assert.Equal(a, b);
    }
     
    [Fact]
    public void SameGuidAndDifferentTypeHaveDifferentHashCode()
    {
        var guid = Guid.NewGuid();
        var a = new WordAggregate(new WordAggregateId(guid)).GetHashCode();
        var b = new NotWordAggregate(new NotWordAggregateId(guid)).GetHashCode();
            
        Assert.NotEqual(a, b);
    }
     
    [Fact]
    public void DifferentGuidAndSameTypeHaveDifferentHashCode()
    {
        var a = new WordAggregate(new WordAggregateId(Guid.NewGuid())).GetHashCode();
        var b = new WordAggregate(new WordAggregateId(Guid.NewGuid())).GetHashCode();
            
        Assert.NotEqual(a, b);
    }
     
    [Fact]
    public void EntitiesCannotBeInstantiatedWithoutAnId()
    {
        Assert.Throws<ArgumentNullException>(() => new WordAggregate(null).GetHashCode());
    }
    
    [Fact]
    public async Task DomainEventsCanBeHandled()
    {
        var aggregate = new WordAggregate(WordAggregateId.New());
        
        aggregate.UpdateWord("something");
        
        await _dispatcher.DispatchAndClearDomainEvents(aggregate, new CancellationToken());

        Assert.Equal("something", _services.GetService<WordListener>()!.Word);
    }
   
    [Fact]
    public async Task DomainEventsOnEntityCanBeHandledThroughAggregate()
    {
        var aggregate = new WordAggregate(WordAggregateId.New());

        aggregate.AnnounceNumber(1);

        await _dispatcher.DispatchAndClearDomainEvents(aggregate, new CancellationToken());
    
        Assert.Equal(1, _services.GetService<NumberListener>()!.Number);
    }
    
    [Fact]
    public async Task DomainEventsAreOrdered()
    {
        var aggregate = new WordAggregate(WordAggregateId.New());

        aggregate.AnnounceNumber(1);
        aggregate.AnnounceNumber(2);
        aggregate.AnnounceNumber(3);
        
        await _dispatcher.DispatchAndClearDomainEvents(aggregate, new CancellationToken());

        var numbers = _numberListener!.Numbers;

        Assert.Equal(3, numbers.Pop());        
        Assert.Equal(2, numbers.Pop());        
        Assert.Equal(1, numbers.Pop());        
    }

    readonly IServiceProvider _services;

    readonly IDomainEventDispatcher _dispatcher;
    private readonly NumberListener? _numberListener;

    public AggregateRootAndEntityTests()
    {
        _services = new ServiceCollection()
                .AddMediatR(c=> 
                    c.RegisterServicesFromAssembly(typeof(WordUpdatedEventHandler).Assembly))
                .AddDomainEvents()
                .AddScoped<WordListener>()
                .AddScoped<NumberListener>()
                .BuildServiceProvider()
            ;
        
        _dispatcher = _services.GetService<IDomainEventDispatcher>()!;

        _numberListener = _services.GetService<NumberListener>();
    }
     
}