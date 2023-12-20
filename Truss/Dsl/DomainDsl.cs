using Microsoft.Extensions.DependencyInjection;
using Truss.Dsl.Arguments;

namespace Truss.Dsl;

public abstract class DomainDslOverrideSet<TDsl>(string tag, IServiceCollection overrideCollection) where TDsl : DomainDsl;

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