using System.Text.Json;
using Xunit;

namespace Recore.Tests
{
    public class OfTests
    {
        class Address : Of<string>
        {
            public Address() { }
            public Address(string value) => Value = value;
        }

        [OfJson(typeof(JsonAddress), typeof(string))]
        class JsonAddress : Of<string>
        {
            public JsonAddress() { }
            public JsonAddress(string value) => Value = value;
        }

        class House
        {
            public JsonAddress Street { get; set; }
        }

        [Fact]
        public void Equals_()
        {
            var address1 = new Address("1 Microsoft Way");
            var address2 = new Address("1 Microsoft Way");
            Assert.Equal(address1, address2);

            var address3 = new Address("1 Infinite Loop");
            Assert.NotEqual(address1, address3);
        }

        [Fact]
        public void EqualsWithNull()
        {
            var address1 = new Address("1 Microsoft Way");
            Address address2 = null;

            Assert.NotEqual(address1, address2);
        }

        [Fact]
        public void ToJson()
        {
            Assert.Equal(
                expected: "{\"Value\":\"1 Microsoft Way\"}",
                actual: JsonSerializer.Serialize(new Address("1 Microsoft Way")));

            Assert.Equal(
                expected: "\"1 Microsoft Way\"",
                actual: JsonSerializer.Serialize(new JsonAddress("1 Microsoft Way")));

            Assert.Equal(
                expected: "{\"Street\":\"123 Main St\"}",
                actual: JsonSerializer.Serialize(new House
                {
                    Street = new JsonAddress("123 Main St")
                }));
        }

        [Fact]
        public void FromJson()
        {
            // This throws because `Address` does not have `[OfJson(...)]`
            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Address>("\"1 Microsoft Way\""));

            Assert.Equal(
                expected: new Address("1 Microsoft Way"),
                actual: JsonSerializer.Deserialize<Address>("{\"Value\":\"1 Microsoft Way\"}"));

            Assert.Equal(
                expected: new JsonAddress("1 Microsoft Way"),
                actual: JsonSerializer
                    .Deserialize<Of<string>>("\"1 Microsoft Way\"")
                    .To<JsonAddress>());

            Assert.Equal(
                expected: new JsonAddress("1 Microsoft Way"),
                actual: JsonSerializer.Deserialize<JsonAddress>("\"1 Microsoft Way\""));

            Assert.Equal(
                expected: new JsonAddress("123 Main St"),
                actual: JsonSerializer.Deserialize<House>("{\"Street\":\"123 Main St\"}").Street);
        }
    }
}