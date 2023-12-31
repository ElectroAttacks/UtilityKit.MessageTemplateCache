﻿using System;
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
using Microsoft.CodeAnalysis.Editing;

namespace MessageTemplateCache.Analyzers;

/// <summary>
/// A code fix provider to handle the missing identifier diagnostics.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingIdentifierFix)), Shared]
public sealed class MissingIdentifierFix : CodeFixProvider
{

    #region CodeFixProvider

    /// <summary>
    /// Gets the diagnostic IDs that this code fix can handle.
    /// </summary>
    public override ImmutableArray<string> FixableDiagnosticIds
    {
        get
        {
            return ImmutableArray.Create(DiagnosticRules.MissingIdentifierRule.Id);
        }
    }


    /// <summary>
    /// Registers code fix actions for the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (Diagnostic diagnostic in context.Diagnostics)
        {
            string title = DiagnosticRules.GetLocalizedString(nameof(DiagnosticMessages.MissingIdentifier_CodeFix)).ToString();

            context.RegisterCodeFix(CodeAction.Create(
                title: title,
                createChangedDocument: c => AddUniqueIdentifierAsync(context.Document, diagnostic, c),
                equivalenceKey: title),
                diagnostic);
        }

        return Task.CompletedTask;
    }


    /// <summary>
    /// Gets the fix all provider for batch fixes.
    /// </summary>
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    #endregion


    /// <summary>
    /// Adds a unique identifier to the specified <paramref name="document"/> for the given <paramref name="diagnostic"/>.
    /// </summary>
    /// <param name="document">The document to be modified.</param>
    /// <param name="diagnostic">The diagnostic to fix.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, yielding the modified document.</returns>
    private async Task<Document> AddUniqueIdentifierAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        // Is an AttributeSyntax with an ArgumentList
        if (root?.FindNode(diagnostic.Location.SourceSpan) is not AttributeSyntax { ArgumentList: { } } attributeSyntax)
            return document;

        // Create the identifier argument
        SeparatedSyntaxList<AttributeArgumentSyntax> updatedArguments = attributeSyntax.ArgumentList.Arguments;
        AttributeArgumentSyntax identifierArgument = SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
            kind: SyntaxKind.StringLiteralExpression,
            token: SyntaxFactory.Literal(GetUniqueIdentifier())));


        // No identifier -> Default Value: string.Empty
        if (attributeSyntax.ArgumentList.Arguments.Count == 1)
        {
            // Add the identifier argument
            updatedArguments = updatedArguments.Add(identifierArgument);
        }

        // Check if the identifier is unique
        if (attributeSyntax.ArgumentList.Arguments.Count == 2)
        {
            updatedArguments = updatedArguments.Replace(updatedArguments[1], identifierArgument);
        }

        DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        editor.ReplaceNode(attributeSyntax, attributeSyntax.WithArgumentList(
            SyntaxFactory.AttributeArgumentList(updatedArguments)));

        return editor.GetChangedDocument();
    }

    /// <summary>
    /// Generates a unique identifier.
    /// </summary>
    /// <returns>A unique identifier.</returns>
    private string GetUniqueIdentifier()
    {
        int hashCode = Guid.NewGuid().GetHashCode();

        return $"0x{hashCode:X2}";
    }

}
