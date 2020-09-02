using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class IReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Gets the value that is associated with the specific key or the default value for the type <typeparamref name="TValue"/>.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
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
        /// Gets the value that is associated with the specific key or the default value for the type <typeparamref name="TValue"/>.
        /// </summary>
        /// <remarks>
        /// This is duplicated from <see cref="GetValueOrDefault{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey)"/>
        /// in order to resolve the compile-time ambiguity between that method and <see cref="IDictionaryExtensions.GetValueOrDefault{TKey, TValue}(IDictionary{TKey, TValue}, TKey)"/>
        /// for instances of <see cref="ReadOnlyDictionary{TKey, TValue}"/>.
        /// </remarks>
        public static TValue GetValueOrDefault<TKey, TValue>(this ReadOnlyDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.StaticCast<IReadOnlyDictionary<TKey, TValue>>().GetValueOrDefault(key);
        }
    }
}