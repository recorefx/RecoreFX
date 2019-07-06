using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void Constructor()
        {
            var referenceOptional = new Optional<string>("Hello world");
            Assert.IsTrue(referenceOptional.HasValue);

            var valueOptional = new Optional<int>(123);
            Assert.IsTrue(valueOptional.HasValue);

            var nullOptional = new Optional<object>(null);
            Assert.IsFalse(nullOptional.HasValue);
        }

        [TestMethod]
        public void DefaultConstructor()
        {
            var referenceOptional = new Optional<object>();
            Assert.IsFalse(referenceOptional.HasValue);

            var valueOptional = new Optional<int>();
            Assert.IsFalse(valueOptional.HasValue);
        }

        [TestMethod]
        public void Empty()
        {
            var referenceOptional = Optional.Empty<object>();
            Assert.IsFalse(referenceOptional.HasValue);

            var valueOptional = Optional.Empty<int>();
            Assert.IsFalse(valueOptional.HasValue);
        }

        [TestMethod]
        public void SwitchFunc()
        {
            Optional<int> optional;
            int result;

            optional = 123;
            result = optional.Switch(
                x => 1,
                () => throw new Exception("Should not be called"));
            Assert.AreEqual(1, result);

            optional = Optional.Empty<int>();
            result = optional.Switch(
                x => throw new Exception("Should not be called"),
                () => 10);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void SwitchAction()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.Switch(
                x => { called = true; },
                () => throw new Exception("Should not be called"));
            Assert.IsTrue(called);

            optional = Optional.Empty<int>();
            called = false;
            optional.Switch(
                x => throw new Exception("Should not be called"),
                () => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ValueOr()
        {
            Optional<string> optional;

            optional = "hello world";
            Assert.AreEqual("hello world", optional.ValueOr("xxx"));

            optional = null;
            Assert.AreEqual("xxx", optional.ValueOr("xxx"));
        }

        [TestMethod]
        public void OnValue()
        {
            Optional<int> optional;
            int Square(int n) => n * n;

            optional = 10;
            Assert.AreEqual(100, optional.OnValue(Square));

            optional = Optional.Empty<int>();
            Assert.IsFalse(optional.OnValue(Square).HasValue);

            optional = 100;
            Assert.AreEqual("100", optional.OnValue(x => x.ToString()));
        }

        [TestMethod]
        public void IfValueIfEmpty()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.IsTrue(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.IsFalse(called);

            optional = Optional.Empty<int>();
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.IsFalse(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void FunctorLaws()
        {
            // Identity
            Optional<string> referenceOptional = "hello world";
            Optional<int> valueOptional = 123;
            T Id<T>(T t) => t;

            Assert.AreEqual("hello world", referenceOptional.OnValue(Id));
            Assert.AreEqual(123, valueOptional.OnValue(Id));
            Assert.AreEqual(Optional.Empty<object>(), Optional.Empty<object>().OnValue(Id));

            // Composition
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;

            Assert.AreEqual(
                new Optional<int>(10).OnValue(Square).OnValue(PlusTwo),
                new Optional<int>(10).OnValue(x => PlusTwo(Square(x))));

            Assert.AreEqual(
                Optional.Empty<int>().OnValue(Square).OnValue(PlusTwo),
                Optional.Empty<int>().OnValue(x => PlusTwo(Square(x))));
        }

        [TestMethod]
        public void Then()
        {
            Optional<int> FindFirstSpace(string str)
            {
                var result = str.IndexOf(' ');
                if (result == -1)
                {
                    return Optional.Empty<int>();
                }
                else
                {
                    return result;
                }
            }

            Optional<string> optionalString;
            Optional<int> actual;

            optionalString = "hello world";
            actual = optionalString.Then(FindFirstSpace);
            Assert.AreEqual(5, actual);

            optionalString = "hello";
            actual = optionalString.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);

            optionalString = Optional.Empty<string>();
            actual = optionalString.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void MonadLaws()
        {
            Optional<string> StringToOption(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return Optional.Empty<string>();
                }
                else
                {
                    return str;
                }
            }

            // Left identity
            foreach (var value in new[] { "hello", string.Empty, null })
            {
                Assert.AreEqual(
                    StringToOption(value),
                    new Optional<string>(value).Then(StringToOption));
            }

            // Right identity
            foreach (var optional in new[] { "hello", null, Optional.Empty<string>() })
            {
                Assert.AreEqual(
                    optional,
                    optional.Then(Optional.Of<string>));
            }

            // Associativity
            Optional<int> NullsafeLength(string str)
            {
                if (str == null)
                {
                    return Optional.Empty<int>();
                }
                else
                {
                    return str.Length;
                }
            }

            foreach (var optional in new[] { "hello", null, Optional.Empty<string>() })
            {
                Assert.AreEqual(
                    optional.Then(StringToOption).Then(NullsafeLength),
                    optional.Then(x => StringToOption(x).Then(NullsafeLength)));
            }
        }

        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("hello", Optional.Of("hello").ToString());
            Assert.AreEqual("none", Optional.Empty<string>().ToString());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void Equals(int value)
        {
            // object.Equals
            Assert.IsTrue(
                Equals(Optional.Of(value), Optional.Of(value)));

            Assert.IsTrue(
                Equals(new Optional<string>(), new Optional<string>()));

            Assert.IsFalse(
                Equals(Optional.Of(value), Optional.Empty<string>()));

            // Optional.Equals
            Assert.IsTrue(
                Optional.Of(value).Equals(Optional.Of(value)));

            Assert.IsTrue(
                Optional.Empty<int>().Equals(Optional.Empty<int>()));

            Assert.IsFalse(
                Optional.Empty<int>().Equals(Optional.Of(value)));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void EqualityOperators(int value)
        {
            // operator==
            Assert.IsTrue(
                Optional.Of(value) == Optional.Of(value));

            Assert.IsTrue(
                new Optional<int>() == new Optional<int>());

            Assert.IsFalse(
                Optional.Of(0) == Optional.Empty<int>());

            // operator!=
            Assert.IsFalse(
                Optional.Of(value) != Optional.Of(value));

            Assert.IsFalse(
                new Optional<int>() != new Optional<int>());

            Assert.IsTrue(
                Optional.Of(value) != Optional.Empty<int>());
        }

        [TestMethod]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            Optional<int> a = 1;
            Assert.AreEqual(a, a);

            // Symmetric
            Optional<int> b = 1;
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);

            // Transitive
            Optional<int> c = 1;
            Assert.AreEqual(b, c);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public new void GetHashCode()
        {
            Optional<string> optional = "hello";

            // Idempotence
            Assert.AreEqual(optional.GetHashCode(), optional.GetHashCode());

            // Does not depend on object identity
            Assert.AreEqual(optional.GetHashCode(), Optional.Of("hello").GetHashCode());
        }

        [TestMethod]
        public void GetEnumerator()
        {
            {
                var optional = Optional.Of(123);
                var enumerator = optional.GetEnumerator();

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(123, enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());

                var numberOfElements = 0;
                foreach (var item in optional)
                {
                    Assert.AreEqual(123, item);
                    numberOfElements++;
                }

                Assert.AreEqual(1, numberOfElements);
            }
            {
                var optional = Optional.Empty<int>();
                var enumerator = optional.GetEnumerator();

                Assert.IsFalse(enumerator.MoveNext());

                foreach (var item in optional)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Cast()
        {
            int CountChar(string str, char toCount)
            {
                int total = 0;
                foreach (char c in str)
                {
                    if (c == toCount)
                    {
                        total++;
                    }
                }
                return total;
            }

            Optional<string> optional;

            optional = "hello";
            Assert.AreEqual(2, CountChar((string)optional, 'l'));

            optional = Optional.Empty<string>();
            Assert.IsNull((string)optional);
        }

        [TestMethod]
        public void Of()
        {
            Assert.AreEqual(new Optional<int>(12), Optional.Of(12));
        }

        [TestMethod]
        public void Flatten()
        {
            var doubleValue = new Optional<Optional<string>>("hello");
            Assert.AreEqual("hello", doubleValue.Flatten());

            var doubleNone = new Optional<Optional<string>>();
            Assert.IsFalse(doubleNone.Flatten().HasValue);

            var valueNone = new Optional<Optional<string>>(Optional.Empty<string>());
            Assert.IsFalse(valueNone.Flatten().HasValue);
        }

        [TestMethod]
        public void NonEmpty()
        {
            var collection = new[]
            {
                "hello",
                Optional.Empty<string>(),
                "abc",
                "hello world",
                Optional.Empty<string>()
            };

            var nonempty = new[]
            {
                "hello",
                "abc",
                "hello world"
            };

            CollectionAssert.AreEqual(
                nonempty,
                collection.NonEmpty().ToArray());
        }
    }
}
