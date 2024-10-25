using System.Reflection;

namespace Truss.Testing;

/// <summary>
/// Represents an exception that is thrown when a Driver service is not defined as Static
/// </summary>
public sealed class DriverServicesNotStaticException(MemberInfo info) 
    : Exception($"The service definition {info.Name} is not static. Dsl Services must be a static field or property");