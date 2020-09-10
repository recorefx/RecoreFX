using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        // public static TSource Argmin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        // {
        //     return source.GetEnumerator().Current;
        // }

        /// <summary>
        /// Returns the minimum value and the index of the minimum value for a function from a sequence of values.
        /// </summary>
        public static (int Argmin, TSource Min) Argmin<TSource>(this IEnumerable<TSource> source)
        {
            var argmin = source
                .Enumerate()
                .Argmin(pair => pair.Item);

            return (Argmin: argmin.Argmin.Index, argmin.Min);
        }

        /// <summary>
        /// Returns the minimum and the minimizing value for a function from a sequence of values.
        /// </summary>
        public static (TSource Argmin, TResult Min) Argmin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
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
                var argmin = enumerator.Current;
                var min = selector(enumerator.Current);
                var comparer = Comparer<TResult>.Default;
                while (enumerator.MoveNext())
                {
                    var value = selector(enumerator.Current);
                    if (comparer.Compare(value, min) < 0)
                    {
                        min = value;
                        argmin = enumerator.Current;
                    }
                }

                return (argmin, min);
            }
        }
    }
}