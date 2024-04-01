using System.Collections.Generic;
using System.Linq;

namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class ResultToResolutionStepExtensionBuilder : IGenerator
{
    public string Name => "ResultToResolutionStepExtensions";
    private readonly TypingContext _typingContext;
    private readonly IMethodSet[] _methodSets;
 
    public ResultToResolutionStepExtensionBuilder(
        TypingContext typingContext,
        IMethodSet[] methodSets
    )
    {
        _typingContext = typingContext;
        _methodSets = methodSets;
    }
 
    public string Generate()
    {
        return $$"""
                 using System;
                 using System.Threading.Tasks;
                 using Truss.Results;

                 public static class {{Name}}{{_typingContext.Size}}
                 {
                    {{Methods()}}
                 }
                 """;
    }
 
    private string Methods()
    {
        var output = new List<string>();
        foreach (var method in _methodSets.SelectMany(m => m.GetMethods()))
        {
            output.Add(GenerateMethod(method));
        }
 
        return string.Join("\n", output);
    }
 
    private string ReturnSignature(string returnType)
    {
        return $"ResolutionStep<{returnType}>";
    }
     
    private string GenerateMethod(Method method)
    {
        if (method.IsAsync) return GenerateAsyncMethod(method);
        
        return $$"""
                 public static {{ReturnSignature(method.ReturnType)}} {{method.SetName}}<{{method.OperationTypes}}>(
                     this Result<{{method.InTypes}}> result,
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     var resolutionStep = new ResolutionStep<{{method.InTypes}}>(result);
                     return resolutionStep.{{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}({{method.MethodName}});
                 }
                 """;
    }

    private string GenerateAsyncMethod(Method method)
    {
        return $$"""
                 public static async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}<{{method.OperationTypes}}>(
                     this Task<Result<{{method.InTypes}}>> resultTask,
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     var result = await resultTask.ConfigureAwait(false);
                     var resolutionStep = new ResolutionStep<{{method.InTypes}}>(result);
                     return await resolutionStep.{{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}({{method.MethodName}});
                 }
                 """;       
    }
}