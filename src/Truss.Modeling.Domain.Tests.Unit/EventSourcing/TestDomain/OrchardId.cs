using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record OrchardId : AggregateRootId<Guid>
{
    public OrchardId(Guid value) : base(value)
    {
    }
}