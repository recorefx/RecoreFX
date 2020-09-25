using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type based on the output of a mapping function.
    /// </summary>
    public sealed class MappedComparer<T, TMapped> : IComparer<T?>
    {
        private readonly static Comparer<TMapped> mappingComparer = Comparer<TMapped>.Default;

        private readonly Func<T, TMapped> mapping;

        /// <summary>
        /// Creates an instance of <see cref="MappedComparer{T, TMapped}"/>.
        /// </summary>
        public MappedComparer(Func<T, TMapped> mapping)
        {
            this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        /// <summary>
        /// Compares the mapped output of two objects.
        /// </summary>
        public int Compare(T? x, T? y)
        {
            TMapped? mappedX = x == null ? default(TMapped?) : mapping(x);
            TMapped? mappedY = y == null ? default(TMapped?) : mapping(y);
            return mappingComparer.Compare(mappedX, mappedY);
        }
    }
}
