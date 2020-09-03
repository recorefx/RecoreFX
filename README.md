# RecoreFX

[![GitHub Actions Badge](https://github.com/brcrista/RecoreFX/workflows/CI/badge.svg)](https://github.com/recorefx/RecoreFX/actions?query=workflow%3ACI)
[![Azure Pipelines Badge](https://dev.azure.com/briancristante/RecoreFX/_apis/build/status/RecoreFX?branchName=main)](https://dev.azure.com/briancristante/RecoreFX/_build/latest?definitionId=11&branchName=main)
[![NuGet Badge](https://buildstats.info/nuget/RecoreFX)](https://www.nuget.org/packages/RecoreFX)

**RecoreFX** fills the most common needs for C# code after the .NET standard library.

## Installation

Install from [NuGet](https://www.nuget.org/packages/RecoreFX/):

```
dotnet add package RecoreFX
```

## Why use it?

### Convenience methods

If you're like me, there's a bunch of useful methods that you write for every project you work on. Some are simple, such as [`IDictionary.GetOrAdd()`](https://recorefx.github.io/api/Recore.Collections.Generic.IDictionaryExtensions.html#Recore_Collections_Generic_IDictionaryExtensions_GetOrAdd__2_IDictionary___0___1____0___1_). Others are more subtle, such as [`SecureCompare.TimeInvariantEquals()`](https://recorefx.github.io/api/Recore.Security.Cryptography.SecureCompare.html#Recore_Security_Cryptography_SecureCompare_TimeInvariantEquals_System_Byte___System_Byte___).

There's a lot of low-hanging fruit. Want JavaScript-style IIFEs? Write [`Func.Invoke()`](https://recorefx.github.io/api/Recore.Func.html#Recore_Func_Invoke__1_Recore_Func___0__). Want ad-hoc RAII like in Go? Create a [`Defer`](https://recorefx.github.io/api/Recore.Defer.html) type. Tired of checking `IsAbsoluteUri`? Define an [`AbsoluteUri`](https://recorefx.github.io/api/Recore.AbsoluteUri.html) subtype. (But let's be honest, who really checks?)

All of this starts to add up, though. That's why I put it all together into a single installable, unit-tested package.

### New stuff

There are some other goodies here that are farther reaching:
- [`Either<TLeft, TRight>`](https://recorefx.github.io/api/Recore.Either-2.html) gives you a type-safe union type that will be familiar to TypeScript users.
- [`Optional<T>`](https://recorefx.github.io/api/Recore.Optional-1.html) gives you compiler-checked null safety if you don't have nullable references enabled (or if you're on .NET Framework).
- [`Result<TValue, TError>`](https://recorefx.github.io/api/Recore.Result.html) gives you a way to handle errors besides immediately terminating execution of a method or going `Try*` everywhere. Instead, you can build up an error context as you go along.
- [`Of<T>`](https://recorefx.github.io/api/Recore.Of-1.html) takes the boilerplate out of definining simple types. Want to replace `string` with `Email`? Now you can.
- [`Unit`](https://recorefx.github.io/api/Recore.Unit.html) fixes the `Task` / `Task<T>` problem of having to duplicate your generic types for void-returning operations.

These are all borrowed from functional programming, but the goal here isn't to turn C# into F#.
RecoreFX is meant to encourage more expressive, type-safe code that's still idiomatic C#.

## What's in it?

#### Recore

- [`AbsoluteUri`](https://recorefx.github.io/api/Recore.AbsoluteUri.html) and [`RelativeUri`](https://recorefx.github.io/api/Recore.RelativeUri.html)
- [`AsyncAction`](https://recorefx.github.io/api/Recore.AsyncAction.html), [`AsyncAction<T>`](https://recorefx.github.io/api/Recore.AsyncAction-1.html), etc.
- [`AsyncFunc<TResult>`](https://recorefx.github.io/api/Recore.AsyncFunc-1.html), [`AsyncFunc<T, TResult>`](https://recorefx.github.io/api/Recore.AsyncFunc-2.html), etc.
- [`Composer<TValue, TResult>`](https://recorefx.github.io/api/Recore.Composer-2.html) and [`Pipeline<T>`](https://recorefx.github.io/api/Recore.Pipeline-1.html)
- [`Defer`](https://recorefx.github.io/api/Recore.Defer.html)
- [`Either<TLeft, TRight>`](https://recorefx.github.io/api/Recore.Either-2.html), [`Optional<T>`](https://recorefx.github.io/api/Recore.Optional-1.html), and [`Result<TValue, TError>`](https://recorefx.github.io/api/Recore.Result.html)
- [`Func`](https://recorefx.github.io/api/Recore.Func.html)
- [`Of<T>`](https://recorefx.github.io/api/Recore.Of-1.html)
- [`Unit`](https://recorefx.github.io/api/Recore.Unit.html)

#### Recore.Collections.Generic

- [`AnonymousComparer<T>`](https://recorefx.github.io/api/Recore.Collections.Generic.AnonymousComparer-1.html)
- [`AnonymousEqualityComparer<T>`](https://recorefx.github.io/api/Recore.Collections.Generic.AnonymousEqualityComparer-1.html)
- [`MappedComparer<T, U>`](https://recorefx.github.io/api/Recore.Collections.Generic.MappedComparer-2.html)
- [`MappedEqualityComparer<T, U>`](https://recorefx.github.io/api/Recore.Collections.Generic.MappedEqualityComparer-2.html)
- Extension methods:
    - [`ICollection<T>.Append()`](https://recorefx.github.io/api/Recore.Collections.Generic.ICollectionExtensions.html#Recore_Collections_Generic_ICollectionExtensions_Append__1_ICollection___0____0_)
    - [`IDictionary<TKey, TValue>.AddRange()`](https://recorefx.github.io/api/Recore.Collections.Generic.IDictionaryExtensions.html#Recore_Collections_Generic_IDictionaryExtensions_AddRange__2_IDictionary___0___1__IEnumerable_KeyValuePair___0___1___)
    - [`IDictionary<TKey, TValue>.GetOrAdd()`](https://recorefx.github.io/api/Recore.Collections.Generic.IDictionaryExtensions.html#Recore_Collections_Generic_IDictionaryExtensions_GetOrAdd__2_IDictionary___0___1____0___1_)
    - [`IDictionary<TKey, TValue>.GetValueOrDefault()`](https://recorefx.github.io/api/Recore.Collections.Generic.IDictionaryExtensions.html#Recore_Collections_Generic_IDictionaryExtensions_GetValueOrDefault__2_Dictionary___0___1____0_)
    - [`LinkedList<T>.Add()`](https://recorefx.github.io/api/Recore.Collections.Generic.LinkedListExtensions.html#Recore_Collections_Generic_LinkedListExtensions_Add__1_LinkedList___0____0_)
    
#### Recore.Linq

- [`IEnumerable<T>.Argmax()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Argmax__2_IEnumerable___0__Recore_Func___0___1__)
- [`IEnumerable<T>.Argmin()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Argmin__2_IEnumerable___0__Recore_Func___0___1__)
- [`IEnumerable<T>.Enumerate()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Enumerate__1_IEnumerable___0__)
- [`IEnumerable<T>.Flatten()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Flatten__1_IEnumerable_IEnumerable___0___)
- [`IEnumerable<T>.ForEach()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_ForEach__1_IEnumerable___0__Action___0__)
- [`IEnumerable<T>.NonNull()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_NonNull__1_IEnumerable___0__)
- [`IEnumerable<T>.Product()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Product__2_IEnumerable___0__IEnumerable___1__)
- [`IEnumerable<T>.ToLinkedList()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_ToLinkedList__1_IEnumerable___0__)
- [`IEnumerable<T>.Zip()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_Zip__2_IEnumerable___0__IEnumerable___1__)
- [`IEnumerable<KeyValuePair<TKey, TValue>>.OnKeys()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_OnKeys__3_IEnumerable_KeyValuePair___0___1___Recore_Func___0___2__)
- [`IEnumerable<KeyValuePair<TKey, TValue>>.OnValues()`](https://recorefx.github.io/api/Recore.Linq.Renumerable.html#Recore_Linq_Renumerable_OnValues__3_IEnumerable_KeyValuePair___0___1___Recore_Func___1___2__)
- [`IEnumerable<KeyValuePair<TKey, TValue>>.ToDictionary()`]()

#### Recore.Security.Cryptography

- [`Ciphertext<THash>`](https://recorefx.github.io/api/Recore.Security.Cryptography.Ciphertext-1.html)
- [`SecureCompare`](https://recorefx.github.io/api/Recore.Security.Cryptography.SecureCompare.html)

#### Recore.Threading.Tasks

- [`Task.Synchronize()`](https://recorefx.github.io/api/Recore.Threading.Tasks.TaskExtensions.html#Recore_Threading_Tasks_TaskExtensions_Synchronize_Task_)
- [`Task<T>.Synchronize()`](https://recorefx.github.io/api/Recore.Threading.Tasks.TaskExtensions.html#Recore_Threading_Tasks_TaskExtensions_Synchronize__1_Task___0__)

## Example

Say you want to download a bunch of "blobs" and then print out a summary of what succeeded and what failed.
To add a dose of reality, you can also choose whether you want to overwrite existing blobs you already have locally.

Say the blob interface looks like this:

```cs
interface IBlob
{
    string Name { get; }
    byte[] GetContents();
}
```

### With Recore

Here's how the implementation looks using types from RecoreFX:

```cs
async Task DownloadBlobsAsync(IEnumerable<IBlob> blobs, bool overwrite)
{
    // Check `overwrite` to see which blobs to download
    var compareOnName = new MappedEqualityComparer<IBlob, string>(x => x.Name);
    IEnumerable<IBlob> existingBlobs = await GetLocalBlobsAsync();

    IEnumerable<IBlob> blobsToWrite = Func.Invoke(() =>
    {
        if (overwrite)
        {
            return blobs;
        }
        else
        {
            return blobs.Except(existingBlobs, compareOnName);
        }
    });

    // Write blobs
    Result<IBlob, IBlob>[] results = await Task.WhenAll(blobsToWrite.Select(blob =>
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

    Console.WriteLine($"Downloaded {successes.Except(existingBlobs, compareOnName).Count()} new blob(s)");
    Console.WriteLine($"Overwrote {successes.Intersect(existingBlobs, compareOnName).Count()} existing blob(s)");
    Console.WriteLine($"Failed to download {failures.Count} blob(s):");
    failures.ForEach(x => Console.WriteLine("  " + x.Name));
}
```

### Without Recore

Here's how I would write this code in plain C#:

```cs
async Task DownloadBlobsAsync(IEnumerable<IBlob> blobs, bool overwrite)
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
```

You can see, build, and run this example [here](docs/ReadmeExample).

## Contributing

Have a look at the [contributor's guide](docs/CONTRIBUTING.md).

## FAQs

### Why doesn't this have `$TYPE` or `$METHOD`?

If it's generally useful (as opposed to oriented towards a specific application) and fills a common need in C# programming, then there's no reason why not! Feel free to open an issue or submit a PR for discussion.

### How does this compare to `$LIBRARY`?

The [design principles](docs/CONTRIBUTING.md#design-principles) of RecoreFX are:
1. Generally useful
2. Common sense-ness
3. Follows the programming paradigm of standard C#

If you like RecoreFX, check out these other libraries:
- [louthy/language-ext](https://github.com/louthy/language-ext)
- [mcintyre321/OneOf](https://github.com/mcintyre321/OneOf)
- [morelinq/MoreLINQ](https://github.com/morelinq/MoreLINQ)
- [StephenCleary/AsyncEx](https://github.com/StephenCleary/AsyncEx)

### Does this work with .NET Framework?

RecoreFX v1 targets .NET Standard 2.0, so it works with [.NET Framework ≥ 4.6.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

## Sample App

The [RecoreFX Sample App](https://github.com/recorefx/RecoreFX-Sample-App) is a fully worked out Web app with a console app client using RecoreFX.

## Reference

<https://recorefx.github.io>
