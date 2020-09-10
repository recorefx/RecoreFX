using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Returns each element from a sequence along with its number from the beginning of the sequence, starting from zero.
        /// </summary>
        public static IEnumerable<(int Index, TSource Item)> Enumerate<TSource>(this IEnumerable<TSource> source)
            => Enumerable.Range(0, int.MaxValue).Zip(source);
    }
}