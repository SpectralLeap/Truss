namespace Truss.Dsl;

/// <summary>
/// Specifies that a field or property is to be used as a base service.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class BaseServicesAttribute : Attribute;

/// <summary>
/// Represents an attribute that is used to mark a field or property for overriding services with a specific tag.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ServiceOverride(string tag) : Attribute
{
    /// <summary>
    /// Gets the tag of the property.
    /// </summary>
    /// <value>
    /// The tag of the property.
    /// </value>
    public string Tag { get; } = tag;
}

/// <summary>
/// Represents an attribute that can be used to mark a method as a DSL method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class DslMethodAttribute : Attribute;