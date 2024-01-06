using Truss.Testing.Dsl.Attributes;

namespace Truss.Testing.Dsl;

/// <summary>
/// Represents an abstract class for DSL (Domain Specific Language) implementation.
/// </summary>
public abstract class Dsl
{
    /// <summary>
    /// Do not override
    /// </summary>
    /// <param name="args"></param>
    [DslMethod]
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // This is used for the proxy interceptor
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected virtual async Task Act(DslArgs args) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}