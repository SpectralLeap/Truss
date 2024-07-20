namespace Truss.Testing.Dsl;

public sealed class DslRequiredParameterNotSetException(params string[] names) 
    : Exception($"Required parameters \"{string.Join(", ", names)}\" did not have a value set");