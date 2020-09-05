using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{TKey, TValue}"/> from a sequence of key-value pairs.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
            => source.ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// Projects each key in a sequence of key-value pairs to a new form.
        /// </summary>
        public static IEnumerable<KeyValuePair<TResult, TValue>> OnKeys<TKey, TValue, TResult>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source,
            Func<TKey, TResult> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source
                // TODO .NET Standard 2.1 use KeyValuePair.Create
                .Select(kvp => new KeyValuePair<TResult, TValue>(func(kvp.Key), kvp.Value))
                .ToDictionary();
        }

        /// <summary>
        /// Projects each value in a sequence of key-value pairs to a new form.
        /// </summary>
        public static IEnumerable<KeyValuePair<TKey, TResult>> OnValues<TKey, TValue, TResult>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source,
            Func<TValue, TResult> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source
                // TODO .NET Standard 2.1 use KeyValuePair.Create
                .Select(kvp => new KeyValuePair<TKey, TResult>(kvp.Key, func(kvp.Value)))
                .ToDictionary();
        }
    }
}