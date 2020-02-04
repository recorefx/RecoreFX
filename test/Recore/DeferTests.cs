using System;

using Xunit;

namespace Recore.Tests
{
    public class DeferTests
    {
        [Fact]
        public void InvokesCallbackOnDispose()
        {
            bool called = false;

            using (var defer = new Defer(() => { called = true; }))
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
    }
}