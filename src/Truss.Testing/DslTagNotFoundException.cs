namespace Truss.Testing;

/// <summary>
/// Represents an exception that is thrown when a DSL tag is not found among the available tags.
/// </summary>
public sealed class DslTagNotFoundException(string tag, IEnumerable<string> availableTags) 
    : Exception($"The override tag {tag} was not in the available tags [{string.Join(", ", availableTags)}]");