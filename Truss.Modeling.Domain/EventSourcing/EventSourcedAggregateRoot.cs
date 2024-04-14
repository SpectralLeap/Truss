using Truss.Modeling.Domain.Entities;
using Truss.Monads.Results;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Contains event sourcing logic for implementing event sourced aggregates
/// </summary>
/// <typeparam name="TRoot">
/// The type of <see cref="AggregateRoot{TId,TIdType}"/> encapsulated in <see cref="Result"/>.
/// The intent is for this to be self-referential for method chaining. See Curiously Recurring Template Pattern.
/// </typeparam>
/// <typeparam name="TId"><see cref="AggregateRootId{TId}"/> of the aggregate type type. Must be a type of <see cref="Guid"/></typeparam>
public abstract class EventSourcedAggregateRoot<TRoot, TId> : AggregateRoot<TId, Guid>, IEventSourcedAggregateRoot<TId>
    where TId : AggregateRootId<Guid>
    where TRoot : EventSourcedAggregateRoot<TRoot, TId>
{
    
    /// <summary>
    /// Events that have not yet been stored
    /// </summary>
    public IReadOnlyCollection<ChangeEvent> PendingChangeEvents => _pendingEventSourcingEvents;
    private readonly List<ChangeEvent> _pendingEventSourcingEvents = new();
    
    /// <summary>
    /// It is not reliable to depend on dates or times in event sourcing.
    /// The sequence number is used to enforce ordering.
    /// Using optimistic concurrency.
    /// </summary>
    private EventSequenceNumber _currentEventSequenceNumber = new(0);

    /// <summary>
    /// A registry for derived class's change events
    /// </summary>
    private readonly ChangeEventHandlerRegistry<TRoot, TId> _handlers;

    /// <summary>
    /// The configuration builder given to derived types to configure
    /// </summary>
    private readonly IEventSourcingConfigurationBuilder<TRoot> _configurationBuilder;
    
    /// <summary>
    /// Notifies if the configuration has occurred
    /// </summary>
    private bool _configured = false;

    /// <summary>
    /// Initialize an aggregate with the Id
    /// </summary>
    protected EventSourcedAggregateRoot(TId id) : base(id)
    {
        _handlers = new ChangeEventHandlerRegistry<TRoot, TId>();
        
        _configurationBuilder = new EventSourcingConfigurationBuilder<TRoot>(
            _handlers
        );
    }

    /// <summary>
    /// Configure event sourcing for the aggregate
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void Configure(IEventSourcingConfigurationBuilder<TRoot> builder);
    
    /// <summary>
    /// Register a change event with the aggregate
    /// </summary>
    /// <param name="event">The <see cref="ChangeEvent"/> that should be registered</param>
    /// <typeparam name="TChangeEvent">The <see cref="Type"/> of the <see cref="ChangeEvent"/></typeparam>
    protected Result<TRoot> Apply<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        if (_currentEventSequenceNumber.Value > 1 
            && typeof(CreationEvent<TId>).IsAssignableFrom(typeof(TChangeEvent)))
        {
            return Result.Fail("An aggregate cannot be created more than once.");
        } 
        
        return Restore(@event).Then(root =>
        {
            _currentEventSequenceNumber += 1;

            @event.SetSequence(_currentEventSequenceNumber);

            RegisterDomainEvent(@event);
            _pendingEventSourcingEvents.Add(@event);
            
            return Result.Success(root);
        });
    }
           
    /// <summary>
    /// Applies the registered change event handler
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="TChangeEvent"></typeparam>
    private Result<TRoot> Restore<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        if (!_configured)
        {
            Configure(_configurationBuilder);
            
            _configured = true;
        }

        return _handlers.Handle(@event);
    }
    
    /// <summary>
    /// Rehydrates an aggregate from a sequence of events.
    /// This returns an aggregate to the state based
    /// on changes.   
    /// </summary>
    /// <param name="typeGenerator">A function to create an instance of the type T</param>
    /// <param name="eventHistory">The events to be applied</param>
    /// <typeparam name="T">This should correspond to the aggregate being created</typeparam>
    /// <returns>A result with a new instance of the aggregate or reasons the rehydration failed</returns>
    protected static Result<T> Rehydrate<T>(Func<Guid, T> typeGenerator, IEnumerable<ChangeEvent> eventHistory)
        where T : EventSourcedAggregateRoot<TRoot, TId>
    {
        var changeEvents = eventHistory.OrderBy(e => e.EventSequenceNumber).ToArray();

        var sequenceAnalysis = ChangeEventSequenceAnalysis(changeEvents);
        if (!sequenceAnalysis.IsValid)
        {
            return Result.Fail(sequenceAnalysis.InvalidationReasons.ToArray());
        }
        
        var entity = typeGenerator(sequenceAnalysis.CreationEvent!.AggregateId);

        entity._currentEventSequenceNumber = changeEvents.Last().EventSequenceNumber!;

        foreach (var changeEvent in changeEvents)
        {
            entity.Restore(changeEvent);
        }

        return Result.Success(entity);
    }

    /// <summary>
    /// Determines the sequence of events to be rehydrated from
    /// makes sense.
    /// </summary>
    /// <param name="changeEvents"></param>
    /// <returns></returns>
    private static (bool IsValid, List<string> InvalidationReasons, CreationEvent<TId>? CreationEvent) 
        // ReSharper disable once CyclomaticComplexity
        // It's not super great but it's just validations
        ChangeEventSequenceAnalysis(ChangeEvent[] changeEvents)
    {
        var isValid = true;
        var invalidationReasons = new List<string>();

        if (!changeEvents.Any())
        {
            isValid = false;
            invalidationReasons.Add("There are no events to rehydrate from");
            return (isValid, invalidationReasons, null);
        }
        
        var possibleCreationEvent = changeEvents.First() as CreationEvent<TId>;

        if (possibleCreationEvent is null)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event is missing");
        }
        
        if(possibleCreationEvent is not null &&
           possibleCreationEvent.EventSequenceNumber is null)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event was not created with a valid sequence. Events must be created through aggregates");
        }

        if (possibleCreationEvent is not null &&
            possibleCreationEvent.EventSequenceNumber is not null &&
            possibleCreationEvent.EventSequenceNumber != 1)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event is not in sequence");           
        }

        var sequence = new EventSequenceNumber(1);
        foreach (var changeEvent in changeEvents.Skip(1))
        {
            if (changeEvent is CreationEvent<TId>)
            {
                isValid = false;
                invalidationReasons.Add("The entity has multiple creation events");
            }

            if (changeEvent.EventSequenceNumber != sequence + 1)
            {
                isValid = false;
                invalidationReasons.Add($"A change event is missing between sequence {sequence} and {changeEvent}");
            }

            sequence += 1;
        }

        return (isValid, invalidationReasons, possibleCreationEvent);
    }
    
    /// <summary>
    /// Report a successful change
    /// </summary>
    /// <returns></returns>
    protected Result<TRoot> Success()
    {
        return Result.Success((TRoot)this);
    }

    /// <summary>
    /// Report a failed change and the reasons for failure
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    protected static Result<TRoot> Fail(params string[] failureReasons)
    {
        return Result.Fail(failureReasons);
    }      
 
}