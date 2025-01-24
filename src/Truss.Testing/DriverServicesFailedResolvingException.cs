using Truss.Testing.Services;

namespace Truss.Testing;

/// <summary>
/// The exception that is thrown when services requested by a specific type were not registered.
/// </summary>
public sealed class DriverServicesFailedResolvingException(
    Type type,
    string message
)
    : Exception(
        $"Services failed resolving when constructing {type.Name}."
        + $" {message}. "
        + $" This may be because they are not registered in a {nameof(BaseServicesAttribute)} or an error occurred."
    );