using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadmeExample
{
    class MockBlob : IBlob
    {
        public string Name { get; set; }

        public byte[] GetContents() => Array.Empty<byte>();
    }

    static class Program
    {
        static async Task Main(string[] args)
        {
            var example = new WithRecore();
            // var example = new WithoutRecore();

            var blobs = new[]
            {
                "hello",
                "abc/def",
                "world"
            }.Select(name => new MockBlob { Name = name });

            await example.DownloadBlobsAsync(blobs, overwrite: false);
        }
    }
}
