using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Events;

/// <summary>
/// The type of an event
/// </summary>
public sealed record EventType
{
    [JsonProperty]
    private string Value { get; }
    
    private static readonly int MaxLength = 50;
    private static readonly Regex WhiteSpace = new("\\s");
    private static readonly Regex NonWordCharacters = new("\\W");


    [JsonConstructor]
    private EventType(string value)
    {
        if (value is null || string.IsNullOrEmpty(value)) throw new ArgumentException("Event types must have a value");
                
        if (value.Length > MaxLength) throw new ArgumentException($"Event types cannot be longer than {MaxLength} characters");
                
        if (WhiteSpace.IsMatch(value)) throw new ArgumentException("Event types cannot contain whitespace characters");
                
        if (NonWordCharacters.IsMatch(value)) throw new ArgumentException("Event types cannot contain non word characters");
                
        Value = value;
    }

    /// <summary>
    /// Implicit conversion to a <see cref="string"/>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static implicit operator string(EventType e) => e.Value;
    
    /// <summary>
    /// Implicit conversion from a <see cref="string"/> to an <see cref="EventType"/>
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static implicit operator EventType(string s) => new(s);
    
    /// <summary>
    /// Implicit conversion to a valid <see cref="EventType"/> from a <see cref="Type"/>
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static implicit operator EventType(Type t)
    {
        var typeName = t.Name;
        if (!typeName.EndsWith("Event"))
            throw new ArgumentException($"The type name {typeName} is invalid for an event. Event type names must end with Event");

        return string.Join("", typeName.Substring(0, typeName.Length - 5));
    }
}