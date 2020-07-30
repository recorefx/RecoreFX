using System;
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
    /// using System.Collections.Generic;
    ///
    /// class OrderStatusUpdater
    /// {
    ///     private readonly IEnumerator&lt;string&gt; statusEnumerator;
    ///
    ///     private string currentStatus = null;
    ///
    ///     // We need to look ahead so we know when we've returned the last element
    ///     private string nextStatus = null;
    ///     private bool isFinished = false;
    ///
    ///     public OrderStatusUpdater(IEnumerable&lt;string&gt; statuses)
    ///     {
    ///         statusEnumerator = statuses.GetEnumerator();
    ///         UpdateStatus();
    ///     }
    ///
    ///     private void UpdateStatus()
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
    /// using System.Collections.Generic;
    ///
    /// using Recore.Collections.Generic;
    ///
    /// class OrderStatusUpdater
    /// {
    ///     private readonly IIterator&lt;string&gt; statusIterator;
    ///
    ///     private string currentStatus = null;
    ///
    ///     public OrderStatusUpdater(IEnumerable&lt;string&gt; statuses)
    ///     {
    ///         statusIterator = Iterator.FromEnumerable(statuses);
    ///     }
    ///
    ///     public OrderStatus GetStatusUpdate()
    ///     {
    ///         if (statusIterator.HasNext)
    ///         {
    ///             currentStatus = statusIterator.Next();
    ///         }
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
        public static IIterator<T> FromEnumerator<T>(IEnumerator<T> enumerator)
        {
            if (enumerator is null)
            {
                throw new ArgumentNullException(nameof(enumerator));
            }

            return new IteratorFromEnumerator<T>(enumerator);
        }

        /// <summary>
        /// Retrieves an <see cref="IIterator{T}"/> for the collection.
        /// </summary>
        public static IIterator<T> FromEnumerable<T>(IEnumerable<T> source)
        {
            return FromEnumerator(source.GetEnumerator());
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

            if (HasNext)
            {
                current = enumerator.Current; // appease the compiler; will get discarded on the first call to `Next()`
                lookahead = enumerator.Current;
            }
        }

        public T Next()
        {
            if (HasNext)
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
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
