using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        /// <summary>
        /// Returns the maximum value and the index of the maximum value for a function from a sequence of values.
        /// </summary>
        public static (int Argmax, TSource Max) Argmax<TSource>(this IEnumerable<TSource> source)
        {
            var argmax = source
                .Enumerate()
                .Argmax(pair => pair.Item);

            return (Argmax: argmax.Argmax.Index, argmax.Max);
        }

        /// <summary>
        /// Returns the maximum value and the maximizing value for a function from a sequence of values.
        /// </summary>
        public static (TSource Argmax, TResult Max) Argmax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            using (var enumerator = source.GetEnumerator())
            {
                // No elements
                if (!enumerator.MoveNext())
                {
                    if (default(TSource) == null)
                    {
                        // Reference type
                        return default;
                    }
                    else
                    {
                        // Value type
                        throw new InvalidOperationException(); // TODO message
                    }
                }

                // Initialize with first element
                var argmax = enumerator.Current;
                var max = selector(enumerator.Current);
                var comparer = Comparer<TResult>.Default;
                while (enumerator.MoveNext())
                {
                    var value = selector(enumerator.Current);
                    if (comparer.Compare(value, max) > 0)
                    {
                        max = value;
                        argmax = enumerator.Current;
                    }
                }

                return (argmax, max);
            }
        }
    }
}