namespace Truss.Testing.Services;

/// <summary>
/// Specifies that a field or property is to be used as a base service.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public sealed class BaseServicesAttribute : Attribute;