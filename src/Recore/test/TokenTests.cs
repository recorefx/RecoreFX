using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void NullNotAllowed()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Token(null));
        }

        [TestMethod]
        public void EmptyStringNotAllowed()
        {
            Assert.ThrowsException<ArgumentException>(() => new Token(string.Empty));
        }

        [TestMethod]
        public void WhitespaceNotAllowed()
        {
            Assert.ThrowsException<ArgumentException>(() => new Token("  hello"));
            Assert.ThrowsException<ArgumentException>(() => new Token("hello  "));
            Assert.ThrowsException<ArgumentException>(() => new Token("hello  world"));
            Assert.ThrowsException<ArgumentException>(() => new Token("new\nline"));
            Assert.ThrowsException<ArgumentException>(() => new Token("tab\ttab"));
        }

        [TestMethod]
        public void ValueIsAsExpected()
        {
            Assert.AreEqual("hello", new Token("hello").Value);
        }
    }
}