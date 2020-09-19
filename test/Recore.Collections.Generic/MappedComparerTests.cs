using System;
using System.Collections.Generic;
using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class MappedComparerTests
    {
        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void Sort()
        {
            var persons = new List<Person>
            {
                new Person { Id = 0, Name = "A", Age = 29 },
                new Person { Id = 1, Name = "B", Age = 14 },
                new Person { Id = 2, Name = "C", Age = 52 }
            };

            var sortedPersons = new List<Person>
            {
                new Person { Id = 1, Name = "B", Age = 14 },
                new Person { Id = 0, Name = "A", Age = 29 },
                new Person { Id = 2, Name = "C", Age = 52 }
            };

            var byAge = new MappedComparer<Person, int>(x => x.Age);
            persons.Sort(byAge);

            var equalityComparer = new AnonymousEqualityComparer<Person>(
                (x, y) =>
                    x.Id == y.Id
                    && x.Name == y.Name
                    && x.Age == y.Age,
                x => HashCode.Combine(x.Name, x.Age));

            Assert.Equal(sortedPersons, persons, equalityComparer);
        }

        [Theory]
        [InlineData("hello", "world", 0)]
        [InlineData("hello", null, 1)]
        [InlineData(null, "world", -1)]
        [InlineData(null, null, 0)]
        public void WithNull(string? x, string? y, int expected)
        {
            var compareOnLength = new MappedComparer<string, int>(x => x.Length);
            Assert.Equal(expected, compareOnLength.Compare(x, y));
        }
    }
}
