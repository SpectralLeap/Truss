using Microsoft.Extensions.Logging;

namespace Truss.Testing.Dsl.Drivers;

internal sealed class DriverNotFoundException(Type type)
    : Exception($"Driver for {type.Name} was not found");

internal sealed class DriverInterfaceNotFoundException(Type type)
    : Exception($"Driver for {type.Name} does not implement the driver interface");

/// <summary>
/// Bus connecting the Dsls to the Drivers
/// </summary>
/// <param name="serviceProvider"></param>
internal sealed class DriverDispatcher(IServiceProvider serviceProvider, ILogger<DriverDispatcher> logger)
{
    public async Task CallDriver(DslArgs args)
    {
        logger.LogInformation("WTF");
        
        var driverType = typeof(Driver<>).MakeGenericType(args.ActionType);

        if (serviceProvider is null) throw new ApplicationException();
        var driver = serviceProvider.GetService(driverType);

        if (driver is null) throw new DriverNotFoundException(args.ActionType);

        // this should never happen
        if (driver is not IDriver driverImplementation) throw new DriverInterfaceNotFoundException(driver.GetType());
        
        await driverImplementation.Drive(args).ConfigureAwait(false);
    }

}

