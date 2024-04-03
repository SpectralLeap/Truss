using Newtonsoft.Json;
using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record NumberIncrementedEvent : ChangeEvent
{
    public NumberIncrementedEvent(Guid counterId) : base(counterId)
    {
    }
}