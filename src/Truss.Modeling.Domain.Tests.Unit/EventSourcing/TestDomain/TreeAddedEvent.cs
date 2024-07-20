using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record TreeAddedEvent : ChangeEvent
{
    public TreeId TreeId { get; }
    
    public string TreeType { get; }

    public TreeAddedEvent(OrchardId id, TreeId treeId, string treeType) : base(id.Value)
    {
        this.TreeId = treeId;
        this.TreeType = treeType;
    }

}