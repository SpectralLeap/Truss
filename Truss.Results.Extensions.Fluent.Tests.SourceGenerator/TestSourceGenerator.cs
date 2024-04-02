using Microsoft.CodeAnalysis;

namespace Truss.Results.Extensions.Fluent.Tests.SourceGenerator;

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
        var source = 
            $$""""
              using Xunit;
              
              public class Tests
              {
                  [Fact]
                  public void DoesThing()
                  {
                      Assert.True(true);
                  }
              }
            """";
        
        context.AddSource("x.g.cs", source);
    }

}