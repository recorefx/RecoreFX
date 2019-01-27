using System;
using System.Collections.Generic;
using System.Linq;

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
        public void ConvertToString()
        {
            Assert.AreEqual("hello", new Token("hello").ToString());
            Assert.AreNotEqual("asdf", new Token("hello").ToString());
        }

        [TestMethod]
        public void Uninitialized()
        {
            Token token;
            Assert.ThrowsException<UninitializedStructException<Token>>(token.ToString);
        }

        [TestMethod]
        public void IEquatableEquals()
        {
            // reflexive
            var helloworld1 = new Token("helloworld");
            Assert.IsTrue(helloworld1.Equals(helloworld1));

            // symmetric
            var helloworld2 = new Token("hello" + "world");
            Assert.IsTrue(helloworld1.Equals(helloworld2));
            Assert.IsTrue(helloworld2.Equals(helloworld1));

            // transitive
            var helloworld3 = new Token("he" + "lloworld");
            Assert.IsTrue(helloworld2.Equals(helloworld3));
            Assert.IsTrue(helloworld1.Equals(helloworld3));

            // not equal
            var hello = new Token("hello");
            Assert.IsFalse(helloworld1.Equals(hello));
            Assert.IsFalse(hello.Equals(helloworld1));
        }

        [TestMethod]
        public void ObjectEquals()
        {
            var helloworld1 = new Token("helloworld");
            var helloworld2 = new Token("hello" + "world");
            Assert.IsTrue(helloworld1.Equals((object)helloworld2));

            Assert.IsTrue(helloworld1.Equals("helloworld"));
            Assert.IsTrue("helloworld".Equals(helloworld1));
            Assert.IsFalse(helloworld1.Equals(new Exception()));

            Assert.IsTrue(Equals(helloworld1, helloworld2));
            Assert.IsTrue(Equals(helloworld1, "helloworld"));

            // string.Equals
            Assert.IsTrue(string.Equals("hello", new Token("HELLO"), StringComparison.InvariantCultureIgnoreCase));

            // operator==
            Assert.IsTrue(helloworld1 == helloworld2);
            Assert.IsTrue(helloworld2 == helloworld1);

            Assert.IsTrue(helloworld1 == "helloworld");
            Assert.IsTrue("helloworld" == helloworld1);

            var asdf = new Token("asdf");
            Assert.IsFalse(helloworld1 == asdf);
            Assert.IsTrue(helloworld1 != asdf);
        }

        [TestMethod]
        public void ImplicitConversionToString()
        {
            var world = new Token("world");
            var message = "hello " + world;
            Assert.AreEqual("hello world", message);

            var dictionary = new Dictionary<string, int>
            {
                ["burrito"] = 1,
                ["enchilada"] = 2,
                ["chalupa"] = 3
            };

            Assert.AreEqual(1, dictionary[new Token("burrito")]);
        }

        [TestMethod]
        public void Hashable()
        {
            var dictionary = new Dictionary<Token, int>
            {
                [new Token("burrito")] = 1,
                [new Token("enchilada")] = 2,
                [new Token("chalupa")] = 3
            };

            Assert.AreEqual(2, dictionary[new Token("enchil" + "ada")]);
            Assert.IsFalse(dictionary.ContainsKey(new Token("taco")));
        }

        [TestMethod]
        public void Comparable()
        {
            Assert.AreEqual(-1, new Token("abc").CompareTo(new Token("def")));
            Assert.AreEqual(0, new Token("abc").CompareTo(new Token("abc")));
            Assert.AreEqual(1, new Token("def").CompareTo(new Token("abc")));

            Assert.AreEqual(-1, "abc".CompareTo(new Token("def")));
            Assert.AreEqual(0, "abc".CompareTo(new Token("abc")));
            Assert.AreEqual(1, "def".CompareTo(new Token("abc")));
        }

        [TestMethod]
        public void Tokenize()
        {
            var gettysburg = @"Four score and seven years ago,
            our fathers brought forth on this continent a new nation";

            var tokens = new[]
            {
                "Four", "score", "and", "seven", "years", "ago,",
                "our", "fathers", "brought", "forth", "on", "this", "continent", "a", "new", "nation"
            }.Select(x => new Token(x));

            Assert.IsTrue(tokens.SequenceEqual(gettysburg.Tokenize()));

            Assert.AreEqual(0, string.Empty.Tokenize().Length);

            var doubleSpace = "    hello  world    ";
            tokens = new[] { new Token("hello"), new Token("world") };
            Assert.IsTrue(tokens.SequenceEqual(doubleSpace.Tokenize()));
        }
    }
}