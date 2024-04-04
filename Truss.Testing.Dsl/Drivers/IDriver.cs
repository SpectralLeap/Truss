using Truss.Testing.Dsl.Language;

namespace Truss.Testing.Dsl.Drivers;

/// <summary>
/// Should be implemented through using the Driver{T} abstract type
/// <br/>
/// <br/>
/// Marker interface for invoking the drive functionality
/// </summary>
public interface IDriver
{
    /// <summary>
    /// Drives the object for the specific action
    /// </summary>
    /// <param name="args">The DSL arguments for driving the object</param>
    public Task Drive(DslArgs args);
}