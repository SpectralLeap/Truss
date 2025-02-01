using System;
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.TestCore.Domain;

public sealed class AutoShop : Aggregate<Guid>
{
    public string Name { get; private set; }
    
    public IReadOnlyCollection<Garage> Garages => _garages;
    
    private List<Garage> _garages = new();
    
    public AutoShop(string name)
    {
        Name = name;
    }

    public static AutoShop CreateAutoShop(string name)
    {
        var shop = new AutoShop(name)
        {
            Id = Guid.NewGuid()
        };

        return shop;
    }
}