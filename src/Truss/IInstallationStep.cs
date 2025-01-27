using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss;

public interface IInstallationStep
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration,
        ModuleManifest moduleManifest
    );
}