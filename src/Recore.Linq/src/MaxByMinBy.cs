using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class RecoreEnumerable
    {
        public static IEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key) where TKey : IComparable<TKey>
        {
            // TODO
            return source;
        }
    }
}