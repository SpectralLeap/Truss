namespace Truss.Testing.Dsl.SharedDependencies;

/// <summary>
/// Specifies an exposed type
/// that will be registered after creation
/// for other services to use for communication
/// with the dependency
/// </summary>
public sealed class SharedDependencyAdapterAttribute : Attribute;