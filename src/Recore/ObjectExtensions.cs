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
        /// On the other hand, <see cref="StaticCast{T}"/> will never throw at run time if it compiles.
        /// </remarks>
        /// <example>
        /// Useful for overcoming those pesky variance issues:
        /// <code>
        /// Task&lt;IEnumerable&lt;object&gt;&gt; GetObjectsAsync()
        /// {
        ///     var result = new[] { "hello" };
        ///     
        ///     // error CS0029 because the static type of `result` is an array, not an `IEnumerable`
        ///     // return Task.FromResult(result);
        /// 
        ///     // 👍
        ///     return Task.FromResult(result.StaticCast&lt;IEnumerable&lt;object&gt;&gt;());
        /// }
        /// </code>
        /// </example>
        public static T StaticCast<T>(this T obj) => obj;
    }
}
