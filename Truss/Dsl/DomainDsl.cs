using Truss.Dsl.Arguments;

namespace Truss.Dsl;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class BaseServicesAttribute : Attribute
{
    
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class OverrideServicesAttribute(string tag) : Attribute
{
    public string Tag { get; } = tag;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class DslMethodAttribute : Attribute
{
    
}

public abstract class DomainDsl(IIntegrationBus integrationBus)
{
    protected void Act<TAction>(DslArgs args)
    {
        integrationBus.Act<TAction>(args);
    }

    public void Assert<TAssertion>(Action<TAssertion>? expectation = null)
    {
    }
}