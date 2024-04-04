using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public interface IUsesServiceDefinition<T> where T : IServiceDefinition;
public interface IServiceDefinition
{
    
}

public class BaseServiceDefinition : IServiceDefinition
{
    
}

public class FixtureWithNonStaticServices : Fixture, IUsesServiceDefinition<BaseServiceDefinition>
{
    [BaseServices] public IServiceCollection NotStatic = new ServiceCollection();
}