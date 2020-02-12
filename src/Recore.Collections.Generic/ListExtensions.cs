using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <see cref="List{T}" />.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the list and passes the list through.
        /// </summary>
        public static List<T> AppendRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            list.AddRange(collection);
            return list;
        }
    }
}