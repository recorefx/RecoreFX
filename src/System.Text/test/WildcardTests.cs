using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Text.UnitTest
{
    [TestClass]
    public class WildcardTests
    {
        [TestMethod]
        public void EmptyPattern()
        {
            Assert.IsTrue(new Wildcard("").IsMatch(""));
            Assert.IsFalse(new Wildcard("").IsMatch("a"));
        }

        [TestMethod]
        public void NoSpecialCharacters()
        {
            Assert.IsTrue(new Wildcard("abc").IsMatch("abc"));
            Assert.IsFalse(new Wildcard("abc").IsMatch("ab"));
            Assert.IsFalse(new Wildcard("abc").IsMatch("abcd"));
        }

        [TestMethod]
        public void JustAsterisk()
        {
            Assert.IsTrue(new Wildcard("*").IsMatch(""));
            Assert.IsTrue(new Wildcard("*").IsMatch("abc"));
        }

        [TestMethod]
        public void JustQuestionMark()
        {
            Assert.IsFalse(new Wildcard("?").IsMatch(""));
            Assert.IsTrue(new Wildcard("?").IsMatch("x"));
        }

        [TestMethod]
        public void SingleWildcard()
        {
            Assert.IsTrue(new Wildcard("a?c").IsMatch("axc"));
        }

        [TestMethod]
        public void AsteriskAtEnd()
        {
            Assert.IsTrue(new Wildcard("a*").IsMatch("abc"));
            Assert.IsFalse(new Wildcard("a*").IsMatch("bc"));
        }

        [TestMethod]
        public void AsteriskInMiddle()
        {
            Assert.IsFalse(new Wildcard("a*b").IsMatch("a"));
            Assert.IsFalse(new Wildcard("a*b").IsMatch("aa"));
            Assert.IsTrue(new Wildcard("a*b").IsMatch("ab"));
            Assert.IsTrue(new Wildcard("a*b").IsMatch("abbbb"));
            Assert.IsFalse(new Wildcard("a*b").IsMatch("abbbbc"));
        }

        [TestMethod]
        public void MultipleAsterisks()
        {
            Assert.IsTrue(new Wildcard("*ab*").IsMatch("ab"));
            Assert.IsTrue(new Wildcard("*ab*").IsMatch("123ab456"));

            Assert.IsTrue(new Wildcard("*/*/*").IsMatch("foo/bar/baz"));
            Assert.IsTrue(new Wildcard("*/*/*").IsMatch("foo/bar/baz/bash"));
            Assert.IsTrue(new Wildcard("*/*/*").IsMatch("foo/bar/"));
            Assert.IsFalse(new Wildcard("*/*/*").IsMatch("foo/bar"));
        }

        [TestMethod]
        public void MixedWildcards()
        {
            Assert.IsTrue(new Wildcard("?ab*").IsMatch("1ab456"));
            Assert.IsFalse(new Wildcard("?ab*").IsMatch("ab456"));

            Assert.IsTrue(new Wildcard("ab?*").IsMatch("abc"));
            Assert.IsFalse(new Wildcard("ab?*").IsMatch("ab"));
            Assert.IsTrue(new Wildcard("ab?*").IsMatch("abcdef"));

            Assert.IsTrue(new Wildcard("?*/?*/?*").IsMatch("foo/bar/baz"));
            Assert.IsTrue(new Wildcard("?*/?*/?*").IsMatch("foo/bar/baz/bash"));
            Assert.IsFalse(new Wildcard("?*/?*/?*").IsMatch("foo/bar/"));
            Assert.IsFalse(new Wildcard("?*/?*/?*").IsMatch("foo/bar"));
        }
    }
}
