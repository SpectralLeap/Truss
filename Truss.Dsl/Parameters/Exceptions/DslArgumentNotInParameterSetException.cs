namespace Truss.Dsl.Parameters.Exceptions;

public sealed class DslArgumentNotInParameterSetException(string name, string[] keys) 
    : Exception($"Argument \"{name}\" is not in the parameter set. Available arguments are [{string.Join(", ", keys)}]");