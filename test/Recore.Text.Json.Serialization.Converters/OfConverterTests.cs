using System.Security.Cryptography;
using System.Text.Json;
using Xunit;

using Recore.Security.Cryptography;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class OfConverterTests
    {
        enum SomeEnum { Yes, No }
        class DerivedOfString : Of<string> { }
        class DerivedDerivedOfString : Of<string> { }

        class StreetAddress : Of<string>
        {
            public StreetAddress() { }
            public StreetAddress(string value) => Value = value;
        }

        [OfJson(typeof(JsonStreetAddress), typeof(string))]
        class JsonStreetAddress : Of<string>
        {
            public JsonStreetAddress() { }
            public JsonStreetAddress(string value) => Value = value;
        }

        class House
        {
            public JsonStreetAddress Street { get; set; }
        }

        [OfJson(typeof(User), typeof(Person))]
        class User : Of<Person> { }

        [Fact]
        public void CanConvert()
        {
            var factory = new OfConverter();

            Assert.True(factory.CanConvert(typeof(Of<>)));
            Assert.True(factory.CanConvert(typeof(Of<string>)));
            Assert.True(factory.CanConvert(typeof(Of<int>)));
            Assert.True(factory.CanConvert(typeof(Of<SomeEnum>)));
            Assert.True(factory.CanConvert(typeof(Of<Of<object>>)));
            Assert.True(factory.CanConvert(typeof(Token)));
            Assert.True(factory.CanConvert(typeof(Ciphertext<SHA1>)));
            Assert.True(factory.CanConvert(typeof(DerivedOfString)));
            Assert.True(factory.CanConvert(typeof(DerivedDerivedOfString)));

            Assert.False(factory.CanConvert(typeof(string)));
            Assert.False(factory.CanConvert(typeof(object)));
        }

        [Fact]
        public void CreateConverter()
        {
            var factory = new OfConverter();
            var options = new JsonSerializerOptions();

            Assert.IsType<OfConverter<string>>(factory.CreateConverter(typeof(Of<string>), options));
            Assert.IsType<OfConverter<string>>(factory.CreateConverter(typeof(DerivedOfString), options));
        }

        [Fact]
        public void ToJson()
        {
            Assert.Equal(
                expected: "{\"Value\":\"1 Microsoft Way\"}",
                actual: JsonSerializer.Serialize(new StreetAddress("1 Microsoft Way")));

            Assert.Equal(
                expected: "\"1 Microsoft Way\"",
                actual: JsonSerializer.Serialize(new JsonStreetAddress("1 Microsoft Way")));

            Assert.Equal(
                expected: "{\"Street\":\"123 Main St\"}",
                actual: JsonSerializer.Serialize(new House
                {
                    Street = new JsonStreetAddress("123 Main St")
                }));

            Assert.Equal(
                expected: "{\"Name\":\"Mario\",\"Age\":42}",
                actual: JsonSerializer.Serialize(new User
                {
                    Value = new Person { Name = "Mario", Age = 42 }
                }));
        }

        [Fact]
        public void FromJson()
        {
            // This throws because `Address` does not have `[OfJson(...)]`
            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<StreetAddress>("\"1 Microsoft Way\""));

            Assert.Equal(
                expected: new StreetAddress("1 Microsoft Way"),
                actual: JsonSerializer.Deserialize<StreetAddress>("{\"Value\":\"1 Microsoft Way\"}"));

            Assert.Equal(
                expected: new JsonStreetAddress("1 Microsoft Way"),
                actual: JsonSerializer
                    .Deserialize<Of<string>>("\"1 Microsoft Way\"")
                    .To<JsonStreetAddress>());

            Assert.Equal(
                expected: new JsonStreetAddress("1 Microsoft Way"),
                actual: JsonSerializer.Deserialize<JsonStreetAddress>("\"1 Microsoft Way\""));

            Assert.Equal(
                expected: new JsonStreetAddress("123 Main St"),
                actual: JsonSerializer.Deserialize<House>("{\"Street\":\"123 Main St\"}").Street);

            var deserializedUser = JsonSerializer.Deserialize<User>("{\"Name\":\"Mario\",\"Age\":42}");
            Assert.Equal(
                expected: "Mario",
                actual: deserializedUser.Value.Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedUser.Value.Age);
        }
    }
}
