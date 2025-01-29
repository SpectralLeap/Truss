namespace Truss.Testing.Dsl;

/// <summary>
/// Exception thrown when a required parameter is not set
/// </summary>
/// <param name="names"></param>
public sealed class DslRequiredParameterNotSetException(params string[] names) 
    : Exception($"Required parameters \"{string.Join(", ", names)}\" did not have a value set");