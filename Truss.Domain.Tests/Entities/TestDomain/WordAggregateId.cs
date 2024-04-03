using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.Entities.TestDomain;

internal sealed record WordAggregateId(Guid value) : AggregateRootId<Guid>(value)
{
    public static WordAggregateId New()
    {
        return new WordAggregateId(Guid.NewGuid());
    }
};