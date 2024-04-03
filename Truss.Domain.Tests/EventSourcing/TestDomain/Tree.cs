using Truss.Domain.Entities;

namespace Truss.Domain.Tests.EventSourcing.TestDomain;

public sealed class Tree : Entity<TreeId>
{
    public string TreeType { get; private set; }
    
    public Tree(TreeId id, string treeType) : base(id)
    {
        TreeType = treeType;
    }
}