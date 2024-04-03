using Truss.Application.Abstractions.Events;
using Truss.Application.Cqrs.EventSourcing.Reading;

namespace Truss.Application.Cqrs.EventSourcing.Common;

internal sealed class ChangeEventMapper : IChangeEventTypeMap
{
    private readonly Dictionary<EventType, Type> _map = new();
    private readonly Dictionary<Type, EventType> _coMap = new();

    public void Add(Type t)
    {
        _map.Add(t, t);
        _coMap.Add(t, t);
    }
    
    public Type Map(EventType eventType)
    {
        return _map[eventType];
    }

    public EventType Map(Type type)
    {
        return _coMap[type];
    }
}