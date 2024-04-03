using Truss.Application.Abstractions.Events;

namespace Truss.Application.Cqrs.EventSourcing.Reading;

public interface IChangeEventTypeMap
{
    Type Map(EventType eventType);
    EventType Map(Type type);
}