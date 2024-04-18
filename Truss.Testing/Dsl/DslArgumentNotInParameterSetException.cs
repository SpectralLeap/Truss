namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an exception that is thrown when an argument is not in the parameter set.
/// </summary>
public sealed class DslArgumentNotInParameterSetException(string name, string[] keys) 
    : Exception($"Argument \"{name}\" is not in the parameter set. Available arguments are [{string.Join(", ", keys)}]");