using Truss.Domain.EventSourcing;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record NumberIncrementedEvent : ChangeEvent
{
    public NumberIncrementedEvent(Guid counterId) : base(counterId)
    {
    }
}