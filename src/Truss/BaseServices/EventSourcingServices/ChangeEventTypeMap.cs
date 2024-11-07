using System.Reflection;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.BaseServices.EventSourcingServices;

internal sealed class ChangeEventTypeMap
{
    private readonly Dictionary<EventType, Type> _map = new();
    private readonly Dictionary<Type, EventType> _coMap = new();

    public ChangeEventTypeMap(Assembly[] assemblies)
    {
        assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IChangeEvent).IsAssignableFrom(type))
            .ToList()
            .ForEach(type => Add(type))
            ;
    }

    private void Add(Type t)
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