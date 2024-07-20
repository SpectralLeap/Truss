using Truss.Testing.Services;

namespace Truss.Testing;

/// <summary>
/// The exception that is thrown when services requested by a specific type were not registered.
/// </summary>
public sealed class DslServicesNotRegisteredException(Type type) 
    : Exception($"Services requested by {type.Name} were not registered on the type." 
                + $" Assure all types requested for are registered in a {nameof(BaseServicesAttribute)}");