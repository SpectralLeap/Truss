using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Tests.EfCore.Persistence;

namespace Truss.Modeling.Application.Tests.EfCore;

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