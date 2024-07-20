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
                 #nullable enable
                 
                 using System;
                 using System.Threading.Tasks;
                 using Truss.Monads.Results;

                 public readonly struct {{Name}}<{{_typingContext.InTypes}}> 
                    : IDisposable, IAsyncDisposable
                 {
                    public readonly bool Failed => {{_typingContext.PriorResultName}}.Failed;
                    public readonly FailureDetails FailureDetails => {{_typingContext.PriorResultName}}.FailureDetails;
                    
                    private readonly Result<{{_typingContext.InTuple}}> {{_typingContext.PriorResultName}};
                    private readonly HashSet<IDisposable> _disposables;
                    private readonly HashSet<IAsyncDisposable> _asyncDisposables;
                    
                    private readonly SemaphoreSlim _disposalSemaphore = new(1, 1);
                    private readonly bool[] _disposed = [false];
                    
                    public {{Name}}(
                        Result<{{_typingContext.InTuple}}> fromResult,
                        HashSet<IDisposable>? disposables = null,
                        HashSet<IAsyncDisposable>? asyncDisposables = null
                    )
                    {
                        _disposables = disposables ?? new();
                        _asyncDisposables = asyncDisposables ?? new();
                        
                        {{_typingContext.PriorResultName}} = fromResult;
                        
                        if (fromResult.Succeeded)
                        {
                            {{StoreDisposable(_typingContext)}}
                        }
                    } 
                    
                    {{Methods()}}
                    
                    public Result<{{_typingContext.InTuple}}> AsResult() 
                    {
                         return {{_typingContext.PriorResultName}};
                    }
                    
                    public static implicit operator Result<{{_typingContext.InTuple}}>(
                        {{Name}}<{{_typingContext.InTypes}}> {{Name.ToLower()}})
                            => {{Name.ToLower()}}.AsResult();
                    
                    public void Dispose()
                    {
                        if (_disposed[0]) return;
                        
                        _disposalSemaphore.Wait();
                        
                        try
                        {
                            DisposeInternal(); 
                            
                            GC.SuppressFinalize(this);
                        }
                        finally
                        {
                            _disposalSemaphore.Release();
                        }
                    }
                   
                    public async ValueTask DisposeAsync()
                    {
                        if (_disposed[0]) return;
                        
                        await _disposalSemaphore.WaitAsync();
                        
                        try
                        {
                            await Task.WhenAll(_asyncDisposables
                                .Select(async d => await d.DisposeAsync().ConfigureAwait(false))
                            ).ConfigureAwait(false);
                            
                            DisposeInternal();
                            
                            GC.SuppressFinalize(this);
                        }
                        finally
                        {
                            _disposalSemaphore.Release();
                        }
                    }
                     
                    private void DisposeInternal()
                    {
                        foreach(var disposable in _disposables) 
                        {
                            disposable.Dispose();
                        }                       
                        
                        _disposed[0] = true;
                    }
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

    private string SuccessReturnStatement(Method method)
    {
        return $"return new {ReturnSignature(method.ReturnType)}(Result.Success({method.ReturnBody}), _disposables, _asyncDisposables);";
    }

    private string FailureReturnStatement(Method method)
    {
        return $"return new {ReturnSignature(method.ReturnType)}(Result.Fail(ex), _disposables, _asyncDisposables);";
    }

    private string IfFailedReturnStatement(Method method)
    {
        return $"if (Failed) return new {ReturnSignature(method.ReturnType)}(Result.Fail(FailureDetails), _disposables, _asyncDisposables);";
    }
    
    private string GenerateMethod(Method method)
    {
        if (method.ProducesResult) return GenerateResultMethod(method);
        if (method.IsAsync) return GenerateAsyncMethod(method);
        
        return $$"""
                 public {{ReturnSignature(method.ReturnType)}} {{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}(
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     {{IfFailedReturnStatement(method)}}
                     
                     try 
                     {
                        {{method.MethodBody}}
                        
                        {{SuccessReturnStatement(method)}}
                     }
                     catch (Exception ex)
                     {
                        {{FailureReturnStatement(method)}}
                     }
                 }
                 """;
    }

    private string GenerateResultMethod(Method method)
    {
        if (method.IsAsync) return GenerateAsyncResultMethod(method);
        
        return $$"""
                 public {{ReturnSignature(method.ReturnType)}} {{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}(
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     {{IfFailedReturnStatement(method)}}
                     
                     try
                     {
                        {{method.MethodBody}}
                        
                        if (value.Failed) return new {{ReturnSignature(method.ReturnType)}}(Result.Fail(value.FailureDetails), _disposables, _asyncDisposables);
                        
                        {{SuccessReturnStatement(method)}}
                     }
                     catch (Exception ex)
                     {
                        {{FailureReturnStatement(method)}}
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
                     {{IfFailedReturnStatement(method)}}
                     
                     try
                     {
                        {{method.MethodBody}}
                        
                        {{SuccessReturnStatement(method)}}
                     }
                     catch (Exception ex)
                     {
                        {{FailureReturnStatement(method)}}
                     }
                 }
                 """;
    }

        
    private string GenerateAsyncResultMethod(Method method)
    {
        return $$"""
                 public async Task<{{ReturnSignature(method.ReturnType)}}> {{method.SetName}}{{(method.OutType is not null ? $"<{method.OutType}>" : "")}}(
                     {{method.MethodSignature}} {{method.MethodName}}
                 )
                 {
                     {{IfFailedReturnStatement(method)}}
                     
                     try
                     {
                        {{method.MethodBody}}
                        
                        if (value.Failed) return new {{ReturnSignature(method.ReturnType)}}(Result.Fail(value.FailureDetails), _disposables, _asyncDisposables);
                        
                        {{SuccessReturnStatement(method)}}
                     }
                     catch (Exception ex)
                     {
                        {{FailureReturnStatement(method)}}
                     }
                 }
                 """;
    }

    private string StoreDisposable(TypingContext typingContext)
    {
        var lastResultObject = "var lastResultObject = fromResult.SuccessValue";

        if (typingContext.Size > 1)
        {
            lastResultObject += $".Item{_typingContext.Size}";
        }
        
        return lastResultObject + ";" +
               """
               
               if (lastResultObject is IDisposable disposable)
               {{
                   _disposables.Add(disposable);
               }}

               if (lastResultObject is IAsyncDisposable asyncDisposable)
               {
                   _asyncDisposables.Add(asyncDisposable);
               }
               """;
    }

}