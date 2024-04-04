using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;

namespace Truss.Testing.Dsl.Tests.Services;

public interface IUsesServiceDefinition<T> where T : IServiceDefinition;
public interface IServiceDefinition
{
    
}

public class BaseServiceDefinition : IServiceDefinition
{
    
}

public class DslWithNonStaticServices : Dsl, IUsesServiceDefinition<BaseServiceDefinition>
{
    [BaseServices] public IServiceCollection NotStatic = new ServiceCollection();
}