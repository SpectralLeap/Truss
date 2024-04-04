using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record OrchardId : AggregateRootId<Guid>
{
    public OrchardId(Guid value) : base(value)
    {
    }
}