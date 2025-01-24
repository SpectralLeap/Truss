using Microsoft.Extensions.DependencyInjection;
using Truss.Infrastructure.EfCore.Tests.Persistence;
using Truss.Infrastructure.Tests.Dependencies;

namespace Truss.Infrastructure.EfCore.Tests;

public static class ServiceExtensions
{
    public static IServiceCollection AddTestCore(this IServiceCollection services)
    {
        return services
                .AddNpgsql<AutoShopContext>(connectionString: PostgresDatabaseSharedDependency.ConnectionString!)
                .AddSingleton<AutoShopService>()
            ;
    }
}