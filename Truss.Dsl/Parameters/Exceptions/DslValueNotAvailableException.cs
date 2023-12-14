namespace Truss.Dsl.Parameters.Exceptions;

public sealed class DslValueNotAvailableException(string name, string value, string[] availableValues)
    : Exception($"For parameter \"{name}\" the value \"{value}\" was not in available values [{string.Join(", ", availableValues.Select(v => $"\"{v}\""))}]");