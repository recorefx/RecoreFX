using System;
using System.Threading.Tasks;

namespace Recore.Threading.Tasks
{
    /// <summary>
    /// Provides additional methods for working with tasks.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Blocks the current thread until the task completes.
        /// </summary>
        /// <remarks>
        /// If <paramref name="task"/> is terminated by an exception, that exception
        /// will be rethrown in the current context.
        /// Unlike <see cref="Task.Wait()"/>, that exception will be of its original type,
        /// not <see cref="AggregateException"/>.
        /// It will also preserve its original stack trace.
        /// This exception-throwing behavior is the same as if you had used <c>await</c>.
        /// Note that it is still possible to deadlock with this method.
        /// See https://blog.stephencleary.com/2014/12/a-tour-of-task-part-6-results.html.
        /// </remarks>
        public static void Synchronize(this Task task) => task.GetAwaiter().GetResult();

        /// <summary>
        /// Blocks the current thread until the task completes.
        /// </summary>
        /// <remarks>
        /// If <paramref name="task"/> is terminated by an exception, that exception
        /// will be rethrown in the current context.
        /// Unlike <see cref="Task{TResult}.Result"/>, that exception will be of its original type,
        /// not <see cref="AggregateException"/>.
        /// It will also preserve its original stack trace.
        /// This exception-throwing behavior is the same as if you had used <c>await</c>.
        /// Note that it is still possible to deadlock with this method.
        /// See https://blog.stephencleary.com/2014/12/a-tour-of-task-part-6-results.html.
        /// </remarks>
        public static T Synchronize<T>(this Task<T> task) => task.GetAwaiter().GetResult();
    }
}