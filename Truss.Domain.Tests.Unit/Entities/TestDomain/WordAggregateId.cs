using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Unit.Entities.TestDomain;

internal sealed record WordAggregateId(Guid value) : AggregateRootId<Guid>(value)
{
    public static WordAggregateId New()
    {
        return new WordAggregateId(Guid.NewGuid());
    }
};