using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type based on the output of a mapping function.
    /// </summary>
    public sealed class MappedComparer<T, U> : IComparer<T>
    {
        private readonly static Comparer<U> mappingComparer = Comparer<U>.Default;

        private readonly Func<T, U> mapping;

        /// <summary>
        /// Creates an instance of <see cref="MappedComparer{T, U}"/>.
        /// </summary>
        public MappedComparer(Func<T, U> mapping)
        {
            this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        /// <summary>
        /// Compares the mapped output of two objects.
        /// </summary>
        public int Compare(T? x, T? y)
        {
            U? mappedX = x == null ? default(U?) : mapping(x);
            U? mappedY = y == null ? default(U?) : mapping(y);
            return mappingComparer.Compare(mappedX, mappedY);
        }
    }
}
