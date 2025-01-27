using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Testing.AspNetCore;


public static class ServiceExtensions
{
    public static IServiceCollection AddWebServer<T>(
        this IServiceCollection injectedServices,
        Action<IConfigurationBuilder>? testApplicationConfiguration = null,
        Action<IServiceCollection>? testServiceConfiguration = null
    ) where T : class
    {
        return injectedServices
            .AddSingleton(_ => new WebApplicationFactory<T>()
                .WithWebHostBuilder(builder =>
                {
                    if (testApplicationConfiguration is not null)
                    {
                        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
                        {
                            testApplicationConfiguration.Invoke(configurationBuilder);
                        });
                    }

                    builder.ConfigureTestServices(servicesUnderTest =>
                        {
                            foreach (var service in injectedServices)
                            {
                                servicesUnderTest.Add(service);
                            }

                            testServiceConfiguration?.Invoke(servicesUnderTest);
                        });
                })
            )
            .AddSingleton<HttpClient>(p => p
                .GetRequiredService<WebApplicationFactory<T>>()
                .CreateClient()
            );
    }
}