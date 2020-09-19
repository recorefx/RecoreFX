﻿using System;
using System.Collections.Generic;

namespace Recore.Collections.Generic
{
    /// <summary>
    /// Compares instances of a type based on the output of a mapping function.
    /// </summary>
    public sealed class MappedEqualityComparer<T, U> : IEqualityComparer<T>
    {
        private readonly static EqualityComparer<U> mappingComparer = EqualityComparer<U>.Default;

        private readonly Func<T, U> mapping;

        /// <summary>
        /// Creates an instance of <see cref="MappedEqualityComparer{T, U}"/>.
        /// </summary>
        public MappedEqualityComparer(Func<T, U> mapping)
        {
            this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        /// <summary>
        /// Invokes the mapping function on two objects and checks if the outputs are equal.
        /// </summary>
        public bool Equals(T? x, T? y)
            => mappingComparer.Equals(mapping(x), mapping(y));

        /// <summary>
        /// Hashes the mapped output of an object.
        /// </summary>
        public int GetHashCode(T obj)
            => mappingComparer.GetHashCode(mapping(obj));
    }
}
