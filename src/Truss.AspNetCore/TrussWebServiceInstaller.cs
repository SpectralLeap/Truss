using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.AspNetCore.MessageToEndpointMapping;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore;

public sealed class TrussWebServiceInstaller : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpContextAccessor()
            .AddSingleton<MessageToEndpointHandler>();
    }
}