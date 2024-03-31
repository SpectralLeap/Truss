namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class AndExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "And";
    protected override string ReturnResultType => $"Result<({InTypes}, {OutTypeName})>";
 
    public AndExtensionGenerator(int size) : base(size)
    {
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, {OutTypeName}>", 
            generatorBody: $"""
                            var {NextResultName} = {GeneratorFunctionName}(
                                {PriorSuccessValues()});
                                
                            return Result.Success((
                                {PriorSuccessValues()},
                                {NextResultName}
                            ));
                            """
        );
         
        AddSyncGenerator(
            generatorType: $"Func<{InTypes}, Result<{OutTypeName}>>", 
            generatorBody: $"""
                            var {NextResultName} = {GeneratorFunctionName}(
                                 {PriorSuccessValues()});
                                 
                            {IfNextResultFailed}

                            return Result.Success((
                                {PriorSuccessValues()},
                                {NextResultName}.SuccessValue
                            ));
                            """
        );
        
        AddAsyncGenerator(
            generatorType: $"Func<{InTypes}, Task<{OutTypeName}>>", 
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
            generatorType: $"Func<{InTypes}, Task<Result<{OutTypeName}>>>", 
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