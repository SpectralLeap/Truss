using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed class WordAggregate 
    : Aggregate<WordAggregateId>
{
    public void UpdateWord(string word)
    {
        ApplyAndAddPendingEvent(new WordUpdatedEvent(word));
    }

    public void AnnounceNumber(int i)
    {
        var entity = new NumberEntity
        {
            Id = Guid.NewGuid()
        };

        entity.UpdateNumber(i);
        ApplyAndAddPendingEvent(new NumberUpdatedEvent(i));
    }
}