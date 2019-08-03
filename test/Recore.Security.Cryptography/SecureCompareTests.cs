using System;

using Xunit;

namespace Recore.Security.Cryptography.Tests
{
    public class SecureCompareTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SecureCompare.TimeInvariantEquals(null, new byte[] { }));

            Assert.Throws<ArgumentNullException>(
                () => SecureCompare.TimeInvariantEquals(new byte[] { }, null));
        }

        [Fact]
        public void UnequalLength()
        {
            Assert.False(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { }));
        }

        [Fact]
        public void EqualSequences()
        {
            Assert.True(SecureCompare.TimeInvariantEquals(new byte[] { }, new byte[] { }));
            Assert.True(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { 0 }));
            Assert.True(SecureCompare.TimeInvariantEquals(new byte[] { 0, 1, 2, 3 }, new byte[] { 0, 1, 2, 3 }));
        }

        [Fact]
        public void UnequalSequences()
        {
            Assert.False(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { 1 }));
            Assert.False(SecureCompare.TimeInvariantEquals(new byte[] { 1 }, new byte[] { 0 }));
        }
    }
}