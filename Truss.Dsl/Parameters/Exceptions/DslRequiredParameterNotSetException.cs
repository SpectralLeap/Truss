namespace Truss.Dsl.Parameters.Exceptions;

public sealed class DslRequiredParameterNotSetException(string name) 
    : Exception($"Required parameter \"{name}\" did not have a value set");