namespace Truss.Results.Extensions.Fluent.SourceGenerator;

public sealed class AndExtensionGenerator : ExtensionGeneratorBase
{
    public AndExtensionGenerator(int size) : base(size)
    {
    }

    public string Generate()
    {
        return "public static void and() {}";
    }
}