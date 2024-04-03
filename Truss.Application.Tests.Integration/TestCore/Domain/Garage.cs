using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration;

public sealed class Garage : Entity<GarageId>
{
    public IReadOnlyCollection<Car> Cars => _cars;
    private List<Car> _cars;
    
    public Garage(GarageId id) : base(id)
    {
    }
}