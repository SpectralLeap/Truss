using System;
using System.Collections.Generic;
using System.Linq;

namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public abstract class ExtensionGeneratorBase
{
    protected const string OutTypeName = "TResult";
    protected const string PriorResultName = "priorResult";
    protected const string MappingFunctionName = "map";
    protected readonly string InTypes;
    protected readonly string InArgs;
    
    private const string InTypeName = "TSuccess";
    private readonly HashSet<Func<string>> _generatorFunctions = new();

   protected ExtensionGeneratorBase(int size)
    {
        List<string> inTypes = new();
        
        for (int i = 1; i <= size; i++)
        {
            inTypes.Add($"{InTypeName}{i}");
        }

        InTypes = string.Join(", ", inTypes);
        InArgs = size > 1 ? $"({InTypes})" : InTypes;
    }
   
    public string Generate()
    {
        var outputs = _generatorFunctions
            .Select(f => f());

        return string.Join("\n", outputs);
    }

    protected void RegisterGeneratorFunction(Func<string> f)
    {
        _generatorFunctions.Add(f);
    }
    
}