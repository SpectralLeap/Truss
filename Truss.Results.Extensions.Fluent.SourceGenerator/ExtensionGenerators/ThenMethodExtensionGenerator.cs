using System.Linq;

namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class ThenMethodExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Then";
    protected override string ReturnResultType => $"Result<{OutType}>";

    public ThenMethodExtensionGenerator(int size) : base(size)
    {
        AddSyncGenerator(
            disambiguator: "AsResult",
            generatorType: $"Func<{InTypes}, {OutType}>", 
            methodBody: $"""
                         var {NextResultName} = {GeneratorFunctionName}(
                             {PriorSuccessValues()}
                         )

                         return Result.Success(
                             {NextResultName}
                         );
                         """
        );
        
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, Result<{OutType}>>", 
            methodBody: $"""
                         var {NextResultName} = {GeneratorFunctionName}(
                              {PriorSuccessValues()}
                         );

                         {IfNextResultFailed}

                         return Result.Success(
                             {NextResultName}.SuccessValue
                         );
                         """
        );
         
        AddAsyncGenerator(
            disambiguator: "AsResult",
            generatorType: $"Func<{InTypes}, Task<{OutType}>>",
            generatorBody: $"""
                            var {NextResultName} = await {GeneratorFunctionName}(
                                {PriorSuccessValues()}
                            );

                            return Result.Success(
                                {NextResultName}
                            );
                            """
        );
        
        AddAsyncGenerator(
            generatorType: $"Func<{InTypes}, Task<Result<{OutType}>>>",
            generatorBody: $"""
                            var {NextResultName} = await {GeneratorFunctionName}(
                                {PriorSuccessValues()}
                            );

                            {IfNextResultFailed}

                            return Result.Success(
                                {NextResultName}.SuccessValue
                            );
                            """
        );
    }
}