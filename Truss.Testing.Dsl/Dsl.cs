using Truss.Testing.Dsl.Attributes;

namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an abstract class for DSL (Domain Specific Language) implementation.
/// </summary>
public abstract class Dsl
{
    /// <summary>
    /// Override to 
    /// </summary>
    /// <param name="args"></param>
    [DslMethod]
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // This is used for the proxy interceptor
    protected virtual Task Act(DslArgs args)
    {
        return Task.CompletedTask;
    }
}