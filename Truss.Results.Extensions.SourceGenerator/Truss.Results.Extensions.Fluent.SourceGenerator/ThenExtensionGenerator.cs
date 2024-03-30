namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class ThenExtensionGenerator : ExtensionGeneratorBase
{
    public ThenExtensionGenerator(int size) : base(size)
    {
        RegisterGeneratorFunction(ThenFuncOfType);
        RegisterGeneratorFunction(ThenFuncOfResultOfType);
        RegisterGeneratorFunction(ThenTaskOfType);
        RegisterGeneratorFunction(ThenFuncOfTaskOfResult);
        RegisterGeneratorFunction(TaskOfResultThenFuncOfType);
        RegisterGeneratorFunction(TaskOfResultThenTaskOfType);
        RegisterGeneratorFunction(TaskOfResultThenFuncOfTaskOfResult);
    }
     
    private string ThenFuncOfType()
    {
        return  $$"""
                    public static Result<{{OutTypeName}}> Then<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Func<{{InArgs}}, {{OutTypeName}}> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return {{MappingFunctionName}}({{PriorResultName}}.SuccessValue);
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }
        
    private string ThenFuncOfResultOfType()
    {
        return  $$"""
                    public static Result<{{OutTypeName}}> Then<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Func<{{InArgs}}, Result<{{OutTypeName}}>> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return {{MappingFunctionName}}({{PriorResultName}}.SuccessValue);
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

    private string TaskOfResultThenFuncOfType()
    {
        return  $$"""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InArgs}}>> {{PriorResultName}}Task,
                        Func<{{InArgs}}, {{OutTypeName}}> {{MappingFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultName}}Task.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return Result.Success(
                                {{MappingFunctionName}}({{PriorResultName}}.SuccessValue)
                            );
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

    private string TaskOfResultThenTaskOfType()
    {
        return  $$"""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InArgs}}>> {{PriorResultName}}Task,
                        Func<{{InArgs}}, Task<{{OutTypeName}}>> {{MappingFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultName}}Task.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return Result.Success(
                                await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false)
                            );
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

    private string TaskOfResultThenFuncOfTaskOfResult()
    {
        return  $$"""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InArgs}}>> {{PriorResultName}}Task,
                        Func<{{InArgs}}, Task<Result<{{OutTypeName}}>>> {{MappingFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultName}}Task.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

    
    private string ThenTaskOfType()
    {
        return  $$"""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Func<{{InArgs}}, Task<{{OutTypeName}}>> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return Result.Success(
                                await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false)
                            );
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

    private string ThenFuncOfTaskOfResult()
    {
        return  $$"""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InArgs}}> {{PriorResultName}},
                        Func<{{InArgs}}, Task<Result<{{OutTypeName}}>>> {{MappingFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        try
                        {
                            return await {{MappingFunctionName}}({{PriorResultName}}.SuccessValue).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            return Result.Fail(ex);
                        }
                    }
                  """;
    }

  
}