using System;

using Xunit;

namespace Recore.Tests
{
    public class DeferTests
    {
        [Fact]
        public void InvokesCallbackWhenDisposed()
        {
            bool called = false;
            using (var defer = new Defer(() => called = true))
            {
                Assert.False(called);
            }

            Assert.True(called);
        }

        [Fact]
        public void DisposeIsIdempotent()
        {
            bool called = false;
            void CallOnlyOnce()
            {
                if (called)
                {
                    throw new InvalidOperationException("The callback was called more than once!");
                }
                else
                {
                    called = true;
                }
            }

            var defer = new Defer(CallOnlyOnce);
            Assert.False(called);

            defer.Dispose();
            Assert.True(called);

            defer.Dispose();
            Assert.True(called);
        }

        [Fact]
        public void DoesNotInvokeCallbackWhenFinalized()
        {
            bool called = false;

            // Allocate a Defer instance in a method scope
            Func.Invoke(Unit.Close(() =>
            {
                var defer = new Defer(() => called = true);
                Assert.False(called);
            }));

            // Garbage collection will put the finalizer in a queue,
            // so we need to wait for the queue to empty.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.False(called);
        }
    }
}