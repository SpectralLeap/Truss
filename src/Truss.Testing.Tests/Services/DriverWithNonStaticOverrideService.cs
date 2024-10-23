using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public class DriverWithNonStaticOverrideService : Driver
{
    [BaseServices] public static IServiceCollection Xyz = new ServiceCollection();
    [ServiceOverride("admin")] public IServiceCollection Or = new ServiceCollection();
}