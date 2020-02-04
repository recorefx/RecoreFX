using Xunit;

namespace Recore.Functional.Tests
{
    public class ComposerTests
    {
        [Fact]
        public void TrivialComposer()
        {
            var result = Composer.Of(1 + 1)
                .Result;

            Assert.Equal(2, result);
        }

        [Fact]
        public void EmptyComposer()
        {
            var Composer = new Composer<string>();
            Assert.Null(Composer.Result);

            var result = Composer
                .Then(string.IsNullOrEmpty)
                .Then(x => x.ToString())
                .Result;

            Assert.Equal("True", result);
        }

        [Fact]
        public void Then()
        {
            bool calledPrint = false;
            void Print(string value) => calledPrint = true;

            var result = Composer.Of("hello world")
                .Then(Print)
                .Then(x => x.Split())
                .Then(x => string.Join(", ", x))
                .Result;

            Assert.Equal("hello, world", result);
            Assert.True(calledPrint);
        }
    }
}
