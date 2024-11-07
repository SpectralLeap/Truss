using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Truss.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class BaseServicesMethodAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Usage";
    private static readonly LocalizableString NonReadonlyServiceMessageFormat =
        new LocalizableResourceString(
            nameof(Resources.TRUSS0001MessageFormat),
            Resources.ResourceManager,
            typeof(Resources)
        );

    private static readonly LocalizableString NonReadonlyServiceDescription =
        new LocalizableResourceString(
            nameof(Resources.TRUSS0001Description),
            Resources.ResourceManager,
            typeof(Resources)
        );

    private static readonly DiagnosticDescriptor NonReadonlyServiceDescriptor = new(
        "TRUSS0001",
        "Non readonly service",
        NonReadonlyServiceMessageFormat,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: NonReadonlyServiceDescription
    );

    private static readonly DiagnosticDescriptor Rule2 = new(
        "TRUSS0002",
        "a",
        "{0}",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description:
        """
        Something something:

        There is a thing.
        """
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [
        NonReadonlyServiceDescriptor, Rule2
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(
            Analyze,
            SymbolKind.Field,
            SymbolKind.Property,
            SymbolKind.Method
        );
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var symbol = context.Symbol;

        if (IsNotBaseServices(symbol)) return;

        AnalyzeSymbolUsage(symbol, context);

        if (symbol is IFieldSymbol fieldSymbol)
        {
            if (!fieldSymbol.IsReadOnly)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(NonReadonlyServiceDescriptor, symbol.Locations[0], symbol.Name)
                );
            }

            context.ReportDiagnostic(
                Diagnostic.Create(Rule2, symbol.Locations[0], symbol.Name)
            );
        }
        switch (context.Symbol)
        {
            case IMethodSymbol methodSymbol:
                Analyze(methodSymbol, context);
                break;
        }
    }


    private static bool IsNotBaseServices(ISymbol symbol)
    {
        return !symbol.GetAttributes().Any(
            attr =>
                attr.AttributeClass.Name.Contains("BaseServices")
        );
    }

    private static void AnalyzeSymbolUsage(
        ISymbol symbol,
        SymbolAnalysisContext context
    )
    {
        if (symbol.IsStatic) return;

        var diagnostic = Diagnostic.Create(NonReadonlyServiceDescriptor, symbol.Locations[0]);
        context.ReportDiagnostic(diagnostic);
    }

    private static void Analyze(IMethodSymbol methodSymbol, SymbolAnalysisContext context)
    {
        foreach (var parameter in methodSymbol.Parameters)
        {
            if (parameter.Type.Name != "IServiceDependencyAdapter")
            {
                var diagnostic = Diagnostic.Create(NonReadonlyServiceDescriptor, parameter.Locations[0]);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}