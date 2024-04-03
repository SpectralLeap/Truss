using Newtonsoft.Json;
using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Cqrs.EventSourcing.Common;

/// <summary>
/// Serialization for streaming change events
/// </summary>
internal sealed class JsonChangeEventSerializer : IChangeEventSerializer
{
    /// <summary>
    /// Serialize events into storable payloads
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public string Serialize(ChangeEvent e)
    {
        return JsonConvert.SerializeObject(e);
    }

    /// <summary>
    /// Deserialize an event from a stored payload
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="serializedEvent"></param>
    /// <returns></returns>
    public ChangeEvent Deserialize(Type eventType, string serializedEvent)
    {
        return (ChangeEvent)JsonConvert.DeserializeObject(serializedEvent, eventType)!;
    }
}