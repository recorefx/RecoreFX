using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class FuncTests
    {
        [TestMethod]
        public void Invoke()
        {
            string name = null;
            var result = Func.Invoke(() =>
            {
                if (name == null)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });

            Assert.AreEqual(-1, result);
        }
    }
}
