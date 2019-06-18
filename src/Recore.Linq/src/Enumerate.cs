using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class RecoreEnumerable
    {
        public static IEnumerable<(int index, TSource item)> Enumerate<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int i = 0;
            foreach (var item in source)
            {
                yield return (index: i, item);
                i++;
            }
        }
    }
}