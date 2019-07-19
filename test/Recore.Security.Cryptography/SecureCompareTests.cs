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
                () => SecureCompare.SafeEquals(null, new byte[] { }));

            Assert.ThrowsException<ArgumentNullException>(
                () => SecureCompare.SafeEquals(new byte[] { }, null));
        }

        [TestMethod]
        public void UnequalLength()
        {
            Assert.IsFalse(SecureCompare.SafeEquals(new byte[] { 0 }, new byte[] { }));
        }

        [TestMethod]
        public void EqualSequences()
        {
            Assert.IsTrue(SecureCompare.SafeEquals(new byte[] { }, new byte[] { }));
            Assert.IsTrue(SecureCompare.SafeEquals(new byte[] { 0 }, new byte[] { 0 }));
            Assert.IsTrue(SecureCompare.SafeEquals(new byte[] { 0, 1, 2, 3 }, new byte[] { 0, 1, 2, 3 }));
        }

        [TestMethod]
        public void UnequalSequences()
        {
            Assert.IsFalse(SecureCompare.SafeEquals(new byte[] { 0 }, new byte[] { 1 }));
            Assert.IsFalse(SecureCompare.SafeEquals(new byte[] { 1 }, new byte[] { 0 }));
        }
    }
}