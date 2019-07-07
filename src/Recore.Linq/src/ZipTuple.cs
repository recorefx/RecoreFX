using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Merges two sequences to a single sequence of tuples.
        /// </summary>
        /// <remarks>
        /// If the sequences are of different lengths,
        /// </remarks>
        public static IEnumerable<(TFirst first, TSecond second)> Zip<TFirst, TSecond>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second)
            => first.Zip(second, (x, y) => (x, y));
    }
}