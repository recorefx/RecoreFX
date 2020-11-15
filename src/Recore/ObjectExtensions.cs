using System;
using System.Threading.Tasks;

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
        public static T StaticCast<T>(this T obj) => obj;

        /// <summary>
        /// Invokes a function on an object.
        /// </summary>
        public static U Apply<T, U>(this T obj, Func<T, U> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return func(obj);
        }

        /// <summary>
        /// Invokes an asynchronous function on a task.
        /// </summary>
        public static async Task<U> ApplyAsync<T, U>(this Task<T> task, AsyncFunc<T, U> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return await func(await task);
        }
    }
}
