namespace Truss.Testing.Dsl;

/// <summary>
/// Exception that is thrown when a DSL parameter value does not match the specified pattern.
/// </summary>
public sealed class DslValueDoesNotMatchPattern(string name, string value, string pattern)
    : Exception($"For parameter \"{name}\" the value \"{value}\" did not match pattern {pattern}");