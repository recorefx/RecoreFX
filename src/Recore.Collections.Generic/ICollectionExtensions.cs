using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <c cref="ICollection{T}">ICollection</c>.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds an item to the collection and passes the collection through.
        /// </summary>
        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return collection;
        }
    }
}