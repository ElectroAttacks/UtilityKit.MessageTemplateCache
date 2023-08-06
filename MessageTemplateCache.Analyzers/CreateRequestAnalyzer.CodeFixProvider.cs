using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessageTemplateCache.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProvider)), Shared]
public class CodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get
        {
            return ImmutableArray.Create(CreateRequestAnalyzer.Descriptors.NoParameters.Id);
        }
    }

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;


    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        // Find the invocation expression node that triggered the diagnostic
        Diagnostic diagnostic = context.Diagnostics.First();
        TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

        if (root?.FindNode(diagnosticSpan) is not InvocationExpressionSyntax invocationNode) return;

        // Create a code action that will invoke the code fix
        CodeAction codeAction = CodeAction.Create("Remove explicit parameters",
            c => RemoveParametersAsync(context.Document, invocationNode, c));

        // Register the code action
        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private async Task<Document> RemoveParametersAsync(Document document, InvocationExpressionSyntax invocationNode, CancellationToken cancellationToken)
    {
        // Remove the argument list from the invocation node
        InvocationExpressionSyntax newInvocationNode = invocationNode.WithArgumentList(SyntaxFactory.ArgumentList());

        // Replace the old invocation node with the new one in the syntax tree
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root is not SyntaxNode syntaxNode)
            return document;


        SyntaxNode newRoot = syntaxNode.ReplaceNode(invocationNode, newInvocationNode);

        // Return the updated document
        return document.WithSyntaxRoot(newRoot);
    }
}