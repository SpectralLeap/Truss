using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration.TestCore.Domain;

public sealed class AutoShop : AggregateRoot<AutoShopId, Guid>
{
    public string Name { get; private set; }
    
    public IReadOnlyCollection<Garage> Garages => _garages;
    
    private List<Garage> _garages = new();
    
    public AutoShop(AutoShopId id, string name) : base(id)
    {
        Name = name;
    }
}