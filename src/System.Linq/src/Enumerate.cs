using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        public static IEnumerable<(TSource item, int index)> Enumerate<TSource>(this IEnumerable<TSource> source)
        {
            int i = 0;
            foreach (var item in source)
            {
                yield return (item: item, index: i);
                i++;
            }
        }
    }
}