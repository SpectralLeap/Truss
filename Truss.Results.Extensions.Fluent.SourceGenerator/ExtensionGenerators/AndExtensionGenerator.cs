using System;

namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class AndExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "And";
    protected override string ReturnResultType => $"Result<({InTypes}, {OutType})>";
 
    public AndExtensionGenerator(int size) : base(size)
    {
        AddSyncGenerator(
            disambiguator: "AsResult",
            generatorType: $"Func<{InTypes}, {OutType}>", 
            methodBody: $"var {NextResultName} = {GeneratorFunctionName}({PriorSuccessValues()});",
            returnBody: $"({PriorSuccessValues()},{NextResultName})"
        );
         
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, Result<{OutType}>>", 
            methodBody: $"""
                            var {NextResultName} = {GeneratorFunctionName}(
                                 {PriorSuccessValues()});
                                 
                            {IfNextResultFailed}
                            """,
            returnBody: $"({PriorSuccessValues()},{NextResultName}).SuccessValue"
        );
        
        AddAsyncGenerator(
            disambiguator: "AsResult",
            generatorType: $"Func<{InTypes}, Task<{OutType}>>", 
            generatorBody: $"""
                            var {NextResultName} = await {GeneratorFunctionName}(
                                    {PriorSuccessValues()}).ConfigureAwait(false);
                                 
                            return Result.Success((
                                {PriorSuccessValues()},
                                {NextResultName}
                            ));
                            """
        );
        
        AddAsyncGenerator(
            generatorType: $"Func<{InTypes}, Task<Result<{OutType}>>>", 
            generatorBody: $"""
                            var {NextResultName} = await {GeneratorFunctionName}(
                                 {PriorSuccessValues()}).ConfigureAwait(false);
                                 
                            {IfNextResultFailed}

                            return Result.Success((
                                {PriorSuccessValues()},
                                {NextResultName}.SuccessValue
                            ));
                            """
        );
    }
}