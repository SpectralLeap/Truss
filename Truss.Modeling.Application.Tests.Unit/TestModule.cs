using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Application.Tests.Unit;

public sealed class TestModule : ITrussModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
    }
}