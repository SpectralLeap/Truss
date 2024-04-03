using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Cqrs.EventSourcing.Common;

public interface IChangeEventSerializer
{
    public string Serialize(ChangeEvent e);
    public ChangeEvent Deserialize(Type eventType, string serializedEvent);
}