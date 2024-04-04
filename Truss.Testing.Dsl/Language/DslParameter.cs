using System.Text.RegularExpressions;

namespace Truss.Testing.Dsl.Language;

/// <summary>
/// Represents a DSL parameter.
/// </summary>
public abstract class DslParameter(string name)
{
    /// <summary>
    /// Represents a DSL parameter value.
    /// </summary>
    public abstract string? Value { get; }

    /// <summary>
    /// Represents the name of a DSL parameter.
    /// </summary>
    /// <value>
    /// The name of the parameter.
    /// </value>
    public string Name { get; } = name;

    /// <summary>
    /// Represents a DSL parameter.
    /// </summary>
    /// <remarks>
    /// This class is an abstract base class for DSL parameters.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Create a new DSL parameter
    /// DslParameter param = new DslParameter("Name");
    /// // Set available values for the parameter
    /// param.SetAvailableValues("Value1", "Value2", "Value3");
    /// // Set the parameter value
    /// param.TrySetValue("Value1");
    /// // Get the parameter value
    /// string value = param.Value;
    /// </code>
    /// </example>
    protected string? SetValue;

    /// <summary>
    /// Represents a DSL parameter with available values.
    /// </summary>
    private string[]? _availableValues;

    /// <summary>
    /// Represents a DSL parameter.
    /// </summary>
    /// <remarks>
    /// This class is used to define a DSL parameter.
    /// </remarks>
    private bool _isList;

    /// the values of the DSL parameter.
    private string? _pattern;

    private char _listDelimiter = ',';

    /// <summary>
    /// Sets the available values for the DSL parameter.
    /// </summary>
    /// <param name="values">An array of strings representing the available values.</param>
    /// <returns>A reference to the current instance of DslParameter.</returns>
    public DslParameter SetAvailableValues(params string[] values)
    {
        _availableValues = values;
        return this;
    }

    /// <summary>
    /// Marks a DSL parameter as a list parameter.
    /// </summary>
    /// <returns>The same <see cref="DslParameter"/> instance.</returns>
    public DslParameter AsList(char delimiter = ',')
    {
        _isList = true;
        _listDelimiter = delimiter;
        return this;
    }

    /// <summary>
    /// Sets the pattern for the DSL parameter.
    /// </summary>
    /// <param name="pattern">The pattern to set for the parameter.</param>
    /// <returns>The updated DSL parameter.</returns>
    public DslParameter SetPattern(string pattern)
    {
        _pattern = pattern;
        return this;
    }

    /// <summary>
    /// Tries to set the value for the DSL parameter.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <exception cref="DslValueNotAvailableException">Thrown if the value is not in the available values.</exception>
    /// <exception cref="DslValueDoesNotMatchPattern">Thrown if the value does not match the specified pattern.</exception>
    public void TrySetValue(string value)
    {
        if (_availableValues is not null && IsNotInAvailable(value))
            throw new DslValueNotAvailableException(Name, value, _availableValues);

        if (_pattern is not null && DoesNotNatchPattern(value))
            throw new DslValueDoesNotMatchPattern(Name, value, _pattern);
        
        SetValue = value;
    }

    /// Determines whether the given value does not match the specified pattern.
    /// @param value The value to check.
    /// @returns `true` if the value does not match the pattern, otherwise `false`.
    /// /
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

    /// <summary>
    /// Checks if a value is not in the available values of a DSL parameter.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is not in the available values, false otherwise.</returns>
    private bool IsNotInAvailable(string value)
    {
        var values = Split(value);

        return values.Any(v => ! _availableValues!.Contains(v));
    }

    /// <summary>
    /// Splits a string value into an array of substrings based on a specified delimiter.
    /// </summary>
    /// <param name="value">The string value to split.</param>
    /// <returns>An array of substrings.</returns>
    private string[] Split(string value)
    {
        return _isList ?
                value.Split([ _listDelimiter ], StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim())
                    .ToArray()
                : [value]
            ;
    }

    /// <summary>
    /// Represents an optional DSL parameter.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>An instance of OptionalDslParameter.</returns>
    public static OptionalDslParameter Optional(string name)
    {
        return new OptionalDslParameter(name);
    }

    /// <summary>
    /// Represents a required DSL parameter.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>A new instance of RequiredDslParameter.</returns>
    public static RequiredDslParameter Required(string name)
    {
        return new RequiredDslParameter(name);
    }

}

/// <summary>
/// Exception that is thrown when a DSL parameter value does not match the specified pattern.
/// </summary>
public sealed class DslValueDoesNotMatchPattern(string name, string value, string pattern)
    : Exception($"For parameter \"{name}\" the value \"{value}\" did not match pattern {pattern}");
    
    
/// <summary>
/// Exception that is thrown when a DSL parameter value is not available.
/// </summary>
public sealed class DslValueNotAvailableException(string name, string value, string[] availableValues)
    : Exception($"For parameter \"{name}\" the value \"{value}\" was not in available values [{string.Join(", ", availableValues.Select(v => $"\"{v}\""))}]");
