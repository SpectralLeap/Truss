using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss.Modeling.Application.Tests.Unit;

public sealed class ApplicationTestModule : Module
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }
}