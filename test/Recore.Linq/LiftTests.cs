using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class LiftTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.Lift<object>(null!));
        }

        [Fact]
        public void LiftAction()
        {
            int timesCalled = 0;
            var lifted = Renumerable.Lift(
                (int n) => { ++timesCalled; });

            Assert.Equal(0, timesCalled);

            var enumerable = Enumerable.Empty<int>();
            lifted(enumerable);
            Assert.Equal(0, timesCalled);

            enumerable = new List<int> { 1, 2, 3, 4, 5 };
            lifted(enumerable);
            Assert.Equal(5, timesCalled);
        }

        [Fact]
        public void LiftFunc()
        {
            var lifted = Renumerable.Lift(
                (int n) => 1);

            var enumerable = Enumerable.Empty<int>();
            Assert.Empty(lifted(enumerable));

            enumerable = new List<int> { 1, 2, 3, 4, 5 };
            lifted(enumerable);
            Assert.Equal(new List<int> { 1, 1, 1, 1, 1 }, lifted(enumerable));
        }
    }
}
