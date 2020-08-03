using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class ResultConverterTests
    {
        [Fact]
        public void ToJson()
        {
            {
                Result<int, string> result;

                result = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(result));

                result = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(result));
            }
            {
                Result<Person, Exception> result;

                result = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(result));
            }
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: new Result<int, string>(12),
                actual: JsonSerializer.Deserialize<Result<int, string>>("12"));

            Assert.Equal(
                expected: new Result<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Result<int, string>>("\"hello\""));

            Assert.Equal(
                expected: new Result<string, int>(12),
                actual: JsonSerializer.Deserialize<Result<string, int>>("12"));

            Assert.Equal(
                expected: new Result<string, int>("hello"),
                actual: JsonSerializer.Deserialize<Result<string, int>>("\"hello\""));

            var deserializedPerson = JsonSerializer.Deserialize<Result<Person, Exception>>("{\"Name\":\"Mario\",\"Age\":42}");
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetValue().First().Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetValue().First().Age);
        }
    }
}
