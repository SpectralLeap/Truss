using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Installation.Abstractions.Services;

public interface IServicesInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration
    );
}
