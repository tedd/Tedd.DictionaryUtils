using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd;

public static class AddExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key,
        Func<TValue> valueFactory)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        if (valueFactory is null)
            throw new ArgumentNullException(nameof(valueFactory));

        // Return if we have it
        if (dic.TryGetValue(key, out var value))
            return value;

        // Generate it
        value = valueFactory();
        dic.Add(key, value);

        return value;
    }
}
