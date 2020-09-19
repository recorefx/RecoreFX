using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type using the given equality function.
    /// </summary>
    /// <remarks>
    /// Note that there is no corresponding <c>AnonymousComparer&lt;T&gt;</c> type for <see cref="IComparer{T}"/>.
    /// That use case is already filled by <see cref="Comparer{T}.Create(Comparison{T})"/>.
    /// </remarks>
    public sealed class AnonymousEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> getHashCode;

        /// <summary>
        /// Creates an instance of <see cref="AnonymousEqualityComparer{T}"/>.
        /// </summary>
        public AnonymousEqualityComparer(Func<T?, T?, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals ?? throw new ArgumentNullException(nameof(equals));
            this.getHashCode = getHashCode ?? throw new ArgumentNullException(nameof(getHashCode));
        }

        /// <summary>
        /// Invokes the given comparison function on two objects.
        /// </summary>
        public bool Equals(T? x, T? y) => equals(x, y);


        /// <summary>
        /// Invokes the given hashing function on an object.
        /// </summary>
        public int GetHashCode(T obj) => getHashCode(obj);
    }
}
