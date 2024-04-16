using System;
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed record WordAggregateId(Guid value) : AggregateRootId<Guid>(value)
{
    public static WordAggregateId New()
    {
        return new WordAggregateId(Guid.NewGuid());
    }
};