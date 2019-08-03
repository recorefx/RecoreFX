using Xunit;

namespace Recore.Text.Tests
{
    public class WildcardTests
    {
        [Fact]
        public void EmptyPattern()
        {
            Assert.True(new Wildcard("").IsMatch(""));
            Assert.False(new Wildcard("").IsMatch("a"));
        }

        [Fact]
        public void NoSpecialCharacters()
        {
            Assert.True(new Wildcard("abc").IsMatch("abc"));
            Assert.False(new Wildcard("abc").IsMatch("ab"));
            Assert.False(new Wildcard("abc").IsMatch("abcd"));
        }

        [Fact]
        public void JustAsterisk()
        {
            Assert.True(new Wildcard("*").IsMatch(""));
            Assert.True(new Wildcard("*").IsMatch("abc"));
        }

        [Fact]
        public void JustQuestionMark()
        {
            Assert.False(new Wildcard("?").IsMatch(""));
            Assert.True(new Wildcard("?").IsMatch("x"));
        }

        [Fact]
        public void SingleWildcard()
        {
            Assert.True(new Wildcard("a?c").IsMatch("axc"));
        }

        [Fact]
        public void AsteriskAtEnd()
        {
            Assert.True(new Wildcard("a*").IsMatch("abc"));
            Assert.False(new Wildcard("a*").IsMatch("bc"));
        }

        [Fact]
        public void AsteriskInMiddle()
        {
            Assert.False(new Wildcard("a*b").IsMatch("a"));
            Assert.False(new Wildcard("a*b").IsMatch("aa"));
            Assert.True(new Wildcard("a*b").IsMatch("ab"));
            Assert.True(new Wildcard("a*b").IsMatch("abbbb"));
            Assert.False(new Wildcard("a*b").IsMatch("abbbbc"));
        }

        [Fact]
        public void MultipleAsterisks()
        {
            Assert.True(new Wildcard("*ab*").IsMatch("ab"));
            Assert.True(new Wildcard("*ab*").IsMatch("123ab456"));

            Assert.True(new Wildcard("*/*/*").IsMatch("foo/bar/baz"));
            Assert.True(new Wildcard("*/*/*").IsMatch("foo/bar/baz/bash"));
            Assert.True(new Wildcard("*/*/*").IsMatch("foo/bar/"));
            Assert.False(new Wildcard("*/*/*").IsMatch("foo/bar"));
        }

        [Fact]
        public void MixedWildcards()
        {
            Assert.True(new Wildcard("?ab*").IsMatch("1ab456"));
            Assert.False(new Wildcard("?ab*").IsMatch("ab456"));

            Assert.True(new Wildcard("ab?*").IsMatch("abc"));
            Assert.False(new Wildcard("ab?*").IsMatch("ab"));
            Assert.True(new Wildcard("ab?*").IsMatch("abcdef"));

            Assert.True(new Wildcard("?*/?*/?*").IsMatch("foo/bar/baz"));
            Assert.True(new Wildcard("?*/?*/?*").IsMatch("foo/bar/baz/bash"));
            Assert.False(new Wildcard("?*/?*/?*").IsMatch("foo/bar/"));
            Assert.False(new Wildcard("?*/?*/?*").IsMatch("foo/bar"));
        }
    }
}
