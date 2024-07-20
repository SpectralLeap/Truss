using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record NumberIncrementedEvent : ChangeEvent
{
    public NumberIncrementedEvent(Guid counterId) : base(counterId)
    {
    }
}