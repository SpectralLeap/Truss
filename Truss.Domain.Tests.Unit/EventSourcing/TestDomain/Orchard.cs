using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Domain.EventSourcing;
using Truss.Results;

namespace Truss.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed class Orchard : EventSourcedAggregateRoot<Orchard, OrchardId>
{
    public List<Tree> Trees { get; } = new();
    public string? Name { get; private set; }

    private Orchard() : this(new OrchardId(Guid.NewGuid()))
    {
        RegisterChangeEvent(new OrchardCreatedEvent(Id, ""));
    }
    
    public Orchard(OrchardId? id, string name) : this(id)
    {
        RegisterChangeEvent(new OrchardCreatedEvent(id, name));
    }

    private Orchard(OrchardId? id) : base(id!) 
    {
    }
 
    protected override void Configure(IEventSourcingConfigurationBuilder<Orchard> builder)
    {
        builder.Handlers
            .AddHandler<OrchardCreatedEvent>(CreateOrchard)
            .AddHandler<TreeAddedEvent>(AddTree);
    }   
    
    private Result<Orchard> CreateOrchard(OrchardCreatedEvent e)
    {
        Name = e.Name;

        return Success();
    }

    public Result<Orchard> AddTree(string treeType)
    {
        if ("invalid".Equals(treeType)) return Fail("tree type cannot be invalid");
        
        var e = new TreeAddedEvent(Id, new TreeId(Guid.NewGuid()), treeType);
        
        return RegisterChangeEvent(e);
    }

    public Result<Orchard> ThrowException()
    {
        throw new InvalidCastException("Something is borked");
    }

    // This is bad to do
    public Result<Orchard> DoIncorrectCreation(string name)
    {
        return RegisterChangeEvent(new OrchardCreatedEvent(Id, name));
    }
    
    private Result<Orchard> AddTree(TreeAddedEvent obj)
    {
        Trees.Add(new Tree(obj.TreeId, obj.TreeType));
        
        return Success();
    }

    public static Orchard FromHistory(IEnumerable<ChangeEvent> events)
    {
        return Rehydrate(id => new Orchard(new OrchardId(id)), events).SuccessValue;
    }

    public static Orchard Create()
    {
        return new Orchard();
    }

}