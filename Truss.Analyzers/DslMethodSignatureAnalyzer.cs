using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Truss.Dsl;

namespace Truss.Analyzers;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DslMethodSignatureAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "Truss1000";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Method Signature Enforcement",
        "Method '{0}' marked with DslMethodAttribute must have the signature 'public DslArgs MethodName(params string[] args)'",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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
        var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

        if (methodSymbol is null) return false;

        return methodSymbol!.GetAttributes().Any(a => typeof(DslMethodAttribute).IsInstanceOfType(a.AttributeClass));
    }

    private void ReportDiagnostic(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
    {
        var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.Text);
        context.ReportDiagnostic(diagnostic);
    }
}
