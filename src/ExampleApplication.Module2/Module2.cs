using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace ExampleApplication.Module2;

public sealed class Module2 : Module
{
    public override void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}