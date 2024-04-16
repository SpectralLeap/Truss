using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.EfCore.TestCore.Domain;

public sealed record AutoShopId 
    : AggregateRootId<Guid>
{
    public AutoShopId(Guid Value) : base(Value)
    {
    }
}