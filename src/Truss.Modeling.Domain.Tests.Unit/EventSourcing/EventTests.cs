using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.Tests.Unit.EventSourcing.TestDomain;

namespace Truss.Modeling.Domain.Tests.Unit.EventSourcing;

public sealed class EventTests
{
    [Fact]
    public void have_a_version_number()
    {
        Assert.True(DomainEvent.DomainEventVersion > 0);
    }
    
    [Fact]
    public void have_a_unique_id()
    {
        var e = new OrchardCreatedEvent(new OrchardId(Guid.NewGuid()), "x");
            
        Assert.NotEqual(Guid.Empty, e.Id);
    }
    
    [Fact]
    public void have_an_occurred_time()
    {
        var e = new OrchardCreatedEvent(new OrchardId(Guid.NewGuid()), "x");
                
        Assert.InRange(DateTime.UtcNow - e.Time, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
    }

    private sealed class NonEventType {}
    
    [Fact]
    public void event_classes_must_have_name_ending_in_event()
    {
        Assert.Throws<ArgumentException>(() => (EventType)typeof(NonEventType));
    }
    
    private sealed class NonEventTypeWithASuperLongNameThatIsWayTooLongToBeUsedEvent {}
     
    [Fact]
    public void event_classes_cannot_have_names_over_50_chars()
    {
        Assert.Throws<ArgumentException>(() => (EventType)typeof(NonEventTypeWithASuperLongNameThatIsWayTooLongToBeUsedEvent));
    }
    
    [Fact]
    public void event_names_cannot_be_empty()
    {
        Assert.Throws<ArgumentException>(() => (EventType)"");
    }
    
    [Fact]
    public void event_names_cannot_contain_white_space()
    {
        Assert.Throws<ArgumentException>(() => (EventType)"an event");
    }
    
    [Fact]
    public void event_names_cannot_contain_non_word_characters()
    {
        var badChars = new[] { "*", "/", "%", "@", "|" };
        foreach (var badChar in badChars)
        {
            Assert.Throws<ArgumentException>(() => (EventType)badChar);
        }
    }
}