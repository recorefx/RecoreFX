using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Returns each element from a sequence along with its number from the beginning of the sequence, starting from zero.
        /// </summary>
        public static IEnumerable<(TFirst first, TSecond second)> Product<TFirst, TSecond>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            foreach (var firstItem in first)
            {
                foreach (var secondItem in second)
                {
                    yield return (firstItem, secondItem);
                }
            }
        }
    }
}