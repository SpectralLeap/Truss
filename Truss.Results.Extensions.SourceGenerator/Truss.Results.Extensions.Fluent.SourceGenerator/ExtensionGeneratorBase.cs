using System;
using System.Collections.Generic;
using System.Linq;

namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public abstract class ExtensionGeneratorBase
{
    protected abstract string FunctionName { get; }
    protected abstract string ArgumentResultType { get; }
    protected abstract string ReturnResultType { get; }
    protected abstract string SyncGeneratorType { get; }
    protected abstract string AsyncGeneratorType { get; }
    
    protected const string OutTypeName = "TResult";
    protected const string PriorResultTaskName = "priorResultTask";
    protected const string PriorResultName = "priorResult";
    protected const string GeneratorFunctionName = "generator";
    protected const string NextResultName = "nextResult";

    protected IReadOnlyCollection<string> InTypeArray => _inTypeArray;
    protected readonly string InTypes;
    protected readonly string InTuple;

    protected readonly string IfNextResultFailed =
        $"if({NextResultName}.Failed) return Result.Fail({NextResultName}.FailureDetails);";

    private const string InTypeName = "TSuccess";
    private readonly HashSet<Func<string>> _generatorFunctions = new();
    private readonly List<string> _inTypeArray;

    protected ExtensionGeneratorBase(int size)
    {
        _inTypeArray = new();

        for (int i = 1; i <= size; i++)
        {
            _inTypeArray.Add($"{InTypeName}{i}");
        }

        InTypes = string.Join(", ", _inTypeArray);
        InTuple = size > 1 ? $"({InTypes})" : InTypes;
    }

    public string Generate()
    {
        var outputs = _generatorFunctions
            .Select(f => f());

        return string.Join("\n\n", outputs);
    }

    protected void RegisterGeneratorFunction(Func<string> f)
    {
        _generatorFunctions.Add(f);
    }

    protected string FromResult(string execution)
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

    protected string FromAsyncResult(string execution)
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