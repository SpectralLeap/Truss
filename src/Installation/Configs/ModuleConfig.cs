using System.ComponentModel.DataAnnotations;

namespace Installation.Configs;

public sealed class ModuleConfig
{
    [Required(ErrorMessage = $"The {nameof(Name)} field is required, must be unique, and should be a title case string")]
    public required string Name { get; init; }

    [Required(ErrorMessage = $"The {nameof(Description)} field is required and should be a succinct description")]
    public required string Description { get; init; }

    [Required(ErrorMessage = $"The {nameof(PathBase)} field is required and must be of the form '/module'")]
    public required string PathBase { get; init; }

    [Required(ErrorMessage = $"The {nameof(Assemblies)} field is required and must be a list of strings of the form 'Assembly.Name'")]
    public required string[] Assemblies { get; init; }

    /// <summary>
    /// If true, the base path will be prepended to endpoints mappings
    /// </summary>
    public required bool UsePathBase { get; init; } = true;

    /// <summary>
    /// Submodules to be scanned and registered as a child
    /// </summary>
    public required ModuleConfig[] SubModules { get; init; } = [];
}