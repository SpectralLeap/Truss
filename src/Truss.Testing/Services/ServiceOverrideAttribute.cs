namespace Truss.Testing.Services;

/// <summary>
/// Represents an attribute that is used to mark a field or property for overriding services with a specific tag.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public sealed class ServiceOverrideAttribute(string tag) : Attribute
{
    /// <summary>
    /// Gets the tag of the property.
    /// </summary>
    /// <value>
    /// The tag of the property.
    /// </value>
    public string Tag { get; } = tag;
}