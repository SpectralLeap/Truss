using Newtonsoft.Json;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.DefaultServices.EventSourcingServices;

/// <summary>
/// Serialization for streaming change events
/// </summary>
internal sealed class ChangeEventSerializer
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

}