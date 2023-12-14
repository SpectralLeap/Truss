using Truss.Dsl.Parameters.Exceptions;

namespace Truss.Dsl.Parameters;

public sealed class DslParameters
{
    private readonly Dictionary<string, DslParameter> _parameterSet;

    private DslParameters(Dictionary<string, DslParameter> parameterSet)
    {
        _parameterSet = parameterSet;
    }
    
    public static DslParameters From(string[] args, params DslParameter[] parameters)
    {
        var parameterSet = parameters.ToDictionary(p => p.Name);
        
        foreach (var arg in args)
        {
            var argParts = arg.Split([':'], StringSplitOptions.RemoveEmptyEntries)
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
        
        return new DslParameters(parameterSet);
    }

    public string? this[string name] => _parameterSet[name].Value;
}