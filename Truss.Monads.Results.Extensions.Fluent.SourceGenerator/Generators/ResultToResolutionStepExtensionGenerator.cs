using Truss.Monads.Results.Extensions.Fluent.SourceGenerator.MethodSets;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator.Generators;

public sealed class ResultToResolutionStepExtensionGenerator : IGenerator
{
    public string Name => "ResultToResolutionStepExtensions";
    private readonly TypingContext _typingContext;
    private readonly IMethodSet[] _methodSets;
 
    public ResultToResolutionStepExtensionGenerator(
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
            output.Add(GenerateAsyncMethod(method));
        }
 
        return string.Join("\n", output);
    }
 
    private string ReturnSignature(string returnType)
    {
        return $"ResolutionStep<{returnType}>";
    }
     
    private string GenerateMethod(Method method)
    {
        var returnSignature = method.IsAsync 
            ? $"static Task<{ReturnSignature(method.ReturnType)}>"
            : $"static {ReturnSignature(method.ReturnType)}";
        
        return $$"""
                 public {{returnSignature}} {{method.SetName}}<{{method.OperationTypes}}>(
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
        var returnSignature = method.IsAsync
            ? "return await"
            : "return";
        
        return $$"""
                 public static async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}<{{method.OperationTypes}}>(
                     this Task<Result<{{method.InTypes}}>> resultTask,
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     var result = await resultTask.ConfigureAwait(false);
                     var resolutionStep = new ResolutionStep<{{method.InTypes}}>(result);
                     {{returnSignature}} resolutionStep.{{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}({{method.MethodName}});
                 }
                 """;       
    }
}