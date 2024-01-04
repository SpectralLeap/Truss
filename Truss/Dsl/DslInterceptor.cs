using System.Reflection;
using Castle.DynamicProxy;
using Truss.Dsl.Arguments;

namespace Truss.Dsl;

internal sealed class DslInterceptor : IInterceptor
{
    private readonly IIntegrationBus _integrationBus;

    public DslInterceptor(IIntegrationBus integrationBus)
    {
        _integrationBus = integrationBus;
    }
    
    public void Intercept(IInvocation invocation)
    {
        if (invocation.MethodInvocationTarget.GetCustomAttribute<DslMethodAttribute>() is null) return;
        
        invocation.Proceed();

        var args = (DslArgs) invocation.ReturnValue;
        
        _integrationBus.Act(args);
    }
}