using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Domain.Tests.EventSourcing.TestDomain;
using Truss.Results;
using Truss.Tests.XunitHelpers;
using Xunit.Abstractions;

namespace Truss.Domain.Tests.EventSourcing;

public sealed class EventSourcingAggregateRootTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private OrchardId? _orchardId;
    private string? _orchardName;
    private Orchard? _orchard;

    [Fact]
    void
        an_aggregates_id_can_be_rehydrated_using_its_creation_event
        ()
    {
        var e = GetCreateOrchardEvent();
        var orchard = Orchard.FromHistory(new[] {e});
        Assert.Equal(_orchardId, orchard.Id);
    }

    [Fact]
    void
        changes_on_aggregate_creation_are_rehydrated_using_its_creation_event
        ()
    {
         var e = GetCreateOrchardEvent();
         var orchard = Orchard.FromHistory(new[] {e});
         Assert.Equal(_orchardName, orchard.Name);       
    }
    
    [Fact]
    void
        creation_events_not_from_internal_cannot_be_added
        ()
    {
        var id = new OrchardId(Guid.NewGuid());
        var creation = new OrchardCreatedEvent(id, "smith's orchard");
        
        Assert.Throws<InvalidOperationException>(() =>  Orchard.FromHistory(new ChangeEvent[]{creation}));
    }
    
    [Fact]
    void
        in_order_subsequent_change_events_are_applied_in_order
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent;
    
        var orchard = Orchard.FromHistory(new ChangeEvent[] {creation, tree1!, tree2!});
            
        Assert.Equal(tree1!.TreeType, orchard.Trees[0].TreeType);
        Assert.Equal(tree2!.TreeType, orchard.Trees[1].TreeType);
    }
     
    [Fact]
    void
        out_of_order_subsequent_change_events_are_applied_in_order
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent;

        var orchard = Orchard.FromHistory(new ChangeEvent[] {creation, tree2!, tree1!});
        
        Assert.Equal(tree1!.TreeType, orchard.Trees[0].TreeType);
        Assert.Equal(tree2!.TreeType, orchard.Trees[1].TreeType);
    }
    
    [Fact]
    void
        if_events_are_missing_then_throws_exception
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        _ = GetTreeAddedEvent() as TreeAddedEvent;
        var tree3 = GetTreeAddedEvent() as TreeAddedEvent;

        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {creation, tree1!, tree3!}));
    }

    [Fact]
    void
        if_no_creation_event_then_fails
        ()
    {
        GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent; // left out of history
        var tree3 = GetTreeAddedEvent() as TreeAddedEvent;
    
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {tree1!, tree2!, tree3!}));
    }
    
    [Fact]
    void
        if_fake_creation_event_then_fails
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = new TreeAddedEvent(new OrchardId(creation.AggregateId), new TreeId(Guid.NewGuid()), "apple");
        
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {tree1}));
    }

    [Fact]
    void
        more_than_one_creation_event_cannot_be_created
        ()
    {
        GetCreateOrchardEvent();
        _ = GetTreeAddedEvent() as TreeAddedEvent;
        Assert.Null(GetBadCreateOrchardEvent());
    }

    [Fact]
    void
        creation_events_have_aggregate_id
        ()
    {
        var creation = GetCreateOrchardEvent();
        
        Assert.Equal(_orchardId!.Value, creation.AggregateId);
    }

    [Fact]
    void
        change_events_have_aggregate_id
        ()
    {
        GetCreateOrchardEvent();
        var tree = GetTreeAddedEvent();
            
        Assert.Equal(_orchardId!.Value, tree.AggregateId);
    }

    [Fact]
    void
        rehydrating_an_aggregate_does_not_keep_events
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree = GetTreeAddedEvent();

        var orchard2 = Orchard.FromHistory(new[] {creation, tree});
        Assert.Equal(0, orchard2.PendingChangeEvents.Count);
    }

    private sealed record FakeEvent(Guid AggregateId) : ChangeEvent(AggregateId);
    
    [Fact]
    void
        applying_a_non_existing_event_type_throws
        ()
    {
        var creation = GetCreateOrchardEvent();
        var fakeEvent = (ChangeEvent)new FakeEvent(Guid.NewGuid());
        fakeEvent.SetSequence(new EventSequenceNumber(2));

        Assert.Throws<KeyNotFoundException>(() => Orchard.FromHistory(new[] { creation, fakeEvent }));
    }

    [Fact]
    void
        rehydrating_an_aggregate_without_events_is_handled
        ()
    {
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent []{}));
    }

    [Fact]
    void
        can_do_binding
        ()
    {
        BindCreation().AssertSuccessful();
    }

    [Fact]
    void
        an_exception_in_binding_is_handled
        ()
    {
        ((Result<Orchard>)BindCreation().Then(o => o.ThrowException())).AssertFailure();
    }
    
    [Fact]
    void
        an_exception_in_binding_returns_the_message
        ()
    {
        Assert.NotEmpty(((Result<Orchard>)BindCreation().Then(o => o.ThrowException())).ExpectFailureAndGet().FailureReasons);
    }

    [Fact]
    void
        exceptions_can_be_thrown
        ()
    {
        Assert.ThrowsAsync<InvalidCastException>(
            () => throw ((Result<Orchard>)BindCreation().Then(o => o.ThrowException())).ExpectFailureAndGet().Exception!);
    }

    [Fact]
    void
        can_do_mapping
        ()
    {
        var orchard = BindCreation();
        var result = (Result<string>)BindCreation().Then(o => Result.Success(o.Name!));
        
        Assert.Equal(orchard.ExpectSuccessAndGet().Name, result.ExpectSuccessAndGet());
    }
    
    [Fact]
    void
        exception_in_mapping_returns_exception_message
        ()
    {
        Assert.Equal("Ruh roh", 
        ((Result<string>)BindCreation().Then(o => 
                {
                    throw new Exception("Ruh roh");
                    return Result.Success(o.Name);
                }))
                .ExpectFailureAndGet().FailureReasons.First()); 
    }

    [Fact]
    void
        exception_in_mapping_is_not_successful
        ()
    {
        var result = (Result<None>)BindCreation()
            .Then(_ =>
            {
                throw new Exception();
                return Result.Success();
            });
        
        result.AssertFailure();
    }

    [Fact]
    void
        invalid_binding_works_without_exceptions
        ()
    {
        InvalidBindCreation().AssertFailure();
    }

    [Fact]
    void
        invalid_binding_reports_invalidations
        ()
    {
        Assert.NotEmpty(InvalidBindCreation().ExpectFailureAndGet().FailureReasons);
    }

    public EventSourcingAggregateRootTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

   
    private CreationEvent<OrchardId> GetCreateOrchardEvent()
    {
        _orchardName = $"smith's orchard";
        return GetCreateOrchardEvent(_orchardName);
    }

    private CreationEvent<OrchardId> GetBadCreateOrchardEvent()
    {
        var result = _orchard!.DoIncorrectCreation("Bad orchard");
        
        Assert.False(result.Succeeded);
        
        var e = _orchard.PendingChangeEvents.Last() as CreationEvent<OrchardId>;

        return e!;
    }
    
    private CreationEvent<OrchardId> GetCreateOrchardEvent(string name)
    {
        _orchardId = new OrchardId(Guid.NewGuid());
        _testOutputHelper.WriteLine($"{_orchardId} created");
        _orchard = new Orchard(_orchardId, name);
        _testOutputHelper.WriteLine("Orchard created");
        _testOutputHelper.WriteLine($"{name} added");

        var e = _orchard.PendingChangeEvents.Last() as CreationEvent<OrchardId>;
        if (e is null)
        {
            Assert.Fail("Creation event failed");
        }
        
        return e;
    }
 
    private ChangeEvent GetTreeAddedEvent()
    {
        return GetTreeAddedEvent(_orchard);
    }

    private ChangeEvent GetTreeAddedEvent(Orchard? orchard)
    {
        orchard!
            .AddTree(Guid.NewGuid().ToString())
            ;

        var e = orchard.PendingChangeEvents.Last() as TreeAddedEvent;
        if (e is null)
        {
            Assert.Fail("Adding tree event failed");
        }
        
        return e;
    }

    private Result<Orchard> BindCreation()
    {
        return Orchard.Create()
                .AddTree("maple")
                .Then(o => o.AddTree("orange"))
                .Then(o => o.AddTree("apple"))
            ;
    }

    private Result<Orchard> InvalidBindCreation()
    {
        return Orchard.Create()
            .AddTree("maple")
            .Then(o => o.AddTree("orange"))
            .Then(o => o.AddTree("invalid"));
    }
}