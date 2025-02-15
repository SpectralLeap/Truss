using System.Reflection;
using Installation.Configs;
using Installation.InstallerServices;
using Installation.Abstractions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Installation;

internal sealed class InstallationManager :
    IDisposable,
    IAsyncDisposable
{
    private ServiceProvider? _installationServices;
    private InstallerAgent? _installerAgent;

    /// <summary>
    /// Installs the module and its submodules
    /// </summary>
    /// <param name="builder">
    ///     The web application builder
    /// </param>
    /// <param name="configureServices">
    ///     An optional action to configure services like adding logging
    /// </param>
    /// <param name="serviceRegistrationSteps"></param>
    public void Install(WebApplicationBuilder builder,
        Action<IServiceCollection>? configureServices = null,
        params Action<IReadOnlyCollection<Assembly>, IServiceCollection>[] serviceRegistrationSteps)
    {
        var services = new ServiceCollection()
            .AddSingleton<InstallerAgent>()
            .AddSingleton<InstallationManifestGenerator>()
            .AddLogging();

        services.AddConfig<ServiceInstallationConfig>(builder.Configuration);

        configureServices?.Invoke(services);

        _installationServices = services.BuildServiceProvider();

        _installerAgent = _installationServices.GetRequiredService<InstallerAgent>();

        _installerAgent.InstallToServices(
            builder.Services,
            builder.Configuration,
            serviceRegistrationSteps
        );
    }

    public void Map(
        IEndpointRouteBuilder builder,
        Action<IEnumerable<Assembly>, IEndpointRouteBuilder>[] endpointAssemblyMappingSteps
    )
    {
        if (_installerAgent is null) throw new InvalidOperationException("Installation has not been run");

        _installerAgent.Map(builder, endpointAssemblyMappingSteps);
    }

    public void Dispose()
    {
        _installationServices?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_installationServices != null) await _installationServices.DisposeAsync();
    }
}