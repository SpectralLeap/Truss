using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration.TestCore.Domain;

public sealed record AutoShopId 
    : AggregateRootId<Guid>
{
    public AutoShopId(Guid Value) : base(Value)
    {
    }
}