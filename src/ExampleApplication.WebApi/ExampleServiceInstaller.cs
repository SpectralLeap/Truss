using System.Reflection;
using ExampleApplication.WebApi.Services;
using MediatR;
using Truss.Modeling.Installation;

namespace ExampleApplication.WebApi;

public sealed class ExampleServiceInstaller : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(GreeterPipelineBehavior<,>)
        );
    }
}