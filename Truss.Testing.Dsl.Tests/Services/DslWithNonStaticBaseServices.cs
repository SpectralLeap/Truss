using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslWithNonStaticBaseServices : Dsl
{
    [BaseServices] public IServiceCollection NotStatic = new ServiceCollection();
}