using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.AspNetCore;

public abstract class WebInstallationStep 
    : IInstallationStep
{
    public virtual void Run(
        WebApplication app,
        ModuleManifest moduleManifest
    )
    {
    }

    public virtual void Run(
        IServiceCollection services,
        IConfiguration configuration,
        ModuleManifest moduleManifest
    )
    {
    }   
}