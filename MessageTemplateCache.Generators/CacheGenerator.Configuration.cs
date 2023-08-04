using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MessageTemplateCache.Generators;

public partial class CacheGenerator
{

    /// <summary>
    /// Contains configuration settings for the message template cache.
    /// </summary>
    private static class Configuration
    {

        private static readonly Assembly _assembly = typeof(CacheGenerator).Assembly;


        #region Cache Class

        /// <summary>
        /// The identifier for the cache class.
        /// </summary>
        public static readonly SyntaxToken _cacheClassIdentifier = Identifier("MessageTemplateCache");

        /// <summary>
        /// The identifier for the initialize method of the cache class.
        /// </summary>
        public static readonly SyntaxToken _initializeMethodIdentifier = Identifier("InitializeCache");

        /// <summary>
        /// The namespace identifier for the cache class.
        /// </summary>
        public static readonly IdentifierNameSyntax _namespaceIdentifier = IdentifierName("MessageTemplateCache");

        /// <summary>
        /// The modifiers for the cache class.
        /// </summary>
        public static readonly SyntaxTokenList _cacheClassModifiers = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword));

        /// <summary>
        /// The modifiers for the initialize method of the cache class.
        /// </summary>
        public static readonly SyntaxTokenList _initializeModifiers = TokenList(Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword));

        /// <summary>
        /// The using directives for the cache class.
        /// </summary>
        public static readonly SyntaxList<UsingDirectiveSyntax> _usingDirectives = List(new UsingDirectiveSyntax[]
        {
            // Using System.Collections.Generic
            UsingDirective(
                QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Collections")),
                    IdentifierName("Generic"))),

            // Using System.CodeDom.Compiler
            UsingDirective(
                    QualifiedName(
                        QualifiedName(IdentifierName("System"), IdentifierName("CodeDom")),
                        IdentifierName("Compiler")))
        });


        /// <summary>
        /// The attributes for the cache class.
        /// </summary>
        public static SyntaxList<AttributeListSyntax> _attributes = SingletonList(
            AttributeList(
                SingletonSeparatedList(
                    Attribute(IdentifierName("GeneratedCode"))
                        .WithArgumentList(AttributeArgumentList(SeparatedList<AttributeArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(_assembly.GetName().Name))),
                                Token(SyntaxKind.CommaToken),
                                AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(_assembly.GetName().Version.ToString())))
                            }
                        )))
                )
            )
        );

        #endregion


        #region Cache Structure

        /// <summary>
        /// The identifier for the cache field.
        /// </summary>
        public static readonly IdentifierNameSyntax _cacheFieldIdentifier = IdentifierName("_cache");

        /// <summary>
        /// The identifier for the cache key.
        /// </summary>
        public static readonly IdentifierNameSyntax _cacheKeyIdentifier = IdentifierName(nameof(AttributeInfo));

        /// <summary>
        /// The identifier for the cache value.
        /// </summary>
        public static readonly IdentifierNameSyntax _cacheValueIdentifier = IdentifierName(nameof(CacheItem));

        /// <summary>
        /// The syntax for the cache value type.
        /// </summary>
        public static readonly TypeSyntax _cacheValueSyntax = GenericName(Identifier("List")).WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(_cacheValueIdentifier)));

        /// <summary>
        /// The type syntax for the cache.
        /// </summary>
        public static readonly TypeSyntax _cacheType = GenericName(Identifier("Dictionary")).WithTypeArgumentList(TypeArgumentList(SeparatedList<TypeSyntax>(new SyntaxNodeOrToken[]
        {
            _cacheKeyIdentifier,
            Token(SyntaxKind.CommaToken),
            _cacheValueSyntax
        })));

        #endregion
    }
}
