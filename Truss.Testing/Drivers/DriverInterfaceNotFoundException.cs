namespace Truss.Testing.Drivers;

internal sealed class DriverInterfaceNotFoundException(Type type)
    : Exception($"Driver for {type.Name} does not implement the driver interface");