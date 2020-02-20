using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Creates a <see cref="LinkedList{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// Linked lists don't need to be resized when adding elements,
        /// which can give this method better performance than <see cref="Enumerable.ToList{T}(IEnumerable{T})"/> or <see cref="Enumerable.ToArray{T}(IEnumerable{T})"/>.
        /// A common case is when you just want to force eager evaluation of a series of
        /// operations on an <see cref="IEnumerable{T}"/>
        /// or when you want to cache elements when performing multiple enumerations.
        /// For these cases, you don't need random access to elements, which makes
        /// <see cref="LinkedList{T}"/> a suitable data structure for storing the elements.
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