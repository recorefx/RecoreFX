using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    internal class Foo
    {
        public Required<string> name;
    }

    [TestClass]
    public class RequiredTests
    {
        [TestMethod]
        public void Unitialized()
        {
            Assert.ThrowsException<UninitializedStructException<Required<string>>>(() =>
            new Foo().name.Value);
        }
    }
}