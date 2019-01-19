using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable // I suspect this will conflict
    {
        public static IEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key) where TKey : IComparable<TKey>
        {
            // TODO
            return source;
        }
    }
}