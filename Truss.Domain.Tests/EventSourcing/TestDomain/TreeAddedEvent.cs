using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Domain.Tests.EventSourcing.TestDomain;

public sealed record TreeAddedEvent : ChangeEvent
{
    public TreeId TreeId { get; init; }
    
    public string TreeType { get; init; }

    public TreeAddedEvent(OrchardId id, TreeId treeId, string treeType) : base(id.Value)
    {
        this.TreeId = treeId;
        this.TreeType = treeType;
    }

}