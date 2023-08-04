using System;
using System.Diagnostics.CodeAnalysis;

namespace MessageTemplateCache.Attributes;

/// <summary>
///     Defines a message template for a method which will be cached in the <see cref="MessageTemplateCache"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class MessageTemplateAttribute : Attribute
{

    /// <summary>
    ///     Gets the message template associated with the method.
    /// </summary>
    public string MessageTemplate { get; }

    /// <summary>
    ///     Gets the identifier for the message template.
    /// </summary>
    public string Identifier { get; }


    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageTemplateAttribute"/> class with the specified <paramref name="messageTemplate"/> and <paramref name="identifier"/>.
    /// </summary>
    /// <param name="messageTemplate">The message template to associate with the method.</param>
    /// <param name="identifier">An optional identifier for the message template.</param>
    public MessageTemplateAttribute([StringSyntax("CompositeFormat")] string messageTemplate, string identifier = "")
    {
        MessageTemplate = messageTemplate;
        Identifier = identifier;
    }
}
