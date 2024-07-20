using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed class WordAggregate 
    : AggregateRoot<WordAggregateId, Guid>
{
    public WordAggregate(WordAggregateId id) : base(id)
    {
    }
 
    public void UpdateWord(string word)
    {
        RegisterDomainEvent(new WordUpdatedEvent(word));
    }

    public void AnnounceNumber(int i)
    {
        var entity = new NumberEntity();
        entity.UpdateNumber(i);
        RegisterDomainEvent(new NumberUpdatedEvent(i));
    }
}