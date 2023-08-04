using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MessageTemplateCache.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed partial class CreateRequestAnalyzer : DiagnosticAnalyzer
{

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            return ImmutableArray.Create(Descriptors.NoParameters, Descriptors.InvalidParameters);
        }
    }


    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
    }

    /// <inheritdoc/>
    private void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
    {
        // Helper method to get the string value of an expression.
        static string GetCleanedString(ExpressionSyntax expressionSyntax) => expressionSyntax.ToString()
            .Replace("\\\\", "\\")
            .Trim('"');


        // Main logic.
        if (context.Node is InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "CreateRequest" } method &&
                method.Expression.ToString() == "MessageTemplateCache")
            {
                if (invocation.ArgumentList.Arguments.Count == 0) return;

                // Check if all parameters are provided and valid.
                if (invocation.ArgumentList.Arguments.Count == 3)
                {
                    // Provided arguments
                    string callerFilePathArg = GetCleanedString(invocation.ArgumentList.Arguments[0].Expression);
                    string callerMemberNameArg = GetCleanedString(invocation.ArgumentList.Arguments[1].Expression);
                    int callerLineNumberArg = int.Parse(invocation.ArgumentList.Arguments[2].Expression.ToString());

                    // Found Values
                    string callerFilePathInCode = context.Node.SyntaxTree.FilePath;
                    string callerMemberNameInCode = context.Node.FirstAncestorOrSelf<MethodDeclarationSyntax>()?.Identifier.ValueText ?? "";
                    int lineNumberInMethod = context.Node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;

                    // Report diagnostics if the provided values are invalid.
                    if (callerFilePathArg != callerFilePathInCode || callerMemberNameArg != callerMemberNameInCode || callerLineNumberArg < lineNumberInMethod)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.InvalidParameters, invocation.GetLocation(), callerMemberNameInCode));
                    }

                    return;
                }

                // Should have no parameters.
                context.ReportDiagnostic(Diagnostic.Create(Descriptors.NoParameters, invocation.GetLocation()));
            }
        }
    }

}
