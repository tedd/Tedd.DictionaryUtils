using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Tedd;

public static class DictionaryListUtilsExtensions
{
    #region Public
    #region KeySelector
    public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
        ToDictionaryList(source, keySelector, null);

    public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (keySelector == null)
            throw new ArgumentNullException(nameof(keySelector));

        comparer ??= EqualityComparer<TKey>.Default;

        var capacity = 0;
        if (source is ICollection<TSource> collection)
        {
            capacity = collection.Count;
            if (capacity == 0)
                return new Dictionary<TKey, List<TSource>>(comparer);

            if (collection is TSource[] array)
                return ToDictionaryList(array, keySelector, comparer);

            if (collection is List<TSource> list)
                return ToDictionaryList(list, keySelector, comparer);
        }

        var d = new Dictionary<TKey, List<TSource>>(capacity, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TSource>();
            list!.Add(element);
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            list.Add(element);
#endif
        }

        return d;
    }
    #endregion

    #region Key and Value selector
    public static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
        ToDictionaryList(source, keySelector, elementSelector, null);

    public static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (keySelector == null)
            throw new ArgumentNullException(nameof(keySelector));

        if (elementSelector == null)
            throw new ArgumentNullException(nameof(elementSelector));

        comparer ??= EqualityComparer<TKey>.Default;

        var capacity = 0;
        if (source is ICollection<TSource> collection)
        {
            capacity = collection.Count;
            if (capacity == 0)
                return new Dictionary<TKey, List<TElement>>(comparer);

            if (collection is TSource[] array)
                return ToDictionaryList(array, keySelector, elementSelector, comparer);

            if (collection is List<TSource> list)
                return ToDictionaryList(list, keySelector, elementSelector, comparer);
        }

        var d = new Dictionary<TKey, List<TElement>>(capacity, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TElement>();
            list!.Add(elementSelector(element));
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            list.Add(elementSelector(element));
#endif
        }

        return d;
    }
    #endregion
    #endregion

    #region Private
    #region Array
    private static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TSource>>(source.Length, comparer);
        for (var i = 0; i < source.Length; i++)
        {
            var ks = keySelector(source[i]);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TSource>();
            list!.Add(source[i]);
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            list.Add(source[i]);
#endif
        }

        return d;
    }

    private static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TSource>>(source.Count, comparer);
        foreach (TSource element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TSource>();
            list!.Add(element);
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            list.Add(element);
#endif
        }

        return d;
    }
    #endregion

    #region List
    private static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(TSource[] source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TElement>>(source.Length, comparer);
        for (var i = 0; i < source.Length; i++)
        {
            var ks = keySelector(source[i]);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TElement>();
            list!.Add(elementSelector(source[i]));
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            list.Add(elementSelector(source[i]));
#endif
        }

        return d;
    }

    private static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(List<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TElement>>(source.Count, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
#if NET8_0_OR_GREATER
#pragma warning disable CS8714
            ref var list = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
#pragma warning restore CS8714
            if (!exists)
                list = new List<TElement>();
            list!.Add(elementSelector(element));
#else
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            list.Add(elementSelector(element));
#endif
        }

        return d;
    }
    #endregion
    #endregion
}
