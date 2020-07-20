using Xunit;

namespace Recore.Text.Tests
{
    public class GlobExpressionTests
    {
        [Fact]
        public void EmptyPattern()
        {
            Assert.True(new GlobExpression("").IsMatch(""));
            Assert.False(new GlobExpression("").IsMatch("a"));
        }

        [Fact]
        public void NoWildcardCharacters()
        {
            Assert.True(new GlobExpression("a").IsMatch("a"));
            Assert.True(new GlobExpression("abc").IsMatch("abc"));
            Assert.False(new GlobExpression("a").IsMatch(""));
            Assert.False(new GlobExpression("abc").IsMatch("ab"));
            Assert.False(new GlobExpression("abc").IsMatch("abcd"));
        }

        [Fact]
        public void JustStar()
        {
            Assert.True(new GlobExpression("*").IsMatch(""));
            Assert.True(new GlobExpression("*").IsMatch("abc"));
            Assert.True(new GlobExpression("*").IsMatch("hello world"));
        }

        [Fact]
        public void StarAtBeginning()
        {
            Assert.True(new GlobExpression("*a").IsMatch("a"));
            Assert.True(new GlobExpression("*a").IsMatch("bba"));
            Assert.False(new GlobExpression("*a").IsMatch("abc"));
        }

        [Fact]
        public void StarInMiddle()
        {
            Assert.False(new GlobExpression("a*b").IsMatch("a"));
            Assert.False(new GlobExpression("a*b").IsMatch("aa"));
            Assert.True(new GlobExpression("a*b").IsMatch("ab"));
            Assert.True(new GlobExpression("a*b").IsMatch("abbbb"));
            Assert.False(new GlobExpression("a*b").IsMatch("abbbbc"));
            Assert.True(new GlobExpression("*ll*").IsMatch("hello world"));
        }

        [Fact]
        public void StarAtEnd()
        {
            Assert.True(new GlobExpression("a*").IsMatch("abc"));
            Assert.True(new GlobExpression("a*").IsMatch("a*"));
            Assert.False(new GlobExpression("a*").IsMatch("bc"));
        }

        [Fact]
        public void MultipleStars()
        {
            Assert.True(new GlobExpression("*ab*").IsMatch("ab"));
            Assert.True(new GlobExpression("*ab*").IsMatch("123ab456"));

            Assert.True(new GlobExpression("*/*/*").IsMatch("foo/bar/baz"));
            Assert.True(new GlobExpression("*/*/*").IsMatch("foo/bar/baz/bash"));
            Assert.True(new GlobExpression("*/*/*").IsMatch("foo/bar/"));
            Assert.False(new GlobExpression("*/*/*").IsMatch("foo/bar"));
        }

        [Fact]
        public void JustQuestionMark()
        {
            Assert.False(new GlobExpression("?").IsMatch(""));
            Assert.True(new GlobExpression("?").IsMatch("x"));
        }

        [Fact]
        public void QuestionMarkInMiddle()
        {
            Assert.True(new GlobExpression("a?c").IsMatch("abc"));
            Assert.True(new GlobExpression("a?c").IsMatch("axc"));
            Assert.True(new GlobExpression("a?c").IsMatch("a?c"));
        }

        [Fact]
        public void MixedWildcards()
        {
            Assert.True(new GlobExpression("?ab*").IsMatch("1ab456"));
            Assert.False(new GlobExpression("?ab*").IsMatch("ab456"));

            Assert.True(new GlobExpression("ab?*").IsMatch("abc"));
            Assert.False(new GlobExpression("ab?*").IsMatch("ab"));
            Assert.True(new GlobExpression("ab?*").IsMatch("abcdef"));

            Assert.True(new GlobExpression("?*/?*/?*").IsMatch("foo/bar/baz"));
            Assert.True(new GlobExpression("?*/?*/?*").IsMatch("foo/bar/baz/bash"));
            Assert.False(new GlobExpression("?*/?*/?*").IsMatch("foo/bar/"));
            Assert.False(new GlobExpression("?*/?*/?*").IsMatch("foo/bar"));
        }

        [Fact]
        public void EscapedWildcardCharacters()
        {
            Assert.True(new GlobExpression("\\").IsMatch("\\"));
            Assert.True(new GlobExpression("\\*").IsMatch("*"));
            Assert.True(new GlobExpression("\\?").IsMatch("?"));
            Assert.True(new GlobExpression("\\\\*").IsMatch("\\*"));
            Assert.False(new GlobExpression("\\*").IsMatch(""));
            Assert.False(new GlobExpression("\\*").IsMatch("a"));
            Assert.False(new GlobExpression("\\*").IsMatch("\\*"));
            Assert.False(new GlobExpression("\\?").IsMatch("a"));
            Assert.False(new GlobExpression("\\?").IsMatch("\\?"));

            Assert.True(new GlobExpression("a\\*").IsMatch("a*"));
            Assert.True(new GlobExpression("a\\?").IsMatch("a?"));
            Assert.False(new GlobExpression("a\\*").IsMatch("a"));
            Assert.False(new GlobExpression("a\\*").IsMatch("ab"));
            Assert.False(new GlobExpression("a\\?").IsMatch("ab"));

            Assert.True(new GlobExpression("*\\*").IsMatch("*"));
            Assert.True(new GlobExpression("a*\\*a").IsMatch("a*a"));
            Assert.True(new GlobExpression("a*\\*a").IsMatch("abc*a"));
            Assert.False(new GlobExpression("*\\*a").IsMatch("aaa"));
        }

        [Fact]
        public void NonescapingBackslash()
        {
            Assert.True(new GlobExpression("\\").IsMatch("\\"));
            Assert.True(new GlobExpression("\\a").IsMatch("\\a"));
            Assert.True(new GlobExpression("*\\a?").IsMatch("hello world\\ab"));
        }
    }
}
