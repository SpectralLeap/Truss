using System;
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.TestCore.Domain;

public sealed class AutoShop : AggregateRoot<AutoShopId>
{
    public string Name { get; private set; }
    
    public IReadOnlyCollection<Garage> Garages => _garages;
    
    private List<Garage> _garages = new();
    
    public AutoShop(AutoShopId id, string name) : base(id)
    {
        Name = name;
    }
}