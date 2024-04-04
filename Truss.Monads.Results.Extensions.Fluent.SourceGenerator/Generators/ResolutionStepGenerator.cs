using System.Collections.Generic;
using System.Linq;
using Truss.Monads.Results.Extensions.Fluent.SourceGenerator.MethodSets;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator.Generators;

public sealed class ResolutionStepGenerator : IGenerator
{
    public string Name => "ResolutionStep";
    private readonly TypingContext _typingContext;
    private readonly IMethodSet[] _methodSets;

    public ResolutionStepGenerator(
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

                 public readonly struct {{Name}}<{{_typingContext.InTypes}}>
                 {
                    public readonly bool Failed => {{_typingContext.PriorResultName}}.Failed;
                    public readonly FailureDetails FailureDetails => {{_typingContext.PriorResultName}}.FailureDetails;
                    
                    private readonly Result<{{_typingContext.InTuple}}> {{_typingContext.PriorResultName}};
                    
                    public {{Name}}(Result<{{_typingContext.InTuple}}> fromResult)
                    {
                        {{_typingContext.PriorResultName}} = fromResult;
                    }
                    
                    {{Methods()}}
                    
                    public static implicit operator Result<{{_typingContext.InTuple}}>(
                        {{Name}}<{{_typingContext.InTypes}}> {{Name.ToLower()}})
                            => {{Name.ToLower()}}.{{_typingContext.PriorResultName}};
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
        return $"{Name}<{returnType}>";
    }
    
    private string GenerateMethod(Method method)
    {
        if (method.IsAsync) return GenerateAsyncMethod(method);
        
        return $$"""
                 public {{ReturnSignature(method.ReturnType)}} {{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}(
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     try 
                     {
                        {{method.MethodBody}}
                        return new {{ReturnSignature(method.ReturnType)}}(Result.Success({{method.ReturnBody}}));
                     }
                     catch (Exception ex)
                     {
                        return new {{ReturnSignature(method.ReturnType)}}(Result.Fail(ex));
                     }
                 }
                 """;
    }
    
    private string GenerateAsyncMethod(Method method)
    {
        return $$"""
                 public async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}(
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     try 
                     {
                        {{method.MethodBody}}
                        return new {{ReturnSignature(method.ReturnType)}}(Result.Success({{method.ReturnBody}}));
                     }
                     catch (Exception ex)
                     {
                        return new {{ReturnSignature(method.ReturnType)}}(Result.Fail(ex));
                     }
                 }
                 """;
    }

}