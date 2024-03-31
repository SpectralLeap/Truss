namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class ThenExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Then";
    protected override string ReturnResultType => $"Result<{OutTypeName}>";
    protected override string ArgumentResultType => $"Result<{InTuple}>";
    protected override string SyncGeneratorType => $"Action<{InTuple}>";
    protected override string AsyncGeneratorType => $"Func<{InTuple}, Task>";
     
    public ThenExtensionGenerator(int size) : base(size)
    {
        RegisterGeneratorFunction(ThenFuncOfType);
        RegisterGeneratorFunction(ThenFuncOfResultOfType);
        
        RegisterGeneratorFunction(ThenFuncOfTaskOfType);
        RegisterGeneratorFunction(ThenFuncOfTaskOfResultOfType);
        
        RegisterGeneratorFunction(TaskOfResultThenFuncOfType);
        RegisterGeneratorFunction(TaskOfResultThenFuncOfResultOfType);
        
        RegisterGeneratorFunction(TaskOfResultThenFuncOfTaskOfType);
        RegisterGeneratorFunction(TaskOfResultThenFuncOfTaskOfResultOfType);
    }

    private string FuncResult => 
        $$""""
          {{FromResult(
              $"""
               return Result.Success(
                   {GeneratorFunctionName}({PriorResultName}.SuccessValue)
               );
               """
          )}}
          """";

    private string AsyncFuncResult => 
        $$""""
          {{FromResult(
              $"""
               return Result.Success(
                   await {GeneratorFunctionName}({PriorResultName}.SuccessValue)
               );
               """
          )}}
          """";

    private string FuncOfResultResult => 
        $$""""
          {{FromResult(
              $"""
               var {NextResultName} = {GeneratorFunctionName}({PriorResultName}.SuccessValue);

               {IfNextResultFailed}

               return Result.Success(
                   {NextResultName}.SuccessValue
               );
               """
          )}}
          """";

    private string AsyncFuncOfResultResult => 
        $$""""
          {{FromResult(
              $"""
                        
               var {NextResultName} = await {GeneratorFunctionName}({PriorResultName}.SuccessValue);

               {IfNextResultFailed}

               return Result.Success(
                   {NextResultName}.SuccessValue
               );
               """
          )}}
          """";

    private string ThenFuncOfType()
    {
        return $$""""
                    public static Result<{{OutTypeName}}> Then<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InTuple}}> {{PriorResultName}},
                        Func<{{InTypes}}, {{OutTypeName}}> {{GeneratorFunctionName}}
                     )
                    {
                        {{FuncResult}}
                    }
                 """";
    }

    private string ThenFuncOfResultOfType()
    {
        return $$""""
                     public static Result<{{OutTypeName}}> Then<{{InTypes}}, {{OutTypeName}}>(
                         this Result<{{InTuple}}> {{PriorResultName}},
                         Func<{{InTypes}}, Result<{{OutTypeName}}>> {{GeneratorFunctionName}}
                     )
                     {
                         {{FuncOfResultResult}}
                     }
                 """";
    }

    private string TaskOfResultThenFuncOfType()
    {
        return $$""""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                        Func<{{InTypes}}, {{OutTypeName}}> {{GeneratorFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultTaskName}}.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        {{FuncResult}}
                    }
                 """";
    }
    
    private string TaskOfResultThenFuncOfResultOfType()
    {
        return $$""""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                        Func<{{InTypes}}, Result<{{OutTypeName}}>> {{GeneratorFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultTaskName}}.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        {{FuncOfResultResult}}
                    }
                 """";
    }

    private string TaskOfResultThenFuncOfTaskOfType()
    {
        return $$""""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                        Func<{{InTypes}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                    )
                    {
                        var {{PriorResultName}} = await {{PriorResultTaskName}}.ConfigureAwait(false);
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        {{AsyncFuncResult}}
                    }
                 """";
    }

    private string TaskOfResultThenFuncOfTaskOfResultOfType()
    {
        return $$""""
                     public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                         this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                         Func<{{InTypes}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                     )
                     {
                         var {{PriorResultName}} = await {{PriorResultTaskName}}.ConfigureAwait(false);
                         if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                         
                         {{AsyncFuncOfResultResult}}
                     }
                 """";
    }


    private string ThenFuncOfTaskOfType()
    {
        return $$""""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InTuple}}> {{PriorResultName}},
                        Func<{{InTypes}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        {{AsyncFuncResult}}
                    }
                 """";
    }

    private string ThenFuncOfTaskOfResultOfType()
    {
        return $$""""
                    public static async Task<Result<{{OutTypeName}}>> ThenAsync<{{InTypes}}, {{OutTypeName}}>(
                        this Result<{{InTuple}}> {{PriorResultName}},
                        Func<{{InTypes}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                    )
                    {
                        if ({{PriorResultName}}.Failed) return Result.Fail({{PriorResultName}}.FailureDetails);
                        
                        {{AsyncFuncOfResultResult}}
                    }
                 """";
    }
}