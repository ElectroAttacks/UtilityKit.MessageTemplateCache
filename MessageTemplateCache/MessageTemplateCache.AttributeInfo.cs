using System.Diagnostics;

namespace MessageTemplateCache;

public static partial class MessageTemplateCache
{

    /// <summary>
    ///     Represents information about the caller's file path and method name.
    /// </summary>
    [DebuggerDisplay("{_filePath}::{_methodName}")]
    private readonly struct AttributeInfo
    {

        /// <summary>
        ///     Gets the caller's file path.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        ///     Gets the caller's method name.
        /// </summary>
        public string MethodName { get; }



        /// <summary>
        ///     Initializes a new instance of the <see cref="AttributeInfo"/> struct with the specified file path and method name.
        /// </summary>
        /// <param name="filePath">The caller's file path.</param>
        /// <param name="methodName">The caller's method name.</param>
        public AttributeInfo(in string filePath, in string methodName)
        {
            FilePath = filePath;
            MethodName = methodName;
        }


        /// <summary>
        ///     Implicitly converts a <see cref="CacheRequest"/> instance to an <see cref="AttributeInfo"/> instance.
        /// </summary>
        /// <param name="cacheRequest">The <see cref="CacheRequest"/> instance to convert.</param>
        /// <returns>
        ///     An <see cref="AttributeInfo"/> instance representing the file path and method name from the <see cref="CacheRequest"/>.
        /// </returns>
        public static implicit operator AttributeInfo(in CacheRequest cacheRequest)
            => new(cacheRequest.FilePath, cacheRequest.MethodName);
    }
}
