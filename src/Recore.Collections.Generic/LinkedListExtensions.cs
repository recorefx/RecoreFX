using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <see cref="LinkedList{T}"/>.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Adds a new node with containing the specified value to the end of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <remarks>
        /// This method is the same as <see cref="LinkedList{T}.AddLast(T)"/>.
        /// It is needed to be able to use collection initializer syntax with <see cref="LinkedList{T}"/>.
        /// </remarks>
        public static void Add<T>(this LinkedList<T> linkedList, T item)
            => linkedList.StaticCast<ICollection<T>>().Add(item);
    }
}