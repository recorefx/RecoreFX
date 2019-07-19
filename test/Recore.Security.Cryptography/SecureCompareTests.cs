using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Security.Cryptography.Tests
{
    [TestClass]
    public class SecureCompareTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => SecureCompare.TimeInvariantEquals(null, new byte[] { }));

            Assert.ThrowsException<ArgumentNullException>(
                () => SecureCompare.TimeInvariantEquals(new byte[] { }, null));
        }

        [TestMethod]
        public void UnequalLength()
        {
            Assert.IsFalse(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { }));
        }

        [TestMethod]
        public void EqualSequences()
        {
            Assert.IsTrue(SecureCompare.TimeInvariantEquals(new byte[] { }, new byte[] { }));
            Assert.IsTrue(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { 0 }));
            Assert.IsTrue(SecureCompare.TimeInvariantEquals(new byte[] { 0, 1, 2, 3 }, new byte[] { 0, 1, 2, 3 }));
        }

        [TestMethod]
        public void UnequalSequences()
        {
            Assert.IsFalse(SecureCompare.TimeInvariantEquals(new byte[] { 0 }, new byte[] { 1 }));
            Assert.IsFalse(SecureCompare.TimeInvariantEquals(new byte[] { 1 }, new byte[] { 0 }));
        }
    }
}