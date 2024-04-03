using Truss.Application.Abstractions.Domain;
using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Entities.TestDomain;

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