using System; // reference RecoreFX System.dll
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        public static IEnumerable<Required<TSource>> NonNull<TSource>(this IEnumerable<TSource> source) where T : class =>
            source
            .Where(source != null)
            .Select(x => new Required<TSource>(x));

        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<Nullable<TSource>> source) where T : struct =>
            source
            .Where(source != null)
            .Select(x => x.Value);
    }
}