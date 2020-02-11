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

            // Apparently, you need method scope for the GC to consider an object dead.
            // A normal code block won't suffice.
            void TestMethod()
            {
                var defer = new Defer(() => called = true);
                Assert.False(called);
            }

            TestMethod();

            // Even though GC.Collect() is supposed to block until GC is done,
            // it seems that you have to wait on the finalizers as well.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.False(called);
        }
    }
}