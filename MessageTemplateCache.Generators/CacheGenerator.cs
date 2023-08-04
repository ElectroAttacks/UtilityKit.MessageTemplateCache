﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MessageTemplateCache.Generators;

/// <summary>
/// A source generator to generate a message template cache class.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed partial class CacheGenerator : ISourceGenerator
{

    /// <summary>
    /// Represents the information of an attribute used for message templates.
    /// </summary>
    private record struct AttributeInfo(string FilePath, string MethodName);

    /// <summary>
    /// Represents a cache item for a message template.
    /// </summary>
    private record struct CacheItem(string Template, int LineNumber, string Identifier);

    /// <summary>
    /// The cache used to store message template information detected by the generator.
    /// </summary>
    private static readonly Dictionary<AttributeInfo, List<CacheItem>> _detectionCache = new();


    #region ISourceGenerator

    public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new CacheSyntaxReceiver());

    public void Execute(GeneratorExecutionContext context)
    {
        StringBuilder sourceBuilder = new();

        // Add a comment to the beginning of the file to indicate that the file was generated.
        sourceBuilder.AppendLine("// <auto-generated />")
            .AppendLine();

        // Add the source code for the cache class.
        sourceBuilder.Append(CompilationUnit()
            .WithUsings(Configuration._usingDirectives)
            .WithMembers(GetClassDeclaration())
            .NormalizeWhitespace()
            .ToFullString());

        // Add a newline after the namespace declaration.
        sourceBuilder.Insert(sourceBuilder
            .ToString()
            .IndexOf("[GeneratedCode"), "\n");

        // Add a comment to the end of the file to indicate when the file was generated.
        sourceBuilder.AppendLine()
            .AppendLine()
            .AppendLine($"// Generated at: {DateTime.Now}");

        // Add the source code to the compilation.
        context.AddSource($"{Configuration._cacheClassIdentifier}.g.cs", sourceBuilder.ToString());


        // Clear the cache.
        _detectionCache.Clear();
    }

    #endregion

    /// <summary>
    /// Gets the class declaration syntax for the cache class.
    /// </summary>
    /// <returns>The class declaration syntax for the cache class.</returns>
    private SyntaxList<MemberDeclarationSyntax> GetClassDeclaration()
    {
        // Local function to get the body of the Initialize method.
        BlockSyntax GetInitializeMethodBody()
            => Block(ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    Configuration._cacheFieldIdentifier,
                    ObjectCreationExpression(Configuration._cacheType)
                        .WithInitializer(GetDictionaryInitializer())
                )
            ));


        // Main logic.
        return SingletonList<MemberDeclarationSyntax>(
            FileScopedNamespaceDeclaration(Configuration._namespaceIdentifier).WithMembers(SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(Configuration._cacheClassIdentifier)
                    .WithAttributeLists(Configuration._attributes)
                    .WithModifiers(Configuration._cacheClassModifiers)
                    .WithMembers(SingletonList<MemberDeclarationSyntax>(
                        MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Configuration._initializeMethodIdentifier)
                            .WithModifiers(Configuration._initializeModifiers)
                            .WithBody(GetInitializeMethodBody())
                    ))
            ))
        );
    }

    /// <summary>
    /// Gets the initializer expression syntax for the cache dictionary.
    /// </summary>
    /// <returns>The initializer expression syntax for the cache dictionary.</returns>
    private InitializerExpressionSyntax GetDictionaryInitializer()
    {
        // Local function to create an AttributeInfo object.
        ObjectCreationExpressionSyntax CreateAttributeInfo(AttributeInfo attributeInfo)
            => ObjectCreationExpression(Configuration._cacheKeyIdentifier)
                .WithArgumentList(ArgumentList(
                    SeparatedList<ArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetCleanedArgument(attributeInfo.FilePath)))),
                            Token(SyntaxKind.CommaToken),
                            Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetCleanedArgument(attributeInfo.MethodName))))
                        }
                    )
                ));

        // Local function to create a CacheItem object.
        ObjectCreationExpressionSyntax CreateCacheItem(CacheItem cacheItem)
            => ObjectCreationExpression(Configuration._cacheValueIdentifier)
                .WithArgumentList(ArgumentList(
                    SeparatedList<ArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetCleanedArgument(cacheItem.Template)))),
                            Token(SyntaxKind.CommaToken),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(cacheItem.LineNumber))),
                            Token(SyntaxKind.CommaToken),
                            Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetCleanedArgument(cacheItem.Identifier))))
                        }
                    )
                ));

        // Local function to clean up the argument string.
        string GetCleanedArgument(string argument) => argument.Trim('"');


        // Main logic.
        List<ExpressionSyntax> initializerExpressions = new();

        foreach (KeyValuePair<AttributeInfo, List<CacheItem>> keyValuePair in _detectionCache)
        {
            ObjectCreationExpressionSyntax attributeInfo = CreateAttributeInfo(keyValuePair.Key);

            ObjectCreationExpressionSyntax cacheItemListExpression = ObjectCreationExpression(Configuration._cacheValueSyntax)
                .WithInitializer(InitializerExpression(SyntaxKind.CollectionInitializerExpression,
                    SeparatedList<ExpressionSyntax>(keyValuePair.Value.Select(CreateCacheItem))));

            initializerExpressions.Add(InitializerExpression(SyntaxKind.ComplexElementInitializerExpression,
                SeparatedList(new ExpressionSyntax[] { attributeInfo, cacheItemListExpression })));
        }

        return InitializerExpression(SyntaxKind.CollectionInitializerExpression, SeparatedList(initializerExpressions));
    }
}
