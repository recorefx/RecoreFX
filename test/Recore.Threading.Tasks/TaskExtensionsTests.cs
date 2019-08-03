using System;
using System.Threading.Tasks;

using Xunit;

namespace Recore.Threading.Tasks.Tests
{
    public class OptionTests
    {
        private class MyExceptionType : Exception
        {
        }

        [Fact]
        public void Synchronize()
        {
            Task.CompletedTask.Synchronize();

            Assert.Throws<MyExceptionType>(
                () => Task.FromException(new MyExceptionType()).Synchronize());
        }

        [Fact]
        public void SynchronizeGeneric()
        {
            Assert.Equal(123, Task.FromResult(123).Synchronize());

            Assert.Throws<MyExceptionType>(
                () => Task<int>.FromException(new MyExceptionType()).Synchronize());
        }
    }
}