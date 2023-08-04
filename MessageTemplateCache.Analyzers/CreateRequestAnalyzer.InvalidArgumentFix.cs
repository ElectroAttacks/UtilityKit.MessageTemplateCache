using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using MessageTemplateCache.Analyzers.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MessageTemplateCache.Analyzers;

/// <summary>
/// Provides code fixes for the 'InvalidArgumentRule' diagnostic.
/// Offers options to either remove the parameters or correct the parameter values.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InvalidArgumentFix)), Shared]
public sealed class InvalidArgumentFix : CodeFixProvider
{

    #region CodeFixProvider

    /// <summary>
    /// Gets the diagnostic IDs that this code fix provider can handle.
    /// </summary>
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get
        {
            return ImmutableArray.Create(DiagnosticRules.InvalidArgumentRule.Id);
        }
    }


    /// <summary>
    /// Gets the fix all provider for batch fixing.
    /// </summary>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for the specified context and diagnostic.
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (Diagnostic diagnostic in context.Diagnostics)
        {
            string removeTitle = DiagnosticRules.GetLocalizedString(nameof(DiagnosticMessages.InvalidArgument_CodeFixRemove)).ToString();
            string replaceTitle = DiagnosticRules.GetLocalizedString(nameof(DiagnosticMessages.InvalidArgument_CodeFixReplace)).ToString();

            // Register the "Remove the parameters" code action.
            context.RegisterCodeFix(CodeAction.Create(
                    title: removeTitle,
                    createChangedDocument: c => RemoveParametersAsync(context.Document, diagnostic, c),
                    equivalenceKey: removeTitle),
                diagnostic);

            // Register the "Correct the parameter values" code action.
            context.RegisterCodeFix(CodeAction.Create(
                    title: replaceTitle,
                    createChangedDocument: c => CorrectParameterValuesAsync(context.Document, diagnostic, c),
                    equivalenceKey: replaceTitle
                    ),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    #endregion


    /// <summary>
    /// Removes all parameters from the specified invocation.
    /// </summary>
    /// <param name="document">The document containing the invocation.</param>
    /// <param name="diagnostic">The diagnostic associated with the code fix.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The document with the updated syntax tree.</returns>
    private async Task<Document> RemoveParametersAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root?.FindNode(diagnostic.Location.SourceSpan) is not InvocationExpressionSyntax invocation)
            return document;

        // Remove all arguments from the invocation.
        InvocationExpressionSyntax newInvocation = invocation.WithArgumentList(SyntaxFactory.ArgumentList());
        SyntaxNode newRoot = root.ReplaceNode(invocation, newInvocation);

        return document.WithSyntaxRoot(newRoot);
    }

    /// <summary>
    /// Corrects the parameter values in the specified invocation.
    /// </summary>
    /// <param name="document">The document containing the invocation.</param>
    /// <param name="diagnostic">The diagnostic associated with the code fix.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The document with the updated syntax tree.</returns>
    private async Task<Document> CorrectParameterValuesAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root?.FindNode(diagnostic.Location.SourceSpan) is not InvocationExpressionSyntax invocation)
            return document;


        // Retrieve the actual values from the code.
        string callerFilePath = document.FilePath ?? "";
        string callerMemberName = invocation.FirstAncestorOrSelf<MethodDeclarationSyntax>()?.Identifier.ValueText ?? "";
        int lineNumberInMethod = invocation.GetLocation().GetLineSpan().StartLinePosition.Line + 1;

        // Create new argument list with the correct values.
        List<ArgumentSyntax> newArguments = new()
        {
            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(callerFilePath))),
            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(callerMemberName))),
            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(lineNumberInMethod))),
        };

        // Replace the existing argument list with the new one.
        InvocationExpressionSyntax newInvocation = invocation.WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(newArguments)));
        SyntaxNode newRoot = root.ReplaceNode(invocation, newInvocation);

        return document.WithSyntaxRoot(newRoot);
    }

}
