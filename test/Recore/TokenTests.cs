using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Tests
{
    public class TokenTests
    {
        [Fact]
        public void NullNotAllowed()
        {
            Assert.Throws<ArgumentNullException>(() => new Token(null!));
        }

        [Fact]
        public void EmptyStringNotAllowed()
        {
            Assert.Throws<ArgumentException>(() => new Token(string.Empty));
        }

        [Fact]
        public void WhitespaceNotAllowed()
        {
            Assert.Throws<ArgumentException>(() => new Token("  hello"));
            Assert.Throws<ArgumentException>(() => new Token("hello  "));
            Assert.Throws<ArgumentException>(() => new Token("hello  world"));
            Assert.Throws<ArgumentException>(() => new Token("new\nline"));
            Assert.Throws<ArgumentException>(() => new Token("tab\ttab"));
        }

        [Fact]
        public void Value()
        {
            Assert.Equal("hello", new Token("hello"));
            Assert.NotEqual("asdf", new Token("hello"));
        }

        [Fact]
        public void ToString_()
        {
            Assert.Equal("hello", new Token("hello").ToString());
            Assert.NotEqual("asdf", new Token("hello").ToString());
        }

        [Fact]
        public void IEquatableEquals()
        {
            // reflexive
            var helloworld1 = new Token("helloworld");
            Assert.True(helloworld1.Equals(helloworld1));

            // symmetric
            var helloworld2 = new Token("hello" + "world");
            Assert.True(helloworld1.Equals(helloworld2));
            Assert.True(helloworld2.Equals(helloworld1));

            // transitive
            var helloworld3 = new Token("he" + "lloworld");
            Assert.True(helloworld2.Equals(helloworld3));
            Assert.True(helloworld1.Equals(helloworld3));

            // not equal
            var hello = new Token("hello");
            Assert.False(helloworld1.Equals(hello));
            Assert.False(hello.Equals(helloworld1));
            Assert.False(helloworld1.Equals(null));
        }

        [Fact]
        public void ObjectEquals()
        {
            var helloworld1 = new Token("helloworld");
            var helloworld2 = new Token("hello" + "world");
            Assert.True(helloworld1.Equals((object)helloworld2));

            Assert.False(helloworld1.Equals("helloworld"));
            Assert.False(helloworld1.Equals(new Exception()));

            Assert.True(Equals(helloworld1, helloworld2));
            Assert.False(Equals(helloworld1, "helloworld"));
        }

        [Fact]
        public void EqualsWithStringComparison()
        {
            // Localized string comparison actually depends on the underlying OS
            // to do the collation, but case sensitivity should be pretty standard.
            var lowercase = new Token("abc");
            var uppercase = new Token("ABC");

            Assert.False(lowercase.Equals(uppercase));
            Assert.False(lowercase.Equals(uppercase, StringComparison.Ordinal));
            Assert.True(lowercase.Equals(uppercase, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Hashable()
        {
            var dictionary = new Dictionary<Token, int>
            {
                [new Token("burrito")] = 1,
                [new Token("enchilada")] = 2,
                [new Token("chalupa")] = 3
            };

            Assert.Equal(2, dictionary[new Token("enchil" + "ada")]);
            Assert.False(dictionary.ContainsKey(new Token("taco")));
        }

        [Fact]
        public void Comparable()
        {
            Assert.Equal(-1, new Token("abc").CompareTo(new Token("def")));
            Assert.Equal(0, new Token("abc").CompareTo(new Token("abc")));
            Assert.Equal(1, new Token("def").CompareTo(new Token("abc")));

            Assert.Equal(1, new Token("abc").CompareTo(null));
        }

        [Fact]
        public void Tokenize()
        {
            var gettysburg = @"Four score and seven years ago,
            our fathers brought forth on this continent a new nation";

            var tokens = new[]
            {
                "Four", "score", "and", "seven", "years", "ago,",
                "our", "fathers", "brought", "forth", "on", "this", "continent", "a", "new", "nation"
            }.Select(x => new Token(x));

            Assert.True(tokens.SequenceEqual(gettysburg.Tokenize()));

            Assert.Empty(string.Empty.Tokenize());

            var doubleSpace = "    hello  world    ";
            tokens = new[] { new Token("hello"), new Token("world") };
            Assert.True(tokens.SequenceEqual(doubleSpace.Tokenize()));
        }
    }
}