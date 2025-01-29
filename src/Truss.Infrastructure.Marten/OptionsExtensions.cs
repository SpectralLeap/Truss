namespace Truss.Infrastructure.Marten;

public static class OptionsExtensions
{
    public static TrussServiceOptions AddMarten(
        this TrussServiceOptions trussServiceOptions
    )
    {
        return trussServiceOptions
            .InstallModule<MartenModule>();
    }
}