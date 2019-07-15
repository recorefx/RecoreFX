using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <c cref="IReadOnlyDictionary{TKey, TValue}">IReadOnlyDictionary</c>.
    /// </summary>
    public static class IReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Gets the value that is associated with the specific key or the default value for the type <typeparamref name="TValue"/>.
        /// </summary>
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
    }
}