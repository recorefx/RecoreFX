using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        public static IEnumerable<(int index, TSource item)> Enumerate<TSource>(this IEnumerable<TSource> source)
            => Enumerable.Range(0, int.MaxValue).Zip(source);
    }
}