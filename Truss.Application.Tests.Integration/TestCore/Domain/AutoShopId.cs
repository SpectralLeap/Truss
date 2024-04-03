using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Tests.Integration;

public sealed record AutoShopId 
    : AggregateRootId<Guid>
{
    public AutoShopId(Guid Value) : base(Value)
    {
    }
}