using System;
using System.Threading.Tasks;

namespace Recore
{
    /// <summary>
    /// Invokes an action when disposed.
    /// </summary>
    /// <remarks>
    /// Not thread-safe.
    /// Concurrent calls to dispose the object may result in the action being invoked
    /// multiple times.
    /// However, sequential calls to <see cref="Dispose"/> are idempotent.
    /// If an instance of this type is created and never disposed, the callback will not be called.
    /// By design, the callback is not called from the finalizer, which would happen non-determinstically.
    /// </remarks>
    public sealed class Defer : IDisposable
    {
        private readonly Action action;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes an object with an action to invoke when the object is disposed.
        /// </summary>
        public Defer(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.action = action;
        }

        /// <summary>
        /// Invokes the callback registered with the object and marks the object as disposed.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            action();
            isDisposed = true;
        }
    }

    /// <summary>
    /// Invokes an asynchronous action when disposed.
    /// </summary>
    /// <remarks>
    /// Not thread-safe.
    /// Concurrent calls to dispose the object may result in the action being invoked
    /// multiple times.
    /// However, sequential calls to <see cref="DisposeAsync"/> are idempotent.
    /// If an instance of this type is created and never disposed, the callback will not be called.
    /// By design, the callback is not called from the finalizer, which would happen non-determinstically.
    /// </remarks>
    public sealed class AsyncDefer : IAsyncDisposable
    {
        private readonly AsyncAction action;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes an object with an action to invoke when the object is disposed.
        /// </summary>
        public AsyncDefer(AsyncAction action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.action = action;
        }

        /// <summary>
        /// Invokes the callback registered with the object and marks the object as disposed.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (isDisposed)
            {
                return;
            }

            await action();
            isDisposed = true;
        }
    }
}