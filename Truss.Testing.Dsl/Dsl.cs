using Truss.Testing.Dsl.Language;

namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an abstract class for DSL (Domain Specific Language) implementation.
/// </summary>
public abstract class Dsl
{
    /// <summary>
    /// Override to perform actions before the driver
    /// </summary>
    /// <param name="args"></param>
    [DslMethod]
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // This is used for the proxy interceptor
    // ReSharper disable once UnusedParameter.Global
    protected virtual Task Act(DslArgs args)
    {
        return Task.CompletedTask;
    }
}