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
            var result = Pipeline.Of<string?>(null)
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
}
