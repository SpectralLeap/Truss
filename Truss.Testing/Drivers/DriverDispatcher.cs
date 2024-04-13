using Microsoft.Extensions.Logging;
using Truss.Testing.Dsl;

namespace Truss.Testing.Drivers;

internal sealed class DriverNotFoundException(Type type)
    : Exception($"Driver for {type.Name} was not found");

internal sealed class DriverInterfaceNotFoundException(Type type)
    : Exception($"Driver for {type.Name} does not implement the driver interface");

/// <summary>
/// Bus connecting the Dsls to the Drivers
/// </summary>
/// <param name="serviceProvider"></param>
internal sealed class DriverDispatcher(
    IServiceProvider serviceProvider,
    ILogger<DriverDispatcher> logger
)
{
    public async Task CallDriver(DslArgs args)
    {
        logger.LogDebug("Calling driver for action {ActionType}", args.ActionType);
        
        var driverType = typeof(Driver<>).MakeGenericType(args.ActionType);

        var driver = serviceProvider.GetService(driverType);

        if (driver is null)
        {
            logger.LogError("Could not find driver for action {ActionType}", args.ActionType);
            throw new DriverNotFoundException(args.ActionType);
        }

        // this should never happen
        if (driver is not IDriver driverImplementation) throw new DriverInterfaceNotFoundException(driver.GetType());
        
        await driverImplementation.Drive(args).ConfigureAwait(false);
    }

}

