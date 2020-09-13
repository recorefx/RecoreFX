using System;
using System.Collections.Generic;
using Recore.Collections.Generic;
using Xunit;

namespace Recore.Tests.Recore.Collections.Generic
{
    public class AnonymousEqualityComparerTests
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
                new Person { Id = 2, Name = "Z", Age = 52 }
            };

            // Compare on Name and Age
            var equalityComparer = new AnonymousEqualityComparer<Person>(
                (x, y) =>
                    x.Name == y.Name
                    && x.Age == y.Age,
                x => HashCode.Combine(x.Name, x.Age));

            Assert.Equal(persons, morePersons, equalityComparer);
            Assert.NotEqual(persons, notEqual, equalityComparer);
        }
    }
}
