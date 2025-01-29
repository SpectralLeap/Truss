namespace Truss.AspNetCore;

public sealed class TrussWebServiceConfiguration : TrussServiceConfiguration
{
    public string? ApiBasePath { get; set; } = "api";
    public bool UseModuleNameInApiPath { get; set; } = true;
}
