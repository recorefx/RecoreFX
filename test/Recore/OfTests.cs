using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class OfTests
    {
        class Address : Of<string> {}

        [TestMethod]
        public void Equals()
        {
            var address1 = new Address { Value = "1 Microsoft Way" };
            var address2 = new Address { Value = "1 Microsoft Way" };
            Assert.AreEqual(address1, address2);

            var address3 = new Address { Value = "1 Infinite Loop" };
            Assert.AreNotEqual(address1, address3);
        }

        [TestMethod]
        public void EqualsWithNull()
        {
            var address1 = new Address { Value = "1 Microsoft Way" };
            Address address2 = null;

            Assert.AreNotEqual(address1, address2);
        }
    }
}