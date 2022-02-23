using System;
using System.Collections.Generic;

namespace Tedd;

public static class DictionaryListUtilsExtensions
{
    #region Public
    #region KeySelector
    public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
        ToDictionaryList(source, keySelector, null);

    public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(element);
        }

        return d;
    }
    #endregion
    #endregion

    #region Key and Value selector
    public static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
        ToDictionaryList(source, keySelector, elementSelector, null);

    public static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
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
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(elementSelector(element));
        }

        return d;
    }
    #endregion

    #region Private
    #region Array
    private static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TSource>>(source.Length, comparer);
        for (var i = 0; i < source.Length; i++)
        {
            var ks = keySelector(source[i]);
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(source[i]);
        }

        return d;
    }

    private static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TSource>>(source.Count, comparer);
        foreach (TSource element in source)
        {
            var ks = keySelector(element);
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TSource>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(element);
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
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(elementSelector(source[i]));
        }

        return d;
    }

    private static Dictionary<TKey, List<TElement>> ToDictionaryList<TSource, TKey, TElement>(List<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        var d = new Dictionary<TKey, List<TElement>>(source.Count, comparer);
        foreach (var element in source)
        {
            var ks = keySelector(element);
            if (!d.TryGetValue(ks, out var list))
            {
                list = new List<TElement>();
                d.Add(ks, list);
            }
            // TODO: Should we avoid dupes?
            list.Add(elementSelector(element));

        }

        return d;
    }
    #endregion
    #endregion
}
