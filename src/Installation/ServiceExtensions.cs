using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Installation.InstallerServices;
using Microsoft.AspNetCore.Routing;

namespace Installation;

/// <summary>
///
/// </summary>
public static class ServiceExtensions
{
    private static InstallationManager? _installer;

    /// <summary>
    /// Use the service installer to install services to the builder
    /// </summary>
    /// <param name="builder">
    /// The web application builder to install services to
    /// </param>
    /// <param name="configureServices">
    /// An optional action to configure the installer's services like adding logging
    /// </param>
    /// <param name="scanningInstallations">
    /// An optional list of actions to install services that require scanning the installation's assemblies
    /// </param>
    public static void UseServiceInstaller(
        this WebApplicationBuilder builder,
        Action<IServiceCollection>? configureServices = null,
        params Action<IReadOnlyCollection<Assembly>, IServiceCollection>[] scanningInstallations
    )
    {
        _installer = new InstallationManager();

        _installer.Install(
            builder,
            configureServices,
            scanningInstallations
        );
    }

    /// <summary>
    /// Use the service installation to map endpoints to the builder
    /// </summary>
    /// <param name="app">
    /// The web application to map endpoints to
    /// </param>
    /// <param name="endpointAssemblyMappingSteps">
    /// The steps to map endpoints to the builder
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the installation has not been initialized
    /// </exception>
    public static void UseServiceInstallation(
        this WebApplication app,
        params Action<IEnumerable<Assembly>, IEndpointRouteBuilder>[] endpointAssemblyMappingSteps
    )
    {
        if (_installer is null)
        {
            throw new InvalidOperationException(
                "Installation not initialized. Ensure UseServiceInstaller is called first"
            );
        }

        _installer.Map(
            app,
            endpointAssemblyMappingSteps
        );

        _installer.Dispose();
    }
}
