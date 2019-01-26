using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class RecoreEnumerable
    {
        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<TSource> source) where TSource : class =>
            source
            .Where(x => x != null);

        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<TSource?> source) where TSource : struct =>
            source
            .Where(x => x != null)
            .Select(x => x.Value);
    }
}