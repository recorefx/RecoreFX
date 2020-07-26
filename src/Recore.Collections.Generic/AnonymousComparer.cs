using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type using the given comparison function.
    /// </summary>
    public sealed class AnonymousComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> compare;

        /// <summary>
        /// Creates an instance of <see cref="AnonymousComparer{T}"/>.
        /// </summary>
        public AnonymousComparer(Func<T, T, int> compare)
        {
            this.compare = compare ?? throw new ArgumentNullException(nameof(compare));
        }

        /// <summary>
        /// Invokes the given comparison function on two objects.
        /// </summary>
        public int Compare(T x, T y) => compare(x, y);
    }
}
