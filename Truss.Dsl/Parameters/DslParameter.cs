using Truss.Dsl.Parameters.Exceptions;

namespace Truss.Dsl.Parameters;

public abstract class DslParameter(string name)
{
    public abstract string? Value { get; }
    public string Name { get; } = name;

    protected string? SetValue;

    private string[]? _availableValues;
    
    private bool _isList = false;


    public DslParameter SetAvailableValues(params string[] values)
    {
        _availableValues = values;
        
        return this;
    }

    public DslParameter AsList()
    {
        _isList = true;
        
        return this;
    }

    public void TrySetValue(string value)
    {
        if (_availableValues is not null && IsNotAvailable(value))
            throw new DslValueNotAvailableException(Name, value, _availableValues);
        
        this.SetValue = value;
    }

    private bool IsNotAvailable(string value)
    {
        var values = _isList ?
                value.Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim())
                    .ToArray()
                : [value]
            ;

        return values.Any(v => ! _availableValues!.Contains(v));
    }

    public static OptionalDslParameter Optional(string name)
    {
        return new OptionalDslParameter(name);
    }

    public static RequiredDslParameter Required(string name)
    {
        return new RequiredDslParameter(name);
    }
}