using System.Linq;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class EitherConverterTests
    {
        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        class Address
        {
            public string Street { get; set; }
            public string Zip { get; set; }
        }

        class PersonAddress
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Street { get; set; }
            public string Zip { get; set; }

            public Either<Person, Address> ToEither()
            {
                if (Name != null)
                {
                    return new Person
                    {
                        Name = Name,
                        Age = Age
                    };
                }
                else
                {
                    return new Address
                    {
                        Street = Street,
                        Zip = Zip
                    };
                }
            }
        }

        [Fact]
        public void ToJson()
        {
            {
                Either<int, string> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<string, int> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<string, Person> either;

                either = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<Person, string> either;

                either = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(either));
            }
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: new Either<int, string>(12),
                actual: JsonSerializer.Deserialize<Either<int, string>>("12"));

            Assert.Equal(
                expected: new Either<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Either<int, string>>("\"hello\""));

            Assert.Equal(
                expected: new Either<string, int>(12),
                actual: JsonSerializer.Deserialize<Either<string, int>>("12"));

            Assert.Equal(
                expected: new Either<string, int>("hello"),
                actual: JsonSerializer.Deserialize<Either<string, int>>("\"hello\""));

            var deserializedPerson = JsonSerializer.Deserialize<Either<Person, string>>("{\"Name\":\"Mario\",\"Age\":42}");
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetLeft().First().Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetLeft().First().Age);
        }

        [Fact(Skip = "https://github.com/recorefx/RecoreFX/issues/114")]
        public void FromJsonBothRecordTypes()
        {
            var deserializedPerson = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Name\":\"Mario\",\"Age\":42}");
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetLeft().First().Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetLeft().First().Age);

            var deserializedAddress = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}");
            Assert.Equal(
                expected: "123 Main St",
                actual: deserializedAddress.GetRight().First().Street);

            Assert.Equal(
                expected: "12345",
                actual: deserializedAddress.GetRight().First().Zip);
        }

        [Fact]
        public void FromJsonBothRecordTypesWorkaround()
        {
            var deserializedPerson = JsonSerializer.Deserialize<PersonAddress>("{\"Name\":\"Mario\",\"Age\":42}").ToEither();
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetLeft().First().Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetLeft().First().Age);

            var deserializedAddress = JsonSerializer.Deserialize<PersonAddress>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}").ToEither();
            Assert.Equal(
                expected: "123 Main St",
                actual: deserializedAddress.GetRight().First().Street);

            Assert.Equal(
                expected: "12345",
                actual: deserializedAddress.GetRight().First().Zip);
        }
    }
}
