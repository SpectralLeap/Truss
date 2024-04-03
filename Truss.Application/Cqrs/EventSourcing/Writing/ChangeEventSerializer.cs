using Newtonsoft.Json;
using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Cqrs.EventSourcing.Common;

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