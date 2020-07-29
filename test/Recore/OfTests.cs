using System.Text.Json;
using Xunit;

using Recore.Text.Json.Serialization.Converters;

namespace Recore.Tests
{
    public class OfTests
    {
        [OfJson(typeof(Address), typeof(string))]
        class Address : Of<string>
        {
            public Address() { }
            public Address(string value) => Value = value;
        }

        class House
        {
            public Address Street { get; set; }
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
            var address = new Address("1 Microsoft Way");
            Assert.Equal("\"1 Microsoft Way\"", JsonSerializer.Serialize(address));

            var house = new House
            {
                Street = new Address("123 Main St")
            };

            Assert.Equal("{\"Street\":\"123 Main St\"}", JsonSerializer.Serialize(house));
        }

        [Fact]
        public void FromJson()
        {
            var address = JsonSerializer.Deserialize<Of<string>>("\"1 Microsoft Way\"").To<Address>();
            Assert.Equal(new Address("1 Microsoft Way"), address);

            address = JsonSerializer.Deserialize<Address>("\"1 Microsoft Way\"");
            Assert.Equal(new Address("1 Microsoft Way"), address);

            var house = JsonSerializer.Deserialize<House>("{\"Street\":\"123 Main St\"}");
            Assert.Equal(new Address("123 Main St"), house.Street);
        }
    }
}