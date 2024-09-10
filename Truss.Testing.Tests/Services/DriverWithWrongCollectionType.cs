using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Services;

public sealed class DriverWithWrongCollectionType : Driver
{
    [BaseServices] public static IServiceProvider IncorrectType = new ServiceCollection().BuildServiceProvider();
}