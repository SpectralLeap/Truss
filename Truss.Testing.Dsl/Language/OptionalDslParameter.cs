namespace Truss.Testing.Dsl.Language;

public sealed class OptionalDslParameter(string name) : DslParameter(name)
{
    public override string? Value => SetValue ?? _defaultValue;
    private string? _defaultValue;
    
    public DslParameter SetDefault(string value)
    {
        _defaultValue = value;
         
        return this;
    }
}