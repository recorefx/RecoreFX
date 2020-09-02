using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadmeExample
{
    class WithoutRecore
    {
        public async Task DownloadBlobsAsync(IEnumerable<IBlob> blobs, bool overwrite)
        {
            // Check `overwrite` to see which blobs to download
            IEnumerable<IBlob> existingBlobs = await GetLocalBlobsAsync();
            IEnumerable<string> existingBlobNames = existingBlobs.Select(x => x.Name);

            List<IBlob> blobsToWrite;
            if (overwrite)
            {
                blobsToWrite = blobs.ToList();
            }
            else
            {
                blobsToWrite = new List<IBlob>();
                foreach (var blob in blobs)
                {
                    if (!existingBlobNames.Contains(blob.Name))
                    {
                        blobsToWrite.Add(blob);
                    }
                }
            }

            // Write blobs
            var successes = new List<IBlob>();
            var failures = new List<IBlob>();

            foreach (var blob in blobsToWrite)
            {
                try
                {
                    await WriteBlobAsync(blob);
                    successes.Add(blob);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    failures.Add(blob);
                }
            }
            
            // Print summary
            int numNewBlobs = successes
                .Select(x => x.Name)
                .Except(existingBlobNames)
                .Count();

            int numOverwrittenBlobs = successes
                .Select(x => x.Name)
                .Intersect(existingBlobNames)
                .Count();

            Console.WriteLine($"Downloaded {numNewBlobs} new blob(s)");
            Console.WriteLine($"Overwrote {numOverwrittenBlobs} existing blob(s)");
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
