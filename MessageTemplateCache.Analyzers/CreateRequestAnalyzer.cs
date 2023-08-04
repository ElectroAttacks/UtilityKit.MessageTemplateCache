using System.Collections.Immutable;
using MessageTemplateCache.Analyzers.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MessageTemplateCache.Analyzers;

/// <summary>
/// Analyzes method invocations of the 'CreateRequest' method from the 'MessageTemplateCache' class
/// and checks the provided arguments for validity.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed partial class CreateRequestAnalyzer : DiagnosticAnalyzer
{

    private const string _cacheClassName = "MessageTemplateCache";

    private const string _cacheRequestMethodName = "CreateRequest";


    #region DiagnosticAnalyzer

    /// <summary>
    /// Gets the supported diagnostics for this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            return ImmutableArray.Create(DiagnosticRules.InvalidArgumentRule);
        }
    }


    /// <summary>
    /// Initializes the analyzer by configuring analysis and registering analysis actions.
    /// </summary>
    /// <param name="context">The context for the analysis.</param>
    public override void Initialize(AnalysisContext context)
    {
        // Configure analysis.
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        // Register analysis.
        context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
    }

    #endregion


    /// <summary>
    /// Analyzes the given method invocation and checks the provided arguments for validity.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
    {
        // Ignore method invocations without arguments.
        if (context.Node is not InvocationExpressionSyntax { ArgumentList: { } } invocation)
            return;

        // Ignore method invocations that are not on the 'MessageTemplateCache' class.
        if (invocation.Expression is not MemberAccessExpressionSyntax { Name.Identifier.ValueText: _cacheRequestMethodName } method)
            return;

        // Ignore method invocations without arguments.
        if (invocation.ArgumentList.Arguments.Count == 0)
            return;

        // Only analyze the 'CreateRequest' method.
        if (method.Expression.ToString() == _cacheClassName)
        {
            int argsCount = invocation.ArgumentList.Arguments.Count;

            // Assign variables based on the number of parameters provided.
            string callerFilePathArg = argsCount > 0
                ? GetCleanedString(invocation.ArgumentList.Arguments[0].Expression)
                : "";
            string callerMemberNameArg = argsCount > 1
                ? GetCleanedString(invocation.ArgumentList.Arguments[1].Expression)
                : "";
            int callerLineNumberArg = argsCount > 2 && int.TryParse(invocation.ArgumentList.Arguments[2].Expression.ToString(), out int lineNumber)
                ? lineNumber
                : 0;

            // Found Values
            string callerFilePathInCode = context.Node.SyntaxTree.FilePath;
            string callerMemberNameInCode = context.Node.FirstAncestorOrSelf<MethodDeclarationSyntax>()?.Identifier.ValueText ?? "";
            int lineNumberInMethod = context.Node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;

            // Report diagnostics if the provided values are invalid.
            if (callerFilePathArg != callerFilePathInCode)
                ReportDiagnostic(context, invocation.GetLocation(), "filePath", callerFilePathInCode);

            if (callerMemberNameArg != callerMemberNameInCode)
                ReportDiagnostic(context, invocation.GetLocation(), "memberName", callerMemberNameInCode);

            if (callerLineNumberArg != lineNumberInMethod)
                ReportDiagnostic(context, invocation.GetLocation(), "lineNumber", lineNumberInMethod);
        }
    }

    /// <summary>
    /// Gets the cleaned string representation of the given expression syntax by removing escape characters and trimming quotes.
    /// </summary>
    /// <param name="expressionSyntax">The expression syntax to clean.</param>
    /// <returns>The cleaned string representation.</returns>
    private static string GetCleanedString(ExpressionSyntax expressionSyntax) => expressionSyntax.ToString()
            .Replace("\\\\", "\\")
            .Trim('"');

    /// <summary>
    /// Reports the diagnostic with the provided arguments at the specified location.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    /// <param name="location">The location of the diagnostic.</param>
    /// <param name="args">The arguments for the diagnostic message.</param>
    private void ReportDiagnostic(SyntaxNodeAnalysisContext context, Location location, params object[] args)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            descriptor: DiagnosticRules.InvalidArgumentRule,
            location: location, args));
    }

}
