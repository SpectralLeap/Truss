using Microsoft.Extensions.DependencyInjection;

namespace Truss;

// ReSharper disable once CheckNamespace
internal static class ServiceExtensions
{
    public static void Load(
        this IServiceCollection services,
        IEnumerable<ServiceDescriptor> serviceDescriptors
    )
    {
        foreach (var descriptor in serviceDescriptors)
        {
            services.Add(descriptor);
        }
    }
}