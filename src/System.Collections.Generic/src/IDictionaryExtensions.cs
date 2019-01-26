using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    public static class IDictionaryExtensions
    {
        // Fluent version of Add
        public static IDictionary<TKey, TValue> Append<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return dict;
        }
    }
}