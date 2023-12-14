namespace Truss.Dsl.Parameters.Exceptions;

public sealed class DslArgumentSyntaxException(string arg) 
    : Exception($"Argument \"{arg}\" is not in format \"name: value\"");