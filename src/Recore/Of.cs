using System;

namespace Recore
{
    /// <summary>
    /// Abstract base class for defining types that alias an existing type.
    /// </summary>
    /// <example>
    /// Use <see cref="Of{T}"/> to create a strongly-typed "alias" of another type.
    /// <code>
    /// class Address : Of&lt;string&gt; {}
    /// var address = new Address { Value = "1 Microsoft Way" };
    /// Console.WriteLine(address); // prints "1 Microsoft Way"
    /// </code>
    /// </example>
    public abstract class Of<T> : IEquatable<Of<T>>
    {
        /// <summary>
        /// The underlying instance of the wrapped type.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Returns the string representation for the underlying object.
        /// </summary>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// Determines whether this instance is equal to another object.
        /// </summary>
        public override bool Equals(object obj)
            => obj is Of<T> && Equals((Of<T>)obj);

        /// <summary>
        /// Determines whether two instances of the type are equal.
        /// </summary>
        public bool Equals(Of<T> other)
            => other != null
            && Equals(Value, other.Value);

        /// <summary>
        /// Returns the hash code for the underlying object.
        /// </summary>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Determines whether two instances of the type are equal.
        /// </summary>
        public static bool operator ==(Of<T> lhs, Of<T> rhs)
            => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of the type are not equal.
        /// </summary>
        public static bool operator !=(Of<T> lhs, Of<T> rhs)
            => !Equals(lhs, rhs);
    }
}