using Microsoft.CodeAnalysis;
using Truss.Monads.Results.Extensions.Fluent.SourceGenerator.Generators;
using Truss.Monads.Results.Extensions.Fluent.SourceGenerator.MethodSets;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator;

[Generator]
public sealed class ExtensionSourceGenerator : ISourceGenerator
{
    private const int MaxSize = 7;
    
    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to see here
    }

    public void Execute(GeneratorExecutionContext context)
    {
        for (var size = 1; size <= MaxSize; size++)
        {
            foreach (var generator in GetGenerators(size))
            {
                var source = generator.Generate();
                
                context.AddSource($"{generator.Name}{size}.g.cs", source);
            } 
        }
    }

    private IGenerator[] GetGenerators(int size)
    {
        var typingContext = new TypingContext(size);
        var setList = new List<IMethodSet>();
        setList.Add( new ThenMethodSet(typingContext));   
        setList.Add( new DoMethodSet(typingContext));   
        
        if (size < MaxSize) setList.Add(new AndMethodSet(typingContext));
        
        var sets = setList.ToArray();

        var generators = new List<IGenerator>();
        generators.Add(new ResolutionStepGenerator(typingContext, sets));
        generators.Add(new ResolutionStepExtensionGenerator(typingContext, sets));
        
        if (size is 1) generators.Add(new ResultToResolutionStepExtensionGenerator(typingContext, sets));
        
        return generators.ToArray();
    }

}