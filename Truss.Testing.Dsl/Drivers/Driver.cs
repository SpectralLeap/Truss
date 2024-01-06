namespace Truss.Testing.Dsl.Drivers;

/// <summary>
/// Represents an abstract driver.
/// </summary>
/// <typeparam name="TAction">The type of action performed by the driver.</typeparam>
public abstract class Driver<TAction> : IDriver
{
    /// <summary>
    /// Drives the object with the specified arguments.
    /// </summary>
    /// <param name="args">The arguments to drive the object.</param>
    public abstract Task Drive(DslArgs args);
}