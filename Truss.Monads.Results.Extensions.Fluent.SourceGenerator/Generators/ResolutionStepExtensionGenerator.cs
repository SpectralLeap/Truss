using Truss.Monads.Results.Extensions.Fluent.SourceGenerator.MethodSets;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator.Generators;

public sealed class ResolutionStepExtensionGenerator : IGenerator
{
    public string Name => "ResolutionStepExtensions";
    private readonly TypingContext _typingContext;
    private readonly IMethodSet[] _methodSets;
 
    public ResolutionStepExtensionGenerator(
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
                 using Truss.Monads.Results;

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
                 public static async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}<{{method.OperationTypes}}>(
                     this Task<ResolutionStep<{{_typingContext.InTypes}}>> resolutionStepTask,
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     var resolutionStep = await resolutionStepTask.ConfigureAwait(false);
                     return resolutionStep.{{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}({{method.MethodName}});
                 }
                 """;
    }

    private string GenerateAsyncMethod(Method method)
    {
        return $$"""
                 public static async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}<{{method.OperationTypes}}>(
                     this Task<ResolutionStep<{{_typingContext.InTypes}}>> resolutionStepTask,
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     var resolutionStep = await resolutionStepTask.ConfigureAwait(false);
                     return await resolutionStep.{{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}({{method.MethodName}});
                 }
                 """;       
    }
}