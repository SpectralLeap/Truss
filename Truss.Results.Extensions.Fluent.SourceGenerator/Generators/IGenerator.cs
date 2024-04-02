namespace Truss.Results.Extensions.Fluent.SourceGenerator.Generators;

public interface IGenerator
{
    string Name { get; }
    public string Generate();
}