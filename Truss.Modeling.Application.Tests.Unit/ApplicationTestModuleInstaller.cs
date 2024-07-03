using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Area;
using Truss.Modeling.Module;

namespace Truss.Modeling.Application.Tests.Unit;

public sealed class ApplicationTestModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
    }
}

public sealed class ApplicationTestAreaInstaller : IAreaInstaller
{
    public void InstallModules(IModuleAggregator moduleAggregator)
    {
        moduleAggregator
            .AddModule<ApplicationTestModuleInstaller>();
    }
}