using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Tedd;

public static class AddExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value using the provided factory if the key does not exist.
    /// </summary>
    /// <remarks>
    /// Time Complexity: O(1) amortized.
    /// Space Complexity: O(1) beyond the allocation of the value itself.
    /// </remarks>
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TValue> valueFactory)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        if (valueFactory is null)
            throw new ArgumentNullException(nameof(valueFactory));

#if NET8_0_OR_GREATER
#pragma warning disable CS8714 // Nullability constraint mismatch
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dic, key, out bool exists);
#pragma warning restore CS8714
        if (!exists)
            val = valueFactory();
#pragma warning disable CS8603 // Possible null reference return
        return val;
#pragma warning restore CS8603 // Possible null reference return
#else
        // Return if we have it
        if (dic.TryGetValue(key, out var value))
            return value;

        // Generate it
        value = valueFactory();
        dic.Add(key, value);

        return value;
#endif
    }
}
