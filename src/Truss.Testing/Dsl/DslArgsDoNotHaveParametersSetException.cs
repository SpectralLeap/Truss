namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an exception that is thrown when the DSL argument class does not have any parameters set.
/// </summary>
public sealed class DslArgsDoNotHaveParametersSetException() : Exception("The Dsl Argument class does not have parameters");