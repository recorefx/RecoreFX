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
        public static TResult Apply<T, TResult>(this T obj, Func<T, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return func(obj);
        }

        /// <summary>
        /// Invokes an action on an object and passes the object through.
        /// </summary>
        public static T Apply<T>(this T obj, Action<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return obj.Apply(action.Fluent());
        }

        /// <summary>
        /// Awaits a task and invokes an asynchronous function on a task.
        /// </summary>
        public static async Task<TResult> ApplyAsync<T, TResult>(this Task<T> task, AsyncFunc<T, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return await func(await task);
        }

        /// <summary>
        /// Awaits a task, invokes an asynchronous action on the result, and passes the awaited task through.
        /// </summary>
        public static async Task<T> ApplyAsync<T>(this Task<T> task, AsyncAction<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var result = await task;
            await action(result);
            return result;
        }
    }
}
