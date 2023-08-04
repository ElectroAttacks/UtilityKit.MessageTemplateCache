using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MessageTemplateCache.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed partial class CreateRequestAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            return ImmutableArray.Create(Descriptors.CreateRequestMustBeCalledWithoutParameters);
        }
    }

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
            GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = context.Node as InvocationExpressionSyntax;
        if (invocation != null)
        {
            var method = invocation.Expression as MemberAccessExpressionSyntax;
            if (method != null && method.Name.Identifier.ValueText == "CreateRequest" && method.Expression.ToString() == "MessageTemplateCache")
            {
                if (invocation.ArgumentList.Arguments.Any())
                {
                    var diagnostic = Diagnostic.Create(Descriptors.CreateRequestMustBeCalledWithoutParameters, invocation.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }

}
