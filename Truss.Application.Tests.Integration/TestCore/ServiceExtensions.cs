using Microsoft.Extensions.DependencyInjection;
using Truss.Application.Tests.Integration.TestCore.Persistence;

namespace Truss.Application.Tests.Integration.TestCore;

public static class ServiceExtensions
{
    public static IServiceCollection AddTestCore(this IServiceCollection services)
    {
        return services
                .AddNpgsql<AutoShopContext>(connectionString: Environment.GetEnvironmentVariable("PG_CONN_STRING"))
                .AddSingleton<AutoShopService>()
            ;
    }
}