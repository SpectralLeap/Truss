using Newtonsoft.Json;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Infrastructure.DefaultServices.EventSourcingServices;

internal sealed class ChangeEventDeserializer
{
    /// <summary>
    /// Deserialize an event from a stored payload
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="serializedEvent"></param>
    /// <returns></returns>
    public ChangeEvent Deserialize(Type eventType, string serializedEvent)
    {
        var deserializedEvent = JsonConvert.DeserializeObject(serializedEvent, eventType)!;
        
        return (ChangeEvent)deserializedEvent;
    }   
}