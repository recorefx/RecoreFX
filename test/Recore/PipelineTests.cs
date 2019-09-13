using Xunit;

namespace Recore.Tests
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
        public void EmptyPipeline()
        {
            var pipeline = new Pipeline<string>();
            Assert.Null(pipeline.Result);

            var result = pipeline
                .Pipe(string.IsNullOrEmpty)
                .Pipe(x => x.ToString())
                .Result;

            Assert.Equal("true", result);
        }

        [Fact]
        public void Pipe()
        {
            bool calledPrint = false;
            void Print(string value) => calledPrint = true;

            var result = Pipeline.Of("hello world")
                .Pipe(Print)
                .Pipe(x => x.Split())
                .Pipe(x => string.Join(", ", x))
                .Result;

            Assert.Equal(result, "hello, world");
            Assert.True(calledPrint);
        }
    }
}
