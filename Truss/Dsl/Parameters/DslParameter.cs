using System.Text.RegularExpressions;

namespace Truss.Dsl.Parameters;


public sealed class DslValueNotAvailableException(string name, string value, string[] availableValues)
    : Exception($"For parameter \"{name}\" the value \"{value}\" was not in available values [{string.Join(", ", availableValues.Select(v => $"\"{v}\""))}]");

public abstract class DslParameter(string name)
{
    public abstract string? Value { get; }
    public string Name { get; } = name;

    protected string? SetValue;

    private string[]? _availableValues;
    
    private bool _isList;

    private string? _pattern;


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

    public DslParameter SetPattern(string pattern)
    {
        _pattern = pattern;
        return this;
    }
    
    public void TrySetValue(string value)
    {
        if (_availableValues is not null && IsNotInAvailable(value))
            throw new DslValueNotAvailableException(Name, value, _availableValues);

        if (_pattern is not null && DoesNotNatchPattern(value))
            throw new DslValueDoesNotMatchPattern(Name, value, _pattern);
        
        SetValue = value;
    }

    private bool DoesNotNatchPattern(string value)
    {
        if (!_pattern!.StartsWith("^")) _pattern = "^" + _pattern;
        
        if (!_pattern!.EndsWith("$")) _pattern += "$";

        var regex = new Regex(_pattern);
        
        var values = Split(value);

        foreach (var v in values)
        {
            if (!regex.IsMatch(v)) return true;
        }

        return false;
    }

    private bool IsNotInAvailable(string value)
    {
        var values = Split(value);

        return values.Any(v => ! _availableValues!.Contains(v));
    }

    private string[] Split(string value)
    {
        return _isList ?
                value.Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim())
                    .ToArray()
                : [value]
            ;
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

public sealed class DslValueDoesNotMatchPattern(string name, string value, string pattern)
    : Exception($"For parameter \"{name}\" the value \"{value}\" did not match pattern {pattern}");