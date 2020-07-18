using Xunit;

namespace Recore.Text.Tests
{
    public class WildcardTests
    {
        [Fact]
        public void EmptyPattern()
        {
            Assert.True(new Glob("").IsMatch(""));
            Assert.False(new Glob("").IsMatch("a"));
        }

        [Fact]
        public void NoSpecialCharacters()
        {
            Assert.True(new Glob("abc").IsMatch("abc"));
            Assert.False(new Glob("abc").IsMatch("ab"));
            Assert.False(new Glob("abc").IsMatch("abcd"));
        }

        [Fact]
        public void JustAsterisk()
        {
            Assert.True(new Glob("*").IsMatch(""));
            Assert.True(new Glob("*").IsMatch("abc"));
        }

        [Fact]
        public void JustQuestionMark()
        {
            Assert.False(new Glob("?").IsMatch(""));
            Assert.True(new Glob("?").IsMatch("x"));
        }

        [Fact]
        public void SingleWildcard()
        {
            Assert.True(new Glob("a?c").IsMatch("axc"));
        }

        [Fact]
        public void AsteriskAtEnd()
        {
            Assert.True(new Glob("a*").IsMatch("abc"));
            Assert.False(new Glob("a*").IsMatch("bc"));
        }

        [Fact]
        public void AsteriskInMiddle()
        {
            Assert.False(new Glob("a*b").IsMatch("a"));
            Assert.False(new Glob("a*b").IsMatch("aa"));
            Assert.True(new Glob("a*b").IsMatch("ab"));
            Assert.True(new Glob("a*b").IsMatch("abbbb"));
            Assert.False(new Glob("a*b").IsMatch("abbbbc"));
        }

        [Fact]
        public void MultipleAsterisks()
        {
            Assert.True(new Glob("*ab*").IsMatch("ab"));
            Assert.True(new Glob("*ab*").IsMatch("123ab456"));

            Assert.True(new Glob("*/*/*").IsMatch("foo/bar/baz"));
            Assert.True(new Glob("*/*/*").IsMatch("foo/bar/baz/bash"));
            Assert.True(new Glob("*/*/*").IsMatch("foo/bar/"));
            Assert.False(new Glob("*/*/*").IsMatch("foo/bar"));
        }

        [Fact]
        public void MixedWildcards()
        {
            Assert.True(new Glob("?ab*").IsMatch("1ab456"));
            Assert.False(new Glob("?ab*").IsMatch("ab456"));

            Assert.True(new Glob("ab?*").IsMatch("abc"));
            Assert.False(new Glob("ab?*").IsMatch("ab"));
            Assert.True(new Glob("ab?*").IsMatch("abcdef"));

            Assert.True(new Glob("?*/?*/?*").IsMatch("foo/bar/baz"));
            Assert.True(new Glob("?*/?*/?*").IsMatch("foo/bar/baz/bash"));
            Assert.False(new Glob("?*/?*/?*").IsMatch("foo/bar/"));
            Assert.False(new Glob("?*/?*/?*").IsMatch("foo/bar"));
        }
    }
}
