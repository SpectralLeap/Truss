namespace Truss.Infrastructure.FluentValidation;

public static class OptionsExtensions
{
    public static TrussServiceOptions AddFluentValidation(
        this TrussServiceOptions trussServiceOptions
    )
    {
        return trussServiceOptions
            .AddInstallationStep<FluentValidationInstallationStep>()
            .InstallModule<FluentValidationModule>();
    }
}