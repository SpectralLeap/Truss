using System.Reflection;
using Truss.Testing;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class DomainSpecificLanguageFactoryLifetimeAdapter : IAsyncLifetime
{
    public DomainSpecificLanguageFactory DomainSpecificLanguageFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DomainSpecificLanguageFactory.InitializeAsync(new List<Assembly> {typeof(EntityFrameworkTests).Assembly}.AsReadOnly());
    }

    public async Task DisposeAsync()
    {
        await DomainSpecificLanguageFactory.DisposeAsync();
    }
}