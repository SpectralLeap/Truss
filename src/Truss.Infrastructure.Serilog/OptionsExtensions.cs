using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Truss.Infrastructure.Serilog;

public static class OptionsExtensions
{
    public static TrussServiceOptions AddSerilog(
        this TrussServiceOptions trussServiceOptions
    )
    {
        trussServiceOptions
            .InstallModule<SerilogModule>()
            .InstallerServices
            .AddLogging(c => c.AddSerilog());

        return trussServiceOptions;
    }
}