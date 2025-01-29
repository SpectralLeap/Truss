using Microsoft.AspNetCore.Builder;

namespace Truss.AspNetCore;

public sealed class AppModuleInstallationStep : WebModuleInstallationStep
{
    public override void Run(
        WebApplication app,
        ModuleManifest moduleManifest
    )
    {
        if (moduleManifest.Module is not WebModule webModule) return;

        webModule.ConfigureApplication(app);

        webModule.MapEndpoints(app);
    }
}
