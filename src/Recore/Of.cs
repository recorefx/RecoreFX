using System;
using System.Text.Json.Serialization;

using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Abstract base class for defining types that alias an existing type.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Of{T}"/> to create a strongly-typed "alias" of another type.
    ///
    /// You can use a <c>using</c> directive to create an alias for a type,
    /// but the scope of that alias is limited to that file.
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
    /// 
    /// Note: as of C# 9, you can replace many use cases for <see cref="Of{T}"/> with record types:
    /// <code>
    /// record Address(string Value);
    /// record Name(string Value);
    /// </code>
    /// 
    /// <see cref="Of{T}"/> is not marked with <see cref="ObsoleteAttribute"/> because records have some limitations that classes do not have.
    /// See <see cref="Token"/> and <see cref=" Recore.Security.Cryptography.Ciphertext{THash}"/> for examples of <see cref="Of{T}"/> subtypes
    /// that can't be converted to records.
    /// </remarks>
    [JsonConverter(typeof(OfConverter))]
    public abstract class Of<T> : IEquatable<Of<T>?>
    {
        /// <summary>
        /// The underlying instance of the wrapped type.
        /// </summary>
        public T? Value { get; init; }

        /// <summary>
        /// Converts this <see cref="Of{T}"/> to another subtype of <see cref="Of{T}"/>
        /// with the same value of <typeparamref name="T"/>.
        /// </summary>
        public TOf To<TOf>() where TOf : Of<T>, new()
            => new TOf { Value = Value };

        /// <summary>
        /// Returns the string representation for the underlying object.
        /// </summary>
        #nullable disable // Set to oblivious because T.ToString() is oblivious
        public override string ToString()
        {
            if (Value == null)
            {
                return string.Empty;
            }
            else
            {
                return Value.ToString();
            }
        }
        #nullable enable

        /// <summary>
        /// Determines whether this instance is equal to another object.
        /// </summary>
        public override bool Equals(object? obj)
            => obj is Of<T> of && Equals(of);

        /// <summary>
        /// Determines whether two instances of the type are equal.
        /// </summary>
        /// <remarks>
        /// Note that instances of two separate subtypes of <see cref="Of{T}"/>
        /// will compare equal to each other if their values are the same type and are equal.
        /// </remarks>
        public bool Equals(Of<T>? other)
            => !(other is null)
            && Equals(Value, other.Value);

        /// <summary>
        /// Returns the hash code for the underlying object.
        /// </summary>
        public override int GetHashCode() => Value!.GetHashCode();

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

        /// <summary>
        /// Converts an instance of <see cref="Of{T}"/> to its inner type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Of{T}"/> is conceptually (though not in fact) a subtype of <typeparamref name="T"/>.
        /// This conversion allows instances of <see cref="Of{T}"/> to work with methods out of the caller's control.
        /// </remarks>
        public static implicit operator T?(Of<T> of) => of.Value;
    }
}