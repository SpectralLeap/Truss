using System.Collections.Generic;

namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class AndExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Then";
    protected override string ReturnResultType => $"Result<{OutTypeName}>";
    protected override string ArgumentResultType => $"Result<{InTuple}>";
    protected override string SyncGeneratorType => $"Action<{InTuple}>";
    protected override string AsyncGeneratorType => $"Func<{InTuple}, Task>";
 
    public AndExtensionGenerator(int size) : base(size)
    {
        RegisterGeneratorFunction(ResultAndFuncOfT);
        RegisterGeneratorFunction(ResultAndFuncOfResultOfT);

        RegisterGeneratorFunction(ResultAndFuncOfTaskOfT);
        RegisterGeneratorFunction(ResultAndFuncOfTaskOfResultOfT);

        RegisterGeneratorFunction(TaskOfResultAndFuncOfT);
        RegisterGeneratorFunction(TaskOfResultAndFuncOfResultOfT);

        RegisterGeneratorFunction(TaskOfResultAndFuncOfTaskOfT);
        RegisterGeneratorFunction(TaskOfResultAndFuncOfTaskOfResultOfT);

        RegisterGeneratorFunction(ResultAndFuncOfTupleAndTaskOfT);
        RegisterGeneratorFunction(ResultAndFuncOfTupleAndTaskOfResultOfT);


        RegisterGeneratorFunction(TaskOfResultAndFuncOfTupleAndOfT);
        RegisterGeneratorFunction(TaskOfResultAndFuncOfTupleAndOfResultOfT);

        RegisterGeneratorFunction(TaskOfResultAndFuncOfTupleAndTaskOfT);
        RegisterGeneratorFunction(TaskOfResultAndFuncOfTupleAndTaskOfResultOfT);
    }

    private string WithPriorValues =>
        $"""
         var {NextResultName} = {GeneratorFunctionName}(
             {PriorSuccessValues()});
         """;

    private string WithPriorValuesAsync =>
        $"""
         var {NextResultName} = await {GeneratorFunctionName}(
            {PriorSuccessValues()}).ConfigureAwait(false);
         """;

    private string WithPriorTuple =>
        $"""
         var {NextResultName} = {GeneratorFunctionName}(
             {PriorResultName}.SuccessValue);
         """;

    private string WithPriorTupleAsync =>
        $"""
         var {NextResultName} = await {GeneratorFunctionName}(
             {PriorResultName}.SuccessValue).ConfigureAwait(false);
         """;

    private string ReturnTuple =>
        $"""
         return Result.Success((
             {PriorSuccessValues()},
             {NextResultName}
         ));
         """;

    private string ReturnTupleFromResult =>
        $"""
         {IfNextResultFailed}

         return Result.Success((
             {PriorSuccessValues()},
             {NextResultName}.SuccessValue
         ));
         """;

    private string PriorSuccessValues()
    {
        if (InTypeArray.Count is 1) return $"{PriorResultName}.SuccessValue";

        var values = new List<string>();

        // Preserves indentation
        values.Add($"{PriorResultName}.SuccessValue.Item1");

        for (int i = 2; i <= InTypeArray.Count; i++)
        {
            values.Add($"    {PriorResultName}.SuccessValue.Item{i}");
        }

        return string.Join(",\n", values);
    }

    private string ResultAndFuncOfT()
    {
        return $$"""
                 public static Result<({{InTypes}}, {{OutTypeName}})> And<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTypes}}, {{OutTypeName}}> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorValues}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string ResultAndFuncOfResultOfT()
    {
        return $$"""
                 public static Result<({{InTypes}}, {{OutTypeName}})> And<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTypes}}, Result<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorValues}


                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTypes}}, {{OutTypeName}}> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorValues}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfResultOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTypes}}, Result<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorValues}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTaskOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTypes}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorValuesAsync}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTaskOfResultOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTypes}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorValuesAsync}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string ResultAndFuncOfTaskOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTypes}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorValuesAsync}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string ResultAndFuncOfTaskOfResultOfT()
    {
        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTypes}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorValuesAsync}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string ResultAndFuncOfTupleAndTaskOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTuple}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorTupleAsync}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string ResultAndFuncOfTupleAndTaskOfResultOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Result<{{InTuple}}> {{PriorResultName}},
                    Func<{{InTuple}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromResult(
                        $"""
                         {WithPriorTupleAsync}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTupleAndTaskOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTuple}}, Task<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorTupleAsync}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTupleAndTaskOfResultOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTuple}}, Task<Result<{{OutTypeName}}>>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorTupleAsync}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTupleAndOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTuple}}, {{OutTypeName}}> {{GeneratorFunctionName}}
                 )
                 {
                    
                    {{FromAsyncResult(
                        $"""
                         {WithPriorTuple}

                         {ReturnTuple}
                         """
                    )}}
                 }
                 """;
    }

    private string TaskOfResultAndFuncOfTupleAndOfResultOfT()
    {
        if (InTypeArray.Count == 1) return "";

        return $$"""
                 public static async Task<Result<({{InTypes}}, {{OutTypeName}})>> AndAsync<{{InTypes}}, {{OutTypeName}}>(
                    this Task<Result<{{InTuple}}>> {{PriorResultTaskName}},
                    Func<{{InTuple}}, Result<{{OutTypeName}}>> {{GeneratorFunctionName}}
                 )
                 {
                    {{FromAsyncResult(
                        $"""
                         {WithPriorTuple}

                         {ReturnTupleFromResult}
                         """
                    )}}
                 }
                 """;
    }
}