using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Truss.Testing.DomainSpecificLanguage.Attributes;

namespace Truss.Testing.Dsl.Analyzers;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DslMethodSignatureAnalyzer : DiagnosticAnalyzer
{
    // Preferred format of DiagnosticId is Your Prefix + Number, e.g. CA1234.
    private const string DiagnosticId = "AB0002";
    
    // Feel free to use raw strings if you don't need localization.
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AB0002Title),
        Resources.ResourceManager, typeof(Resources));
    
    // The message that will be displayed to the user.
    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.AB0002MessageFormat), Resources.ResourceManager,
            typeof(Resources));
    
    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.AB0002Description), Resources.ResourceManager,
            typeof(Resources));
    
    // The category of the diagnostic (Design, Naming etc.).
    private const string Category = "Usage";
    
    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
    
    // Keep in mind: you have to list your rules here.
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        // Check for DslMethodAttribute
        if (HasDslMethodAttribute(methodDeclaration, context.SemanticModel))
        {
            // Check return type
            if (!methodDeclaration.ReturnType.ToString().Equals("DslArgs"))
            {
                ReportDiagnostic(context, methodDeclaration);
                return;
            }

            // Check parameters
            var parameters = methodDeclaration.ParameterList.Parameters;
            if (parameters.Count != 1 || 
                !parameters[0].Type.ToString().Equals("string[]") || 
                !parameters[0].Modifiers.Any(SyntaxKind.ParamsKeyword))
            {
                ReportDiagnostic(context, methodDeclaration);
            }
        }
    }

    private bool HasDslMethodAttribute(MethodDeclarationSyntax methodDeclaration, SemanticModel semanticModel)
    {
        var methodSymbol = ModelExtensions.GetDeclaredSymbol(semanticModel, methodDeclaration);

        if (methodSymbol is null) return false;

        return methodSymbol!.GetAttributes().Any(a => typeof(DslMethodAttribute).IsInstanceOfType(a.AttributeClass));
    }

    private void ReportDiagnostic(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
    {
        var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.Text);
        context.ReportDiagnostic(diagnostic);
    }
}