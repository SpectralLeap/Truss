using System.ComponentModel.DataAnnotations;

namespace Installation.Configs;

public sealed class ServiceInstallationConfig
{
    /// <summary>
    /// The name of the service.
    /// </summary>
    [Required(ErrorMessage = $"The {nameof(Name)} field is required and should be a title cased string")]
    public required string Name { get; init; }

    /// <summary>
    /// A succinct description of the service.
    /// </summary>
    [Required(ErrorMessage = $"The {nameof(Description)} field is required and should be a succinct description of the service")]
    public required string Description { get; init; }

    /// <summary>
    /// Areas that the service supports.
    /// These are groupings of modules that are logically related.
    /// </summary>
    [Required(ErrorMessage = $"The {nameof(Areas)} field is required and should be an array of {nameof(AreaConfig)}")]
    public required AreaConfig[] Areas { get; init; }

    /// <summary>
    /// Modules that support the service.
    /// Generally this refers to infrastructure and other services that don't belong
    /// to a logical area.
    /// </summary>
    [Required(ErrorMessage = $"The {nameof(Modules)} field is required and should be an array of {nameof(ModuleConfig)}")]
    public required ModuleConfig[] Modules { get; init; }
}