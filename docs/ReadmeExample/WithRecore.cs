using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Recore;
using Recore.Collections.Generic;

namespace ReadmeExample
{
    class WithRecore
    {
        public async Task DownloadBlobsAsync(IEnumerable<IBlob> blobs)
        {
            Result<IBlob, IBlob>[] results = await Task.WhenAll(blobs.Select(blob =>
                Result.TryAsync(async () =>
                {
                    await WriteBlobAsync(blob);
                    return blob;
                })
                .CatchAsync((Exception e) =>
                {
                    Console.Error.WriteLine(e);
                    return Task.FromResult(blob);
                })));
            
            // Print summary
            List<IBlob> successes = results.Successes().ToList();
            List<IBlob> failures = results.Failures().ToList();

            var compareOnName = new MappedEqualityComparer<IBlob, string>(x => x.Name);
            IEnumerable<IBlob> existingBlobs = await GetLocalBlobsAsync();

            Console.WriteLine($"Downloaded {successes.Except(existingBlobs, compareOnName).Count()} new blob(s)");
            Console.WriteLine($"Overwrote {successes.Intersect(existingBlobs, compareOnName).Count()} existing blob(s)");
            Console.WriteLine($"Failed to download {failures.Count} blob(s):");
            failures.ForEach(x => Console.WriteLine("  " + x.Name));
        }

        private Task WriteBlobAsync(IBlob blob)
        {
            if (blob.Name.Contains('/'))
            {
                throw new Exception();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private Task<IEnumerable<IBlob>> GetLocalBlobsAsync()
        {
            IEnumerable<IBlob> localBlobs = new[] { new MockBlob { Name = "hello" } };
            return Task.FromResult(localBlobs);
        }
    }
}
