using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Threading.Tasks.Tests
{
    [TestClass]
    public class OptionTests
    {
        private class MyExceptionType : Exception
        {
        }

        [TestMethod]
        public void Synchronize()
        {
            Task.CompletedTask.Synchronize();

            Assert.ThrowsException<MyExceptionType>(
                () => Task.FromException(new MyExceptionType()).Synchronize());
        }

        [TestMethod]
        public void SynchronizeGeneric()
        {
            Assert.AreEqual(123, Task.FromResult(123).Synchronize());

            Assert.ThrowsException<MyExceptionType>(
                () => Task<int>.FromException(new MyExceptionType()).Synchronize());
        }
    }
}