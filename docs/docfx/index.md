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

### `Optional<T>`

[`Optional<T>`](https://recorefx.github.io/api/Recore.Optional-1.html) gives you compiler-checked null safety if you don't have nullable references enabled (or if you're on .NET Framework):

```cs
Optional<string> opt = "hello";
Optional<string> empty = Optional<string>.Empty;

opt.Switch(
    x => Console.WriteLine("Message: " + x),
    () => Console.WriteLine("No message"));

Optional<int> messageLength = opt.OnValue(x => x.Length);
string message = opt.ValueOr(default);
```

### `Either<TLeft, TRight>`

[`Either<TLeft, TRight>`](https://recorefx.github.io/api/Recore.Either-2.html) gives you a type-safe union type that will be familiar to TypeScript users:

```cs
Either<string, int> either = "hello";

var message = either.Switch(
    l => $"Value is a string: {l}",
    r => $"Value is an int: {r}");
```

### `Result<TValue, TError>`

[`Result<TValue, TError>`](https://recorefx.github.io/api/Recore.Result.html) gives you a way to handle "expected" errors. You can think of it as a nicer version of the `TryParse` pattern:

```cs
async Task<Result<Person, HttpStatusCode>> GetPersonAsync(int id)
{
    var response = await httpClient.GetAsync($"/api/v1/person/{id}");
    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<Person>(json);
        return Result.Success<Person, HttpStatusCode>(person);
    }
    else
    {
        return Result.Failure<Person, HttpStatusCode>(response.StatusCode);
    }
}
```

It also makes it easy to build up an error context as you go along rather than terminating immediately:

```cs
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

List<IBlob> successes = results.Successes().ToList();
List<IBlob> failures = results.Failures().ToList();
```

### `Of<T>`

[`Of<T>`](https://recorefx.github.io/api/Recore.Of-1.html) makes it easy to define "type aliases."


Consider a method with the signature:

```cs
void AddRecord(string address, string firstName, string lastName)
```

It's easy to make mistakes like this:

```cs
AddRecord("Jane", "Doe", "1 Microsoft Way"); // oops!
```

You can prevent this with strong typing:

```cs
class Address : Of<string> {}

void AddRecord(Address address, string firstName, string lastName) {}

AddRecord("Jane", "Doe", "1 Microsoft Way"); // compiler error
```

While defining a new type that behaves the same way `string` does usually takes a lot of boilerplate, `Of<T>` handles this automatically:

```cs
var address = new Address { Value = "1 Microsoft Way" };
Console.WriteLine(address); // prints "1 Microsoft Way"

var address2 = new Address { Value = "1 Microsoft Way" };
Console.WriteLine(address == address2); // prints "true"
```

### `Pipeline<T>`

`Pipeline<T>` gives you a way to call any method with postfix syntax:

```cs
var result = Pipeline.Of(value)
    .Then(Foo)
    .Then(Bar)
    .Then(Baz)
    .Result;
```

### `Defer`

`Defer` is analogous to Golang's `defer` statement. It lets you do some kind of cleanup before exiting a method.

The classic way to do this in C# is with `try-finally`:

```cs
try
{
    Console.WriteLine("Doing stuff");
}
finally
{
    Console.WriteLine("Running cleanup");
}
```

With `Defer` and C# 8's new `using` declarations, we can do it more simply:

```cs
using var cleanup = new Defer(() => Console.WriteLine("Running cleanup"));
Console.WriteLine("Doing stuff");
```

### `Unit`

[`Unit`](https://recorefx.github.io/api/Recore.Unit.html) is a type with only one value (like how `Void` is a type with no values).

Imagine a type `ApiResult<T>` that wraps the deserialized JSON response from a REST API.
Without `Unit`, you'd have to define a separate, non-generic `ApiResult` type for when the response doesn't have a body:

```cs
ApiResult postResult = await PostPersonAsync(person);
ApiResult<Person> getResult = await GetPersonAsync(id);
```

With `Unit`, you can just reuse the same type:

```cs
ApiResult<Unit> postResult = await PostPersonAsync(person);
ApiResult<Person> getResult = await GetPersonAsync(id);
```

In the definition of `PostPersonAsync()`, just return `Unit.Value`:

```cs
ApiResult<Unit> PostPersonAsync(Person person)
{
    // ...
    return new ApiResult<Unit>(Unit.Value);
}
```

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

## Contributing

Have a look at the [contributor's guide](https://github.com/recorefx/RecoreFX/blob/main/docs/CONTRIBUTING.md).

## FAQs

### Why doesn't this have `$TYPE` or `$METHOD`?

If it's generally useful (as opposed to oriented towards a specific application) and fills a common need in C# programming, then there's no reason why not! Feel free to open an issue or submit a PR for discussion.

### How does this compare to `$LIBRARY`?

The [design principles](https://github.com/recorefx/RecoreFX/blob/main/docs/CONTRIBUTING.md#design-principles) of RecoreFX are:
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
