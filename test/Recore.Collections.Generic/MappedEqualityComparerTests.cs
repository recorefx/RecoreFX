using System.Collections.Generic;
using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class MappedEqualityComparerTests
    {
        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void SequenceEquals()
        {
            var persons = new List<Person>
            {
                new Person { Id = 0, Name = "A", Age = 29 },
                new Person { Id = 1, Name = "B", Age = 14 },
                new Person { Id = 2, Name = "C", Age = 52 }
            };

            var morePersons = new List<Person>
            {
                new Person { Id = 0, Name = "A", Age = 29 },
                new Person { Id = 1, Name = "B", Age = 14 },
                new Person { Id = 2, Name = "C", Age = 52 }
            };

            var notEqual = new List<Person>
            {
                new Person { Id = 0, Name = "A", Age = 29 },
                new Person { Id = 1, Name = "B", Age = 14 },
                new Person { Id = 2, Name = "C", Age = 53 }
            };

            var sameAge = new MappedEqualityComparer<Person, int>(x => x.Age);
            Assert.Equal(persons, morePersons, sameAge);
            Assert.NotEqual(persons, notEqual, sameAge);
        }

        [Theory]
        [InlineData("hello", "world", true)]
        [InlineData("hello", null, false)]
        [InlineData(null, "world", false)]
        [InlineData(null, null, true)]
        public void WithNull(string? x, string? y, bool expected)
        {
            var compareOnLength = new MappedEqualityComparer<string, int>(x => x.Length);
            Assert.Equal(expected, compareOnLength.Equals(x, y));
        }
    }
}
