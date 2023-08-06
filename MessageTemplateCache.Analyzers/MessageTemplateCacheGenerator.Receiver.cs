using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MessageTemplateCache.Generators;

public partial class MessageTemplateCacheGenerator
{

    /// <summary>
    /// Syntax receiver to collect information about attributes with the name "MessageTemplate".
    /// </summary>
    private class CacheSyntaxReceiver : ISyntaxReceiver
    {

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Ignore nodes that are not attributes.
            if (syntaxNode is AttributeSyntax
                {
                    Name: IdentifierNameSyntax { Identifier.Text: Configuration._messageTemplateIdentifier },
                    ArgumentList.Arguments.Count: > 0
                } attribute)
            {

                // Get the method it decorates.
                if (GetMethodDeclaration(syntaxNode) is not MethodDeclarationSyntax method)
                    return;


                // Get the line position of the attribute.
                FileLinePositionSpan linePosition = attribute.GetLocation().GetLineSpan();

                // Add the attribute to the cache.
                AttributeInfo key = new(
                    FilePath: linePosition.Path,
                    MethodName: method.Identifier.Text);

                CacheItem value = new(
                    Template: attribute.ArgumentList!.Arguments[0].ToString(),
                    LineNumber: linePosition.StartLinePosition.Line + 1,
                    Identifier: attribute.ArgumentList.Arguments.Count > 1 ? attribute.ArgumentList.Arguments[1].ToString() : string.Empty);

                if (!_detectionCache.ContainsKey(key))
                {
                    _detectionCache.Add(key, new());
                }

                _detectionCache[key].Add(value);
            }
        }

        /// <summary>
        /// Gets the <see cref="MethodDeclarationSyntax"/> containing the provided syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <returns>The <see cref="MethodDeclarationSyntax"/> containing the provided syntax node.</returns>
        private MethodDeclarationSyntax? GetMethodDeclaration(SyntaxNode syntaxNode)
        {
            SyntaxNode? parent = syntaxNode.Parent;

            while (parent != null)
            {
                if (parent is MethodDeclarationSyntax methodSyntax)
                {
                    return methodSyntax;
                }

                parent = parent.Parent;
            }

            return null;
        }
    }
}
