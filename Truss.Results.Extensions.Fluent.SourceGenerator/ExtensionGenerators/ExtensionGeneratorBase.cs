using System.Collections.Generic;

namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public abstract class ExtensionGeneratorBase
{
    private readonly int _size;
    protected const string IfNextResultFailed = $"if({NextResultName}.Failed) return Result.Fail({NextResultName}.FailureDetails);";
    private const string PriorResultTaskName = "priorResultTask";
    private const string PriorResultName = "priorResult";
    protected const string OutTypeName = "TResult";
    protected const string GeneratorFunctionName = "generator";
    protected const string NextResultName = "nextResult";
    protected abstract string FunctionName { get; }
    protected abstract string ReturnResultType { get; }
    protected readonly string InTypes;
    protected readonly string InTuple;
  
    private readonly Dictionary<string, string> _syncGenerators = new();
    private readonly Dictionary<string, string> _asyncGenerators = new();
    private readonly Dictionary<string, string> _disambiguators = new();
    private readonly Dictionary<string, string> _typeParameterOverrides = new();

    private string ResultArgument => $"Result<{InTuple}>";
    private const string InTypeName = "TSuccess";

    protected ExtensionGeneratorBase(int size)
    {
        var inTypeArray = new List<string>();

        for (int i = 1; i <= size; i++)
        {
            inTypeArray.Add($"{InTypeName}{i}");
        }

        InTypes = string.Join(", ", inTypeArray);
        InTuple = size > 1 ? $"({InTypes})" : InTypes;
        _size = size;
    }

    public string Generate()
    {
        var outputs = new List<string>();
        foreach (var generator in _syncGenerators)
        {
            outputs.Add(FromSyncToSyncMethod(generator.Key, generator.Value));
            outputs.Add(FromAsyncMethod(generator.Key, generator.Value));
        }

        foreach (var generator in _asyncGenerators)
        {
            outputs.Add(FromSyncToAsyncSignature(generator.Key, generator.Value));
            outputs.Add(FromAsyncMethod(generator.Key, generator.Value));
        }

        return string.Join("\n\n", outputs);
    }

    protected void AddSyncGenerator(
        string generatorType,
        string generatorBody,
        string? disambiguator = null,
        string? typeParameterOverride = null
    )
    {
        _syncGenerators.Add(generatorType, generatorBody);
        
        if (disambiguator is not null) _disambiguators.Add(generatorType, disambiguator);
        if (typeParameterOverride is not null) _typeParameterOverrides.Add(generatorType, typeParameterOverride);
    }
    
    protected void AddAsyncGenerator(
        string generatorType,
        string generatorBody,
        string? disambiguator = null,
        string? typeParameterOverride = null
    )
    {
        _asyncGenerators.Add(generatorType, generatorBody);
 
        if (disambiguator is not null) _disambiguators.Add(generatorType, disambiguator);
        if (typeParameterOverride is not null) _typeParameterOverrides.Add(generatorType, typeParameterOverride);
    }
    
    protected string PriorSuccessValues()
    {
        if (_size is 1) return $"{PriorResultName}.SuccessValue";
     
        var values = new List<string> {
            $"{PriorResultName}.SuccessValue.Item1"
        };

        for (int i = 2; i <= _size; i++)
        {
            values.Add($"    {PriorResultName}.SuccessValue.Item{i}");
        }
     
        return string.Join(",\n", values);
    }

    private string GetFunctionName(string generatorType)
    {
        _disambiguators.TryGetValue(generatorType, out var disambiguator);

        return $"{FunctionName}{disambiguator ?? ""}";
    }

    private string GetTypeParameters(string generatorType)
    {
        _typeParameterOverrides.TryGetValue(generatorType, out var parameterOverride);

        return parameterOverride ?? $"{InTypes}, {OutTypeName}";
    }
    
    private string FromSyncToSyncMethod(string generatorType, string generatorBody)
    {
        return 
            $$"""
              public static {{ReturnResultType}} {{GetFunctionName(generatorType)}} <{{GetTypeParameters(generatorType)}}>(
                 this {{ResultArgument}} {{PriorResultName}},
                 {{generatorType}} {{GeneratorFunctionName}})
              {
                 {{FromResult(generatorBody)}}
              }
              """;
    }

    private string FromSyncToAsyncSignature(string generatorType, string generatorBody)
    {
        return 
            $$"""
              public async static Task<{{ReturnResultType}}> {{GetFunctionName(generatorType)}}Async <{{GetTypeParameters(generatorType)}}>(
                 this {{ResultArgument}} {{PriorResultName}},
                 {{generatorType}} {{GeneratorFunctionName}})
              {
                 {{FromResult(generatorBody)}}
              }
              """;
    }
    
    private string FromAsyncMethod(string generatorType, string generatorBody)
    {
        return 
            $$"""
              public async static Task<{{ReturnResultType}}> {{GetFunctionName(generatorType)}}Async <{{GetTypeParameters(generatorType)}}>(
                 this Task<{{ResultArgument}}> {{PriorResultTaskName}},
                 {{generatorType}} {{GeneratorFunctionName}})
              {
                 {{FromAsyncResult(generatorBody)}}
              }
              """;

    }


    private string FromResult(string execution)
    {
        return $$"""
                 try
                 {
                      if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                      
                      {{execution}}
                 }
                 catch (Exception ex)
                 {
                     return Result.Fail(ex);
                 }
                 """;
    }

    private string FromAsyncResult(string execution)
    {
        return $$"""
                 try
                 {
                      var {{PriorResultName}} = await {{PriorResultTaskName}}.ConfigureAwait(false);
                      if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                      
                      {{execution}}
                 }
                 catch (Exception ex)
                 {
                     return Result.Fail(ex);
                 }
                 """;
    }
}