namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class ThenExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Then";
    protected override string ReturnResultType => $"Result<{OutTypeName}>";
     
    public ThenExtensionGenerator(int size) : base(size)
    {
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, {OutTypeName}>", 
            generatorBody: $"""
                            return Result.Success(
                                {GeneratorFunctionName}(
                                    {PriorSuccessValues()}
                                )
                            );
                            """
        );
        
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, Result<{OutTypeName}>>", 
            generatorBody: $"""
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
            disambiguator: "Task",
            generatorType: $"Func<{InTypes}, Task<{OutTypeName}>>",
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
            disambiguator: "Task",
            generatorType: $"Func<{InTypes}, Task<Result<{OutTypeName}>>>",
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