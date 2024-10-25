namespace Truss.Testing;

/// <summary>
/// Represents an exception that is thrown when a Driver tag is not found among the available tags.
/// </summary>
public sealed class DriverTagNotFoundException(string tag, IEnumerable<string> availableTags) 
    : Exception($"The override tag {tag} was not in the available tags [{string.Join(", ", availableTags)}]");