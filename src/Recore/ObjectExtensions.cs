using System;

namespace Recore
{
    /// <summary>
    /// Helper methods for working with any object in .NET.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object's type to <typeparamref name="T"/> at compile time.
        /// </summary>
        /// <remarks>
        /// This method is analogous to <c>static_cast</c> in C++.
        /// In C#, casting is normally an unsafe operation: a failed cast will throw <see cref="InvalidCastException"/>.
        /// On the other hand, <see cref="StaticCast{T}"/> will never throw at runtime if it compiles.
        /// </remarks>
        public static T StaticCast<T>(this T obj) => obj;
    }
}
