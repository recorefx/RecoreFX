using System;
using System.Linq;

using Xunit;

namespace Recore.Functional.Tests
{
    public class OperatorTests
    {
        [Fact]
        public void Not()
        {
            Assert.False(Operator.Not(true));
            Assert.True(Operator.Not(false));

            var xs = new[] { false, true, false };
            Assert.Equal(new[] { true, false, true }, xs.Select(Operator.Not));
        }
    }
}