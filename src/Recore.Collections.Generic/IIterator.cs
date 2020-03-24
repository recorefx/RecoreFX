using System.Collections;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// An alternative interface to <see cref="IEnumerator{T}"/> that provides access to a sequence of elements.
    /// </summary>
    /// <remarks>
    /// This interface is meant to resemble Java's concept of an iterator.
    /// While using <see cref="IEnumerator{T}"/> usually provides safer access to a collection than, say,
    /// manipulating indices directly in a list, there are times where it is clumsy to use.
    /// </remarks>
    /// <example>
    /// A common case is calling an API that needs to tell its caller whether it should be called again.
    /// In order to do this with <see cref="IEnumerator{T}"/>, the API must look ahead to the next element
    /// with <see cref="IEnumerator.MoveNext()"/> before returning and then maintain this state between calls.
    ///
    /// For example, consider this type that gets status information on a customer's order from some external source:
    /// <code>
    /// class OrderStatusUpdater
    /// {
    ///     private readonly IEnumerator&lt;string&gt; statusEnumerator;
    ///
    ///     private string currentStatus;
    ///     private string nextStatus;
    ///     private bool isFinished;
    ///
    ///     public OrderStatusUpdater(IEnumerable&lt;string&gt; statuses)
    ///     {
    ///         statusEnumerator = statuses.GetEnumerator();
    ///         nextStatus = "Not started";
    ///
    ///         // The assignment to currentStatus is redundant.
    ///         // It is just to appease the compiler when nullable references are enabled.
    ///         currentStatus = UpdateStatus();
    ///     }
    ///
    ///     private string UpdateStatus()
    ///     {
    ///         currentStatus = nextStatus;
    ///
    ///         if (statusEnumerator.MoveNext())
    ///         {
    ///             nextStatus = statusEnumerator.Current;
    ///         }
    ///         else
    ///         {
    ///             isFinished = true;
    ///         }
    ///
    ///         return currentStatus;
    ///     }
    ///
    ///     public OrderStatus GetStatusUpdate()
    ///     {
    ///         UpdateStatus();
    ///
    ///         return new OrderStatus
    ///         {
    ///             Status = currentStatus,
    ///             IsFinished = isFinished
    ///         };
    ///     }
    /// }
    /// </code>
    ///
    /// Now consider the code with <see cref="IIterator{T}"/>:
    /// <code>
    /// class OrderStatusUpdater
    /// {
    ///     private readonly IIterator&lt;string&gt; statusIterator;
    ///
    ///     private string currentStatus;
    ///
    ///     public OrderStatusUpdater(IEnumerable&lt;string&gt; statuses)
    ///     {
    ///         statusIterator = statuses.GetIterator();
    ///         currentStatus = "Not started";
    ///     }
    ///
    ///     public OrderStatus GetStatusUpdate()
    ///     {
    ///         currentStatus = statusIterator.Next();
    ///
    ///         return new OrderStatus
    ///         {
    ///             Status = currentStatus,
    ///             IsFinished = !statusIterator.HasNext
    ///         };
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface IIterator<out T>
    {
        /// <summary>
        /// Advances the iterator to the next element and returns it.
        /// </summary>
        /// <returns></returns>
        T Next();

        /// <summary>
        /// Whether the sequence has more elements.
        /// </summary>
        bool HasNext { get; }
    }

    /// <summary>
    /// Provides helper methods for working with <see cref="IIterator{T}"/>.
    /// </summary>
    public static class Iterator
    {
        /// <summary>
        /// Converts an <see cref="IEnumerator{T}"/> to an <see cref="IIterator{T}"/>.
        /// </summary>
        public static IIterator<T> ToIterator<T>(this IEnumerator<T> enumerator)
        {
            return new IteratorFromEnumerator<T>(enumerator);
        }

        /// <summary>
        /// Retrieves an <see cref="IIterator{T}"/> for the collection.
        /// </summary>
        public static IIterator<T> GetIterator<T>(this IEnumerable<T> source)
        {
            return source.GetEnumerator().ToIterator();
        }
    }

    internal sealed class IteratorFromEnumerator<T> : IIterator<T>
    {
        public bool HasNext { get; private set; }

        private readonly IEnumerator<T> enumerator;
        private T current;
        private T lookahead;

        public IteratorFromEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;

            // The enumerator starts behind the first element
            HasNext = enumerator.MoveNext();
            current = enumerator.Current; // appease the compiler; will get discarded on the first call to `Next()`
            lookahead = enumerator.Current;
        }

        public T Next()
        {
            current = lookahead;

            if (enumerator.MoveNext())
            {
                lookahead = enumerator.Current;
            }
            else
            {
                HasNext = false;
            }

            return current;
        }
    }
}
