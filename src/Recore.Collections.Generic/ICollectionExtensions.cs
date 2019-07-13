using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    public static class ICollectionExtensions
    {
        // Fluent version of Add
        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return collection;
        }
    }
}