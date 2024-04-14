using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public sealed class NumberEntity : Entity<Guid>
{
    public NumberEntity() : this(Guid.NewGuid())
    {
        
    }
    
    private NumberEntity(Guid id) : base(id)
    {
    }

    public void UpdateNumber(int number)
    {
    }
}

public sealed record NumberUpdatedEvent(int Number) : DomainEvent;