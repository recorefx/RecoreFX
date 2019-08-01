using System;

namespace Recore
{
    /// <summary>
    /// A type with only one value.
    /// </summary>
    /// <remarks>
    /// Whereas <c>void</c> is a type with no values,
    /// <c cref="Unit">Unit</c> is a type with one value.
    /// It is useful when designing generic types or methods so that a non-generic version
    /// does not have to be provided.
    /// It is also useful for fluent interfaces (such as LINQ)
    /// so that a chain of method calls does not have to be broken by a <c>void</c>-returning call.
    /// </remarks>
    public readonly struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// Converts a return type of <c>void</c> to a return type of <c cref="Unit">Unit</c>.
        /// </summary>
        public static Func<Unit> Close(Action action) => () =>
        {
            action();
            return new Unit();
        };

        /// <summary>
        /// Determines whether another object is the unit value.
        /// </summary>
        public override bool Equals(object obj) => obj is Unit;

        /// <summary>
        /// Two unit instances are always equal.
        /// </summary>
        public bool Equals(Unit other) => true;

        /// <summary>
        /// Returns the hash code of the unit value.
        /// </summary>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a string representation of the unit value.
        /// </summary>
        public override string ToString() => "()";

        /// <summary>
        /// Two unit instances are always equal.
        /// </summary>
        public static bool operator ==(Unit lhs, Unit rhs) => true;

        /// <summary>
        /// Two unit instances are always equal.
        /// </summary>
        public static bool operator !=(Unit lhs, Unit rhs) => false;
    }
}