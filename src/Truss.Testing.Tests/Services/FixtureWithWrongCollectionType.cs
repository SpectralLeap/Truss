using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public class FixtureWithWrongCollectionType : Fixture
{
    [BaseServices] public static IServiceProvider IncorrectType = new ServiceCollection().BuildServiceProvider();
}