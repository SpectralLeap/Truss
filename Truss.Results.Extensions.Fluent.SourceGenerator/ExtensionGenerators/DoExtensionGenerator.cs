namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class DoExtensionGenerator : ExtensionGeneratorBase
{
    protected override string FunctionName => "Do";
    protected override string ReturnResultType => $"Result<{InTuple}>";
    
    public DoExtensionGenerator(int size) : base(size)
    {
        AddSyncGenerator(
            generatorType: $"Action<{InTypes}>",
            generatorBody: $"""
                            {GeneratorFunctionName}({PriorSuccessValues()});
                            return Result.Success();
                            """,
            typeParameterOverride: $"{InTypes}"
        );
        
        AddAsyncGenerator(
            disambiguator: "Task",
            generatorType: $"Func<{InTypes}, Task>",
            generatorBody:  $"""
                             await {GeneratorFunctionName}({PriorSuccessValues()});
                             return Result.Success();
                             """,
            typeParameterOverride: $"{InTypes}"
        );
    }
}