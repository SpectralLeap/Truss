using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;
using Truss.Dsl.Arguments;

namespace Truss;

internal interface IIntegrationBus
{
    public void Act(DslArgs args);
}


internal sealed class ActionDriverNotFoundException(Type type)
    : Exception($"Action driver for {type.Name} was not found");


internal sealed class IntegrationBus(IServiceProvider serviceProvider) : IIntegrationBus
{
    public void Act(DslArgs args)
    {
        var driverType = typeof(Driver<>).MakeGenericType(args.ActionType);
                        
        var driver = serviceProvider.GetService(driverType);

        if (driver is null) throw new ActionDriverNotFoundException(args.ActionType);

        var driveMethod = driverType.GetMethod("Drive");
        
        driveMethod?.Invoke(driver, new object[] {args});
    }

}

