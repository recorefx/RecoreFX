using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Gets the value that is associated with the specific key or the default value for the type <typeparamref name="TValue"/>.
        /// </summary>
        /// <remarks>
        /// This is duplicated from <see cref="CollectionExtensions.GetValueOrDefault{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey)"/>
        /// because <see cref="IDictionary{TKey, TValue}"/> does not extend <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
        /// </remarks>
        public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        where TKey : notnull
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
        /// Adds an entry to the <see cref="IDictionary{TKey, TValue}"/> and passes the dictionary through.
        /// </summary>
        public static IDictionary<TKey, TValue> Append<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
        {
            dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist.
        /// Returns the new value, or the existing value if the key already exists.
        /// </summary>
        /// <param name="dict">The dictionary to be operated on.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value to be added, if the key does not already exist.</param>
        /// <returns>
        /// The value for the key.
        /// This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.
        /// </returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
        {
            if (dict.TryGetValue(key, out TValue result))
            {
                return result;
            }
            else
            {
                dict[key] = value;
                return value;
            }
        }

        /// <summary>
        /// Adds all elements of the specified collection to the <see cref="IDictionary{TKey, TValue}"/>.
        /// Existing elements are overwritten if there are duplicates.
        /// </summary>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> collection)
        where TKey : notnull
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                dict[item.Key] = item.Value;
            }
        }
    }
}