using Xunit;

namespace Recore.Functional.Tests
{
    public class PipelineTests
    {
        [Fact]
        public void TrivialPipeline()
        {
            var result = Pipeline.Of(1 + 1)
                .Result;

            Assert.Equal(2, result);
        }

        [Fact]
        public void ThenAllFuncs()
        {
            var result = Pipeline.Of<string>(null)
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

            var pipeline = Pipeline.Of("hello world")
                .Then(Print)
                .Then(x => x.Split())
                .Then(x => string.Join(":", x));

            // Action should be called eagerly, before calling .Result
            Assert.True(calledPrint);
            Assert.Equal("hello:world", pipeline.Result);
        }
    }

    public class FunctionPipelineTests
    {
        [Fact]
        public void TrivialPipeline()
        {
            // TODO .NET Core 3
            /*static*/ bool IsEven(int x) => x % 2 == 0;
            var func = new Pipeline<int, bool>(IsEven)
                .Func;

            Assert.True(func(0));
            Assert.False(func(1));
        }

        [Fact]
        public void ThenAllFuncs()
        {
            var func = new Pipeline<string, bool>(string.IsNullOrEmpty)
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

            var func = new Pipeline<Unit, string>(_ => "hello world")
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
