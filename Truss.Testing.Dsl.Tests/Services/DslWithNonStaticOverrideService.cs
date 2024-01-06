using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslWithNonStaticOverrideService : Dsl
{
    [BaseServices] public static IServiceCollection Xyz = new ServiceCollection();
    [ServiceOverride("admin")] public IServiceCollection Or = new ServiceCollection();
}