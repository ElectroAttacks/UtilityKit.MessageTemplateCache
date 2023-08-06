using System.Diagnostics;

namespace MessageTemplateCache;

public static partial class MessageTemplateCache
{

    /// <summary>
    ///     Represents a cached message template item with associated information.
    /// </summary>
    [DebuggerDisplay("{LineNumber}::{Identifier} => {_messageTemplate}")]
    private readonly struct CacheItem
    {

        /// <summary>
        /// Gets the message template associated with the cache item.
        /// </summary>
        public string MessageTemplate { get; }

        /// <summary>
        /// Gets the identifier for the cache item (optional).
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the line number of the cache item in the source code.
        /// </summary>
        public int LineNumber { get; }



        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItem"/> struct with the specified message template, line number, and identifier.
        /// </summary>
        /// <param name="messageTemplate">The message template associated with the cache item.</param>
        /// <param name="lineNumber">The line number of the cache item in the source code.</param>
        /// <param name="identifier">An optional identifier for the cache item.</param>
        public CacheItem(in string messageTemplate, in int lineNumber, in string identifier = "")
        {
            MessageTemplate = messageTemplate;
            LineNumber = lineNumber;
            Identifier = identifier;
        }
    }
}
