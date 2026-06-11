using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Tedd;

public static class DictionaryUtilsExtensions
{
    #region Public
    #region KeySelector
    public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
       ToDictionarySafe(source, keySelector, null);

    public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    {
        if (source == null)
            throw new ArgumentException(nameof(source));

        if (keySelector == null)
            throw new ArgumentException(nameof(keySelector));

        var capacity = 0;
        if (source is ICollection<TSource> collection)
        {
            capacity = collection.Count;
            if (capacity == 0)
                return new Dictionary<TKey, TSource>(comparer);

            if (collection is TSource[] array)
                return ToDictionarySafe(array, keySelector, comparer);

            if (collection is List<TSource> list)
                return ToDictionarySafe(list, keySelector, comparer);
        }

        var d = new Dictionary<TKey, TSource>(capacity, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = element;
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, element);
#endif
        }

        return d;
    }
    #endregion
    #endregion

    #region Key and Value selector
    public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
        ToDictionarySafe(source, keySelector, elementSelector, null);

    public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
    {
        if (source == null)
            throw new ArgumentException(nameof(source));

        if (keySelector == null)
            throw new ArgumentException(nameof(keySelector));

        if (elementSelector == null)
            throw new ArgumentException(nameof(elementSelector));

        var capacity = 0;
        if (source is ICollection<TSource> collection)
        {
            capacity = collection.Count;
            if (capacity == 0)
                return new Dictionary<TKey, TElement>(comparer);

            if (collection is TSource[] array)
                return ToDictionarySafe(array, keySelector, elementSelector, comparer);

            if (collection is List<TSource> list)
                return ToDictionarySafe(list, keySelector, elementSelector, comparer);
        }

        var d = new Dictionary<TKey, TElement>(capacity, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = elementSelector(element);
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, elementSelector(element));
#endif
        }

        return d;
    }
    #endregion

    #region Private
    #region Array
    private static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, TSource>(source.Length, comparer);
        for (var i = 0; i < source.Length; i++)
        {
            var ks = keySelector(source[i]);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = source[i];
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, source[i]);
#endif
        }

        return d;
    }

    private static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, TSource>(source.Count, comparer);
        foreach (TSource element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = element;
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, element);
#endif
        }

        return d;
    }
    #endregion

    #region List
    private static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(TSource[] source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, TElement>(source.Length, comparer);
        for (var i = 0; i < source.Length; i++)
        {
            var ks = keySelector(source[i]);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = elementSelector(source[i]);
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, elementSelector(source[i]));
#endif
        }

        return d;
    }

    private static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(List<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, TElement>(source.Count, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = elementSelector(element);
#else
            if (!d.ContainsKey(ks))
                d.Add(ks, elementSelector(element));
#endif
        }

        return d;
    }
    #endregion
    #endregion
}