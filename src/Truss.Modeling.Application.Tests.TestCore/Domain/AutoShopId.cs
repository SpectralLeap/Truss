using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.TestCore.Domain;

public sealed record AutoShopId 
    : AggregateId<Guid>
{
    public AutoShopId(Guid Value) : base(Value)
    {
    }
}