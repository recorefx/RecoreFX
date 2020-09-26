using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recore.Linq
{
    public static partial class Renumerable
    {
        /// <summary>
        /// Converts an asynchronously-returned sequence of <typeparamref name="T"/> to <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="IAsyncEnumerable{T}"/> returned by this method won't yield the same throughput improvement of
        /// asynchronously generating each element in the sequence.
        /// Rather, this is a shim to help more methods work with <see cref="IAsyncEnumerable{T}"/>
        /// and encourage its use in a codebase.
        /// </remarks>
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this Task<IEnumerable<T>> source)
        {
            foreach (var item in await source)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Converts an sequence of tasks to <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// This is a shim to help more methods work with <see cref="IAsyncEnumerable{T}"/>
        /// and encourage its use in a codebase.
        /// </remarks>
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<Task<T>> source)
        {
            foreach (var item in source)
            {
                yield return await item;
            }
        }
    }
}