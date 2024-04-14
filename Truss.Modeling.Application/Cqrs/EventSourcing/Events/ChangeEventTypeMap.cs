namespace Truss.Modeling.Application.Cqrs.EventSourcing.Events;

public sealed class ChangeEventTypeMap
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