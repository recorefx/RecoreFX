using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <see cref="IDictionary{TKey, TValue}" />.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Gets the value that is associated with the specific key or the default value for the type <typeparamref name="TValue"/>.
        /// </summary>
        /// <remarks>
        /// This is duplicated from <c cref="IReadOnlyDictionaryExtensions">IReadOnlyDictionaryExtensions</c>
        /// because <c cref="IDictionary{TKey, TValue}">IDictionary</c> does not extend <c cref="IReadOnlyDictionary{TKey, TValue}">IReadOnlyDictionary</c>.
        /// </remarks>
        public static TValue ValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Adds an entry to the dictionary and passes the dictionary through.
        /// </summary>
        public static IDictionary<TKey, TValue> Append<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return dict;
        }
    }
}