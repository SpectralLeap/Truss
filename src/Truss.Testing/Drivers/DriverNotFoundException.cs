namespace Truss.Testing.Drivers;

internal sealed class DriverNotFoundException(Type type)
    : Exception($"Driver for {type.Name} was not found");