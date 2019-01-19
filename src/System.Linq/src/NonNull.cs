using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        //public static IEnumerable<Required<TSource>> NonNull<TSource>(this IEnumerable<TSource> source) where TSource : class =>
        //    source
        //    .Where(x => x != null)
        //    .Select(x => new Required<TSource>(x));

        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<Nullable<TSource>> source) where TSource : struct =>
            source
            .Where(x => x != null)
            .Select(x => x.Value);
    }
}