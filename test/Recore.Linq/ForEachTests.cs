using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ForEachTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.ForEach<object>(null, x => { }));

            Assert.Throws<ArgumentNullException>(
                () => Enumerable.Empty<object>().ForEach(null));
        }

        [Fact]
        public void ForEachEmpty()
        {
            int timesCalled = 0;
            Enumerable.Empty<object>().ForEach(x => { ++timesCalled; });
            Assert.Equal(0, timesCalled);
        }

        [Fact]
        public void ForEachWithValues()
        {
            // Type as IEnumerable to avoid calling List<T>.ForEach()
            IEnumerable<int> enumerable = new List<int> { 1, 2, 3, 4, 5 };

            int timesCalled = 0;
            enumerable.ForEach(x => { ++timesCalled; });

            Assert.Equal(5, timesCalled);
        }
    }
}
