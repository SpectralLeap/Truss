using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public class FixtureWithNonStaticServices : Fixture, IUsesServiceDefinition<BaseServiceDefinition>
{
    [BaseServices] public IServiceCollection NotStatic = new ServiceCollection();
}