using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Close()
        {
            bool called = false;

            var result = Unit.Close(() =>
            {
                called = true;
            })();

            Assert.IsTrue(called);
            Assert.AreEqual(new Unit(), result);
        }
    }
}