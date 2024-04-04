namespace Truss.Testing.Dsl.Language;

/// <summary>
/// Represents an exception that is thrown when an argument in a DSL syntax is not in the expected format.
/// </summary>
public sealed class DslArgumentSyntaxException(string arg) 
    : Exception($"Argument \"{arg}\" is not in format \"name: value\"");

/// <summary>
/// Represents an exception that is thrown when an argument is not in the parameter set.
/// </summary>
public sealed class DslArgumentNotInParameterSetException(string name, string[] keys) 
    : Exception($"Argument \"{name}\" is not in the parameter set. Available arguments are [{string.Join(", ", keys)}]");

/// <summary>
/// Represents an exception that is thrown when the DSL argument class does not have any parameters set.
/// </summary>
public sealed class DslArgsDoNotHaveParametersSetException() : Exception("The Dsl Argument class does not have parameters");

/// <summary>
/// Represents a DSL (Domain-Specific Language) argument parser for a specific action type.
/// </summary>
public sealed class DslArgs
{
    internal readonly Type ActionType;
    private Dictionary<string, DslParameter>? _parameterSet;

    private DslArgs(Type actionType)
    {
        ActionType = actionType;
    }

    /// <summary>
    /// Gets the value of the specified parameter by name.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The value of the parameter.</returns>
    /// <exception cref="DslArgumentNotInParameterSetException">
    /// Thrown when the specified parameter name is not found in the parameter set.
    /// </exception>
    public string? this[string name]
    {
        get
        {
            if (_parameterSet is null) throw new DslArgsDoNotHaveParametersSetException();
            if (_parameterSet.TryGetValue(name, out var parameter)) return parameter.Value;

            throw new DslArgumentNotInParameterSetException(name, _parameterSet.Keys.ToArray());
        }
    }

    /// <summary>
    /// Parses the arguments and sets the values for the given DSL parameters.
    /// </summary>
    /// <param name="args">The arguments to parse.</param>
    /// <param name="parameters">The DSL parameters to set the values for.</param>
    /// <returns>A reference to the current instance of the class.</returns>
    public DslArgs From(IEnumerable<string> args, params DslParameter[] parameters)
    {
        var parameterSet = parameters.ToDictionary(p => p.Name);
        
        foreach (var arg in args)
        {
            var argParts = arg.Split(new char[]{':'}, StringSplitOptions.RemoveEmptyEntries)
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToList()
                ;

            if (argParts.Count != 2) 
                throw new DslArgumentSyntaxException(arg);

            var name = argParts[0].Trim().ToLower();

            if (!parameterSet.ContainsKey(name)) 
                throw new DslArgumentNotInParameterSetException(name, parameterSet.Keys.ToArray());
                
            var value = argParts[1].Trim().ToLower();

            parameterSet[name].TrySetValue(value);
        }
        
        var unsetRequiredParams = parameters
                .Where(p => p is RequiredDslParameter && p.Value is null)
                .ToList()
            ;

        if (unsetRequiredParams.Any())
            throw new DslRequiredParameterNotSetException(unsetRequiredParams.Select(p => p.Name).ToArray());
        
        _parameterSet = parameterSet;
        
        return this;
    }

    /// <summary>
    /// Creates an instance of DslArgs for a specified action type.
    /// </summary>
    /// <typeparam name="TAction">The type of the action.</typeparam>
    /// <returns>An instance of DslArgs.</returns>
    public static DslArgs ForAction<TAction>()
    {
        return new DslArgs(typeof(TAction));
    }
}

