using System.Reflection;
using Castle.DynamicProxy;
using Truss.Testing.Dsl.Drivers;

namespace Truss.Testing.Dsl;

internal sealed class DslInterceptor(DriverDispatcher driverDispatcher) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        invocation.Proceed();
        
        // only handle DslMethod attributed methods
        if (invocation.MethodInvocationTarget.GetCustomAttribute<DslMethodAttribute>() is null) return;

        AsyncHandler(invocation);
    }
    
    private void AsyncHandler(IInvocation invocation)
    {
        var task = (Task)invocation.ReturnValue;
        
        invocation.ReturnValue = HandleAsync(task, invocation);
    }

    private async Task HandleAsync(Task task, IInvocation invocation)
    {
        await task.ConfigureAwait(false);

        var args = (DslArgs) invocation.Arguments[0];
        await driverDispatcher.CallDriver(args).ConfigureAwait(false);
    }
}