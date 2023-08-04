using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MessageTemplateCache;

/// <summary>
///     Represents a request to retrieve a cached message template.
/// </summary>
public readonly struct CacheRequest
{

    /// <summary>
    ///     Gets the caller's file path.
    /// </summary>
    internal readonly string FilePath { get; }

    /// <summary>
    ///     Gets the caller's method name.
    /// </summary>
    internal readonly string MethodName { get; }

    /// <summary>
    ///     Gets the caller's line number.
    /// </summary>
    internal readonly int LineNumber { get; }

    /// <summary>
    ///     Gets the identifier for the message template (optional).
    /// </summary>
    internal readonly string Identifier { get; }



    /// <summary>
    ///     Initializes a new instance of the <see cref="CacheRequest"/> struct with the specified file path, method name, and line number.
    /// </summary>
    /// <param name="filePath">The caller's file path.</param>
    /// <param name="methodName">The caller's method name.</param>
    /// <param name="lineNumber">The caller's line number.</param>
    internal CacheRequest(in string filePath, in string methodName, in int lineNumber)
    {
        FilePath = filePath;
        MethodName = methodName;
        LineNumber = lineNumber;
        Identifier = "";
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CacheRequest"/> struct with the specified cache request and identifier.
    /// </summary>
    /// <param name="cacheRequest">The original <see cref="CacheRequest"/> instance.</param>
    /// <param name="identifier">The additional identifier to include.</param>
    internal CacheRequest(in CacheRequest cacheRequest, in string identifier) : this(cacheRequest.FilePath, cacheRequest.MethodName, cacheRequest.LineNumber)
    {
        Identifier = identifier;
    }



    /// <summary>
    ///     Retrieves the message from the cached template with the specified arguments.
    /// </summary>
    /// <param name="args">An array of objects to format the message template.</param>
    /// <returns>
    ///     The formatted message or <see cref="string.Empty"/> if the template is not found.
    /// </returns>
    /// <exception cref="FormatException">When the format of the template is invalid.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="args"/> is <see langword="null"/>.</exception>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMessage(params object[] args)
        => GetTemplate() is string template ? string.Format(template, args) : "";

    /// <summary>
    ///     Retrieves the message from the cached template with the specified arguments and format provider.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <param name="args">An array of objects to format the message template.</param>
    /// <returns>
    ///     The formatted message or <see cref="string.Empty"/> if the template is not found.
    /// </returns>
    /// <exception cref="FormatException">When the format of the template is invalid.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="args"/> is <see langword="null"/>.</exception>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMessage(in IFormatProvider formatProvider, params object[] args)
        => GetTemplate() is string template ? string.Format(formatProvider, template, args) : "";


    /// <summary>
    ///     Retrieves the cached message template.
    /// </summary>
    /// <returns>
    ///     The cached message template or <see langword="null"/> if not found.
    /// </returns>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? GetTemplate()
        => MessageTemplateCache.GetCacheItem(this) is CacheItem item ? item.MessageTemplate : null;
}
