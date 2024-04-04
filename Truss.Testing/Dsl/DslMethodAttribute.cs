namespace Truss.Testing.Language;

/// <summary>
/// Represents an attribute that can be used to mark a method as a DSL method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class DslMethodAttribute : Attribute;