using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure.FluentValidation;

public sealed class FluentValidationModule : Module
{
    public override string Name => "Truss.Infrastructure.FluentValidation";
}

public sealed class FluentValidationServiceInstaller : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(FluentValidationBehavior<,>)
        );
    }
}