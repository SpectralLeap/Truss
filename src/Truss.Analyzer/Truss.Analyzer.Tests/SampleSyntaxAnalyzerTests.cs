using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest
    <Truss.Analyzer.BaseServicesMethodAnalyzer, Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace Truss.Analyzer.Tests;

public sealed class SampleSyntaxAnalyzerTests
{
    [Fact]
    public async Task AlertsWhenBaseServicesNotStatic()
    {
        const string text = @"
using Truss.Testing.Services;
using Microsoft.Extensions.DependencyInjection;

public class SomethingClass
{
    [BaseServices]
    private IServiceCollection _services;
}
";
        var additionalReferences = new[]
        {
            MetadataReference.CreateFromFile(typeof(Testing.Services.BaseServicesAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location)
        };

        var test = new Verifier
        {
            TestCode = text,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60
        };

        test.TestState.AdditionalReferences.AddRange(additionalReferences);

        var expected = new DiagnosticResult()
            .WithLocation(4, 32)
            .WithArguments("_services");

        test.ExpectedDiagnostics.Add(expected);

        await test.RunAsync();
    }
}