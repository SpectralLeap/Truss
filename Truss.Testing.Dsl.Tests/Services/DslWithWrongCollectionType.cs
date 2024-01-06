using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslWithWrongCollectionType : Dsl
{
    [BaseServices] public static IServiceProvider IncorrectType = new ServiceCollection().BuildServiceProvider();
}