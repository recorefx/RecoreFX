using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Provides additional methods for working with <c cref="LinkedList{T}">LinkedList</c>.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Adds a new node with containing the specified value to the end of the <c>LinkedList&lt;T&gt;</c>.
        /// </summary>
        /// <remarks>
        /// This method is the same as <c>AddLast</c>.
        /// It is needed to be able to use collection initializer syntax with <c>LinkedList&lt;T&gt;</c>.
        /// </remarks>
        public static void Add<T>(this LinkedList<T> linkedList, T item)
            => linkedList.AddLast(item);

        /// <summary>
        /// Adds a new node with containing the specified value to the end of the <c>LinkedList&lt;T&gt;</c>
        /// and returns the <c>LinkedList&lt;T&gt;</c>.
        /// </summary>
        public static LinkedList<T> Append<T>(this LinkedList<T> linkedList, T item)
        {
            linkedList.AddLast(item);
            return linkedList;
        }
    }
}