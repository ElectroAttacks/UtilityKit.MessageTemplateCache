using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MessageTemplateCache.Analyzers.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MessageTemplateCache.Analyzers;

/// <summary>
/// Analyzes methods with multiple MessageTemplateAttributes and checks if their identifiers are unique.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MessageTemplateAttributeAnalyzer : DiagnosticAnalyzer
{

    private const string _messageTemplateAttributeName = "MessageTemplate";


    #region DiagnosticAnalyzer

    /// <summary>
    /// Gets the supported diagnostics for this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get
        {
            return ImmutableArray.Create(DiagnosticRules.MissingIdentifierRule);
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
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
    }

    #endregion


    /// <summary>
    /// Analyzes the given method for MessageTemplateAttributes and checks their identifiers' uniqueness.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
            return;

        // Contains all MessageTemplateAttributes with an ArgumentList
        IEnumerable<AttributeSyntax> messageTemplateAttributes = methodDeclaration.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .Where(attribute => attribute.Name is IdentifierNameSyntax identifierName &&
                                identifierName.Identifier.Text == _messageTemplateAttributeName &&
                                attribute.ArgumentList is not null);

        // Skip if there is only one MessageTemplateAttribute
        if (messageTemplateAttributes.Count() <= 1)
            return;

        HashSet<string> identifiers = new();
        foreach (AttributeSyntax attribute in messageTemplateAttributes)
        {
            if (attribute.ArgumentList!.Arguments.Count == 1)
            {
                if (!identifiers.Add(""))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.MissingIdentifierRule, attribute.GetLocation()));
                }
            }

            if (attribute.ArgumentList!.Arguments.Count == 2)
            {
                string identifier = attribute.ArgumentList.Arguments[1].Expression.ToString().Trim('"');

                if (!identifiers.Add(identifier))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.MissingIdentifierRule, attribute.GetLocation()));
                }
            }
        }
    }

}
