using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Module;

namespace Truss.Modeling.Domain.Tests.Unit;

#pragma warning disable CS8625
public sealed class DomainTestModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
    }
}