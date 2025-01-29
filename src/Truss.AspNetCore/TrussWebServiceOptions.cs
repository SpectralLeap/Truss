namespace Truss.AspNetCore;

public sealed class TrussWebServiceOptions : TrussServiceOptions
{
    public string? ApiBasePath { get; set; } = "api";
    public bool UseModuleNameInApiPath { get; set; } = true;
}
