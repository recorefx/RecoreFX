using System;
using System.Collections.Generic;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Creates a <c>LinkedList&lt;T&gt;</c> from an <c>IEnumerable&lt;T&gt;</c>.
        /// </summary>
        /// <remarks>
        /// Linked lists don't need to be resized when adding elements,
        /// which can give this method better performance than <c>ToList</c> or <c>ToArray</c>.
        /// A common case is when you just want to force eager evaluation of a series of
        /// operations on an <c>IEnumerable</c> or when you want to cache elements when performing
        /// multiple enumerations.
        /// For these cases, you don't need random access to elements, which makes
        /// <c>LinkedList</c> a suitable data structure for storing the elements.
        /// </remarks>
        public static LinkedList<TSource> ToLinkedList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = new LinkedList<TSource>();
            foreach (var item in source)
            {
                result.AddLast(item);
            }
            return result;
        }
    }
}