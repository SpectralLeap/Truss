using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public class DriverWithNonStaticServices : Driver, IUsesServiceDefinition<BaseServiceDefinition>
{
    [BaseServices] public IServiceCollection NotStatic = new ServiceCollection();
}