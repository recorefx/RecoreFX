using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Collects all non-null values from the sequence.
        /// </summary>
        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<TSource?> source)
        where TSource : class
            => source
                .Where(x => x != null)
                .Select(x => x!);

        /// <summary>
        /// Collects all non-null values from the sequence.
        /// </summary>
        public static IEnumerable<TSource> NonNull<TSource>(this IEnumerable<TSource?> source)
        where TSource : struct
            => source
                .Where(x => x != null)
                .Select(x => x!.Value);
    }
}