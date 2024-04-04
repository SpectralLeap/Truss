using Microsoft.CodeAnalysis;

namespace Truss.Monads.Results.Extensions.Fluent.Tests.SourceGenerator;

[Generator]
public sealed class TestSourceGenerator : ISourceGenerator
{
    private const int MaxSize = 7;

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to see here
    }

    public void Execute(GeneratorExecutionContext context)
    {
        for (var size = 1; size <= 7; size++)
        {
            var testGenerator = new TestGenerator(size);
            
            var source = 
                $$""""
                    using Xunit;
                    using Truss.Monads.Results;
                    using Truss.Monads.Results.Extensions.Fluent;
                    
                    namespace Results.Extensions.Fluent.Tests;
                    
                    public class Tests{{size}}
                    {
                        {{testGenerator.Generate()}}
                    }
                  """";
        
            context.AddSource($"FluentExtensionsTests{size}.g.cs", source);
        }
    }
}