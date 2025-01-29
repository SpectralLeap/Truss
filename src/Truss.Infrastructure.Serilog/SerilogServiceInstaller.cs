using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure.Serilog;


public sealed class SerilogServiceInstaller : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddLogging(c => c.AddSerilog())
            .AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(SerilogLoggingBehavior<,>)
            );
    }
}