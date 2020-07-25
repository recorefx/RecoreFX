using Xunit;

namespace Recore.Functional.Tests
{
    public class ValueComposerTests
    {
        [Fact]
        public void TrivialComposer()
        {
            var result = Composer.Of(1 + 1)
                .Result;

            Assert.Equal(2, result);
        }

        [Fact]
        public void ThenAllFuncs()
        {
            var result = Composer.Of<string>(null)
                .Then(string.IsNullOrEmpty)
                .Then(x => x.ToString())
                .Result;

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
                .Then(x => string.Join(":", x));

            // Action should be called eagerly, before calling .Result
            Assert.True(calledPrint);
            Assert.Equal("hello:world", composer.Result);
        }
    }

    public class FunctionComposerTests
    {
        [Fact]
        public void TrivialComposer()
        {
            // TODO .NET Core 3
            /*static*/ bool IsEven(int x) => x % 2 == 0;
            var func = new Composer<int, bool>(IsEven)
                .Func;

            Assert.True(func(0));
            Assert.False(func(1));
        }

        [Fact]
        public void ThenAllFuncs()
        {
            var func = new Composer<string, bool>(string.IsNullOrEmpty)
                .Then(x => x.ToString())
                .Then(x => x.Length)
                .Func;

            Assert.Equal(4, func(null));
            Assert.Equal(4, func(string.Empty));
            Assert.Equal(5, func("hello"));
        }

        [Fact]
        public void ThenWithAction()
        {
            bool calledPrint = false;
            void Print(string value) => calledPrint = true;

            var func = new Composer<Unit, string>(_ => "hello world")
                .Then(Print)
                .Then(x => x.Split())
                .Then(x => string.Join(":", x))
                .Func;

            // Action should be called lazily
            Assert.False(calledPrint);

            Assert.Equal("hello:world", func(Unit.Value));
            Assert.True(calledPrint);
        }
    }
}
