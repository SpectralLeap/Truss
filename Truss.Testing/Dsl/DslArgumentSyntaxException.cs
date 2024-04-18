namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an exception that is thrown when an argument in a DSL syntax is not in the expected format.
/// </summary>
public sealed class DslArgumentSyntaxException(string arg) 
    : Exception($"Argument \"{arg}\" is not in format \"name: value\"");