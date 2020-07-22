using System;

namespace Recore
{
    /// <summary>
    /// Abstract base class for defining types that alias an existing type.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Of{T}"/> to create a strongly-typed "alias" of another type.
    ///
    /// You can use a <c>using</c> directive to create an alias for a type, but the scope of that alias is limited to that file.
    /// Furthermore, the alias is just that -- an alias -- not a separate type.
    /// So, an alias won't prevent errors like this:
    /// <code>
    /// using Name = string;
    /// using Address = string;
    /// class Person
    /// {
    ///     public Person(int age, Address address, Name name)
    ///     {
    ///     }
    /// }
    ///
    /// var person = new Person(22, "Alice", "1 Microsoft Way"); // oops!
    /// </code>
    /// </remarks>
    /// <example>
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
            => obj is Of<T> of && Equals(of);

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