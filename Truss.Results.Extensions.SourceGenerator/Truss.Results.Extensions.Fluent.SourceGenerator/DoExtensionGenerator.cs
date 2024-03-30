namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class DoExtensionGenerator : ExtensionGeneratorBase
{
    public DoExtensionGenerator(int size) : base(size)
    {
        RegisterGeneratorFunction(DoAction);
        RegisterGeneratorFunction(DoTask);
        RegisterGeneratorFunction(TaskOfResultDoAction);
        RegisterGeneratorFunction(TaskOfResultDoTask);
    }
    
    private string DoAction()
    {
        return  $$"""
                    public static Result<None> Do<{{InTypes}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Action<{{InArgs}}> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            {{MappingFunctionName}}({{PriorResultName}}.SuccessValue);
                            return Result.Success();
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }
    private string TaskOfResultDoAction()
    {
        return  $$"""
                    public static async Task<Result<None>> DoAsync<{{InTypes}}>(
                        this Task<Result<{{InArgs}}>> {{PriorResultName}}Task,
                        Action<{{InArgs}}> {{MappingFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultName}}Task.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            {{MappingFunctionName}}({{PriorResultName}}.SuccessValue);
                            return Result.Success();
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }
    
    private string TaskOfResultDoTask()
    {
        return  $$"""
                    public static async Task<Result<None>> DoAsync<{{InTypes}}>(
                        this Task<Result<{{InArgs}}>> {{PriorResultName}}Task,
                        Func<{{InArgs}}, Task> {{MappingFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultName}}Task.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false);
                            return Result.Success();
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }
        
     
    private string DoTask()
    {
        return  $$"""
                    public static async Task<Result<None>> DoAsync<{{InTypes}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Func<{{InArgs}}, Task> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false);
                            return Result.Success();
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }    
}