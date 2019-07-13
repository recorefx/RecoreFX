using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Performs an action on each element in a sequence.
        /// </summary>
        /// <remarks>
        /// This method is evaluated eagerly.
        /// </remarks>
        public static void ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}