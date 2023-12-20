using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;
using Truss.Dsl.Arguments;

namespace Truss;

public interface IIntegrationBus
{
    public void Act<TAction>(DslArgs args);
}


internal sealed class ActionDriverNotFoundException(Type type)
    : Exception($"Action driver for {type.Name} was not found");


internal sealed class IntegrationBus(IServiceProvider serviceProvider) : IIntegrationBus
{
    public void Act<TAction>(DslArgs args)
    {
        var driver = serviceProvider.GetService<Driver<TAction>>()!;

        if (driver is null) throw new ActionDriverNotFoundException(typeof(TAction));
        
        driver.Drive(args);
    }

}

