namespace Truss.Testing.Dsl;

/// <summary>
/// Represents a parameter that is optional
/// </summary>
/// <param name="name"></param>
public sealed class OptionalDslParameter(string name) : DslParameter(name)
{
    /// <summary>
    /// Gets the value of the parameter or a default value
    /// </summary>
    public override string? Value => SetValue ?? _defaultValue;
    private string? _defaultValue;
    
    /// <summary>
    /// Sets the default value of the parameter
    /// </summary>
    /// <param name="value">
    /// The default value to set
    /// </param>
    /// <returns>
    /// The parameter for chaining
    /// </returns>
    public DslParameter SetDefault(string value)
    {
        _defaultValue = value;
         
        return this;
    }
}