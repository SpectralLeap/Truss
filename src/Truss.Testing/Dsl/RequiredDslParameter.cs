
namespace Truss.Testing.Dsl;

/// <summary>
/// Represents a parameter that is required
/// </summary>
/// <param name="name"></param>
public sealed class RequiredDslParameter(string name) : DslParameter(name)
{
    /// <summary>
    /// Gets the value of the parameter or throws an exception if not set
    /// </summary>
    /// <exception cref="DslRequiredParameterNotSetException">
    /// Thrown when the parameter is not set
    /// </exception>
    public override string Value => SetValue ?? throw new DslRequiredParameterNotSetException(Name);
}