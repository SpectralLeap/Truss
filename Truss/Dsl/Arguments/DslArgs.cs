using Truss.Dsl.Parameters;

namespace Truss.Dsl.Arguments;


public sealed class DslArgumentSyntaxException(string arg) 
    : Exception($"Argument \"{arg}\" is not in format \"name: value\"");

public sealed class DslArgumentNotInParameterSetException(string name, string[] keys) 
    : Exception($"Argument \"{name}\" is not in the parameter set. Available arguments are [{string.Join(", ", keys)}]");


public sealed class DslArgs
{
    internal readonly Type ActionType;
    private Dictionary<string, DslParameter> _parameterSet;

    private DslArgs(Type actionType)
    {
        ActionType = actionType;
    }
 
    public string? this[string name]
    {
        get
        {
            if (_parameterSet.TryGetValue(name, out var parameter)) return parameter.Value;

            throw new DslArgumentNotInParameterSetException(name, _parameterSet.Keys.ToArray());
        }
    }
    
    public DslArgs From(string[] args, params DslParameter[] parameters)
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

    public static DslArgs ForAction<TAction>()
    {
        return new DslArgs(typeof(TAction));
    }
   
}