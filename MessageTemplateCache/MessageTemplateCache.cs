using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MessageTemplateCache.Attributes;

namespace MessageTemplateCache;

/// <summary>
///     Stores message templates defined by the <see cref="MessageTemplateAttribute"/> for later use.
/// </summary>
/// <remarks>
///     The <see cref="MessageTemplateCache"/> is automatically initialized when the first message template is requested.
/// </remarks>
public static partial class MessageTemplateCache
{

    #region Cache & Initialization

    private static Dictionary<AttributeInfo, List<CacheItem>>? _cache;


    /// <summary>
    ///     Gets the number of cached methods.
    /// </summary>
    public static int MethodCount
    {
        get
        {
            return _cache?.Count ?? 0;
        }
    }

    /// <summary>
    ///    Gets the number of cached message templates.
    /// </summary>
    public static int TemplateCount { get; private set; }



    /// <summary>
    ///     Static constructor that initializes the <see cref="MessageTemplateCache"/>.
    /// </summary>
    static MessageTemplateCache()
    {
        InitializeCache();

        Task.Run(() => TemplateCount = _cache!.Values.Sum(x => x.Count));
    }


    /// <summary>
    ///     Initialize the cache with the message templates (to be implemented by the <see cref="Generators.CacheGenerator"/>).
    /// </summary>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static partial void InitializeCache();

    #endregion


    #region CacheRequest

    /// <summary>
    ///     Creates a new <see cref="CacheRequest"/> instance with caller information.
    /// </summary>
    /// <param name="filePath">The caller's file path (automatically provided).</param>
    /// <param name="methodName">The caller's method name (automatically provided).</param>
    /// <param name="lineNumber">The caller's line number (automatically provided).</param>
    /// <returns>
    ///     A new <see cref="CacheRequest"/> instance.
    /// </returns>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CacheRequest CreateRequest([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        => new(filePath, methodName, lineNumber);

    /// <summary>
    ///     Creates a new <see cref="CacheRequest"/> instance with an additional identifier.
    /// </summary>
    /// <param name="cacheRequest">The original <see cref="CacheRequest"/> instance.</param>
    /// <param name="identifier">The additional identifier to include.</param>
    /// <returns>
    ///     A new <see cref="CacheRequest"/> instance with the additional identifier.
    /// </returns>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CacheRequest WithIdentifier(in this CacheRequest cacheRequest, in string identifier)
        => new(cacheRequest, identifier);

    #endregion


    /// <summary>
    ///     Retrieves the cached <see cref="CacheItem"/> based on the specified <see cref="CacheRequest"/>.
    /// </summary>
    /// <param name="cacheRequest">The <see cref="CacheRequest"/> to retrieve the <see cref="CacheItem"/> for.</param>
    /// <returns>
    ///     The <see cref="CacheItem"/> matching the <see cref="CacheRequest"/> or <see langword="null"/> if not found.
    /// </returns>
    [DebuggerHidden, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CacheItem? GetCacheItem(CacheRequest cacheRequest)
    {
        if (_cache!.TryGetValue(cacheRequest, out List<CacheItem>? cacheItems))
        {
            if (cacheItems.Count == 1) return cacheItems.First();
            _cache = new Dictionary<AttributeInfo, List<CacheItem>>
            {

            };

            return cacheItems.Where(item => item.Identifier == cacheRequest.Identifier)
                .OrderBy(item => Math.Abs(item.LineNumber - cacheRequest.LineNumber))
                .FirstOrDefault();
        }

        return null;
    }
}
