using System;
using System.Collections.Generic;

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

        public static TSource Argmax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var comparer = Comparer<TResult>.Default;
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
                while (enumerator.MoveNext())
                {
                    var value = selector(enumerator.Current);
                    if (comparer.Compare(value, max) > 0)
                    {
                        max = value;
                        argmax = enumerator.Current;
                    }
                }

                return argmax;
            }
        }
    }
}