using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed record WordAggregateId(Guid value) : AggregateId<Guid>(value)
{
    public static WordAggregateId New()
    {
        return new WordAggregateId(Guid.NewGuid());
    }

    public Guid value { get; } = value;
};