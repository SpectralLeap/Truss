using Microsoft.Extensions.DependencyInjection;

namespace Truss.Testing.Services;

internal sealed class ServiceDefinition(IServiceCollection collection, string? tag = null)
{
    public readonly string? Tag = tag;
    public readonly IServiceCollection Collection = collection;
}