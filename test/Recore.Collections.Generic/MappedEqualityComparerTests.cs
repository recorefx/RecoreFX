using System.Collections.Generic;
using Recore.Collections.Generic;
using Xunit;

namespace Recore.Tests.Recore.Collections.Generic
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
    }
}
