using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Domain.Tests.Unit;

#pragma warning disable CS8625
public sealed class TestModuleInstaller : ITrussModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
    }
}