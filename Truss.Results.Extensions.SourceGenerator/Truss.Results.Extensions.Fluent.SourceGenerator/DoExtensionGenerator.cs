namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class DoExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Do";
    protected override string ReturnResultType => $"Result<{InTuple}>";
    protected override string ArgumentResultType => $"Result<{InTuple}>";
    protected override string SyncGeneratorType => $"Action<{InTuple}>";
    protected override string AsyncGeneratorType => $"Func<{InTuple}, Task>";
    
    public DoExtensionGenerator(int size) : base(size)
    {
        RegisterGeneratorFunction(DoAction);
        RegisterGeneratorFunction(DoTask);
        RegisterGeneratorFunction(TaskOfResultDoAction);
        RegisterGeneratorFunction(TaskOfResultDoTask);
    }

    private string DoAction()
    {
        return $$"""
                   public static Result<{{InTuple}}> Do<{{InTypes}}>(
                       this Result<{{InTuple}}> {{PriorResultName}},
                       Action<{{InTuple}}> {{GeneratorFunctionName}}
                   )
                   {
                       {{FromResult(
                           $"""
                            {GeneratorFunctionName}({PriorResultName}.SuccessValue);
                            return Result.Success();
                            """
                       )}}
                   }
                 """;
    }
    private string TaskOfResultDoAction()
    {
        return $$"""
                   public static async Task<Result<{{InTuple}}>> DoAsync<{{InTypes}}>(
                       this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                       Action<{{InTuple}}> {{GeneratorFunctionName}}
                   )
                   {
                       {{FromAsyncResult(
                           $"""
                            {GeneratorFunctionName}({PriorResultName}.SuccessValue);
                            return Result.Success();
                            """
                       )}}
                   }
                 """;
    }

    private string TaskOfResultDoTask()
    {
        return $$"""
                   public static async Task<Result<{{InTuple}}>> DoAsync<{{InTypes}}>(
                       this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                       Func<{{InTuple}}, Task> {{GeneratorFunctionName}}
                   )
                   {
                       {{FromAsyncResult(
                           $"""
                            await {GeneratorFunctionName}({PriorResultName}.SuccessValue).ConfigureAwait(false);
                            return Result.Success();
                            """)}}
                   }
                 """;
    }

    private string DoTask()
    {
        return $$"""
                   public static async Task<Result<{{InTuple}}>> DoAsync<{{InTypes}}>(
                       this Result<{{InTuple}}> {{PriorResultName}},
                       Func<{{InTuple}}, Task> {{GeneratorFunctionName}}
                   )
                   {
                       {{FromResult(
                           $"""
                            await {GeneratorFunctionName}({PriorResultName}.SuccessValue).ConfigureAwait(false);
                            return Result.Success();
                            """
                       )}}
                   }
                 """;
    }
}