using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Dsl;

internal sealed class ServiceDefinition(IServiceCollection collection, string? tag = null)
{
    public readonly string? Tag = tag;
    public IServiceCollection Collection = collection;
}