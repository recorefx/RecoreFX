using Xunit;

namespace Recore.Functional.Tests
{
    public class ComposerTests
    {
        [Fact]
        public void TrivialComposer()
        {
            var result = Composer.Of(1 + 1)
                .Func();

            Assert.Equal(2, result);
        }

        [Fact]
        public void ThenAllFuncs()
        {
            var result = Composer.Of<string>(null)
                .Then(string.IsNullOrEmpty)
                .Then(x => x.ToString())
                .Func();

            Assert.Equal("True", result);
        }

        [Fact]
        public void ThenWithAction()
        {
            bool calledPrint = false;
            void Print(string value) => calledPrint = true;

            var composer = Composer.Of("hello world")
                .Then(Print)
                .Then(x => x.Split())
                .Then(x => string.Join(", ", x));

            // Action should be called lazily
            Assert.False(calledPrint);

            Assert.Equal("hello, world", composer.Func());
            Assert.True(calledPrint);
        }
    }
}
