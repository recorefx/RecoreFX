using System.Threading.Tasks;

namespace Recore.Threading.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Block the current thread until the task completes.
        /// </summary>
        /// <remarks>
        /// If <paramref name="task"/> is terminated by an exception, that exception
        /// will be rethrown in the current context.
        /// Unlike <c>Task.Wait</c>, that exception will be of its original type,
        /// not <c>AggregateException</c>.
        // Also, it will preserve its original stack trace.
        /// </remarks>
        public static void Synchronize(this Task task) => task.GetAwaiter().GetResult();

        /// <summary>
        /// Block the current thread until the task completes.
        /// </summary>
        /// <remarks>
        /// If <paramref name="task"/> is terminated by an exception, that exception
        /// will be rethrown in the current context.
        /// Unlike <c>Task&lt;T&gt;.Result</c>, that exception will be of its original type,
        /// not <c>AggregateException</c>.
        /// Also, it will preserve its original stack trace.
        /// </remarks>
        public static T Synchronize<T>(this Task<T> task) => task.GetAwaiter().GetResult();
    }
}