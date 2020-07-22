using Xunit;

namespace Recore.Tests
{
    public class OfTests
    {
        class Address : Of<string>
        {
            public Address(string value) => Value = value;
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
    }
}