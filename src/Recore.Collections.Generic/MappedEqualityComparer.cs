using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type based on the output of a mapping function.
    /// </summary>
    public sealed class MappedEqualityComparer<T, TMapped> : IEqualityComparer<T>
    {
        private readonly static EqualityComparer<TMapped> mappingComparer = EqualityComparer<TMapped>.Default;

        private readonly Func<T, TMapped> mapping;

        /// <summary>
        /// Creates an instance of <see cref="MappedEqualityComparer{T, TMapped}"/>.
        /// </summary>
        public MappedEqualityComparer(Func<T, TMapped> mapping)
        {
            this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        /// <summary>
        /// Invokes the mapping function on two objects and checks if the outputs are equal.
        /// </summary>
        public bool Equals(T? x, T? y)
        {
            TMapped? mappedX = x == null ? default(TMapped?) : mapping(x);
            TMapped? mappedY = y == null ? default(TMapped?) : mapping(y);
            return mappingComparer.Equals(mappedX, mappedY);
        }

        /// <summary>
        /// Hashes the mapped output of an object.
        /// </summary>
        public int GetHashCode(T obj)
        {
            var mapped = mapping(obj);
            if (mapped == null)
            {
                return 0;
            }
            else
            {
                return mappingComparer.GetHashCode(mapped);
            }
        }
    }
}
