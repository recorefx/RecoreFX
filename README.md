# RecoreFX

[![CI](https://github.com/brcrista/RecoreFX/workflows/CI/badge.svg)](https://github.com/recorefx/RecoreFX/actions?query=workflow%3ACI)
[![Build Status](https://dev.azure.com/briancristante/RecoreFX/_apis/build/status/RecoreFX?branchName=master)](https://dev.azure.com/briancristante/RecoreFX/_build/latest?definitionId=11&branchName=master)

**RecoreFX** fills the most common needs for C# code after the .NET standard library.

Recore includes a variety of [collection](src/Recore.Collections.Generic) and [LINQ](src/Recore.Linq) extensions as well as some [best practices](src/Recore.Security.Cryptography/SecureCompare.cs) that most codebases end up implementing from scratch.
Recore also brings some F#-specific features, such as [`Optional`](src/Recore/Optional.cs) and [`Of`](src/Recore/Of.cs), to C# and other languages targeting .NET.

## Installation

Install from [NuGet](https://www.nuget.org/packages/RecoreFX/):

```
dotnet add package RecoreFX
```

## What's in it?

* `Recore`
    - [`AbsoluteUri`]() and [`RelativeUri`]()
    - [`AsyncAction`](), [`AsyncAction<T>`](), etc.
    - [`AsyncFunc<TResult>`](), [`AsyncFunc<T, TResult>`](), etc.
    - [`Composer<TValue, TResult>`]() and [`Pipeline<T>`]()
    - [`Defer`]()
    - [`Either<TLeft, TRight>`](), [`Optional<T>`](), and [`Result<TValue, TError>`]()
    - [`Func`]()
    - [`Of<T>`]()
    - [`Unit`]()
* `Recore.Collections.Generic`
    - [`AnonymousComparer<T>`]()
    - [`AnonymousEqualityComparer<T>`]()
    - [`MappedComparer<T, U>`]()
    - [`MappedEqualityComparer<T, U>`]()
    - Extension methods:
        - [`ICollection<T>.Append()`]()
        - [`IDictionary<TKey, TValue>.AddRange()`]()
        - [`IDictionary<TKey, TValue>.GetOrAdd()`]()
        - [`IDictionary<TKey, TValue>.ValueOrDefault()`]()
        - [`LinkedList<T>.Add()`]()
* `Recore.Linq`
    - [`IEnumerable<T>.Argmax()`]()
    - [`IEnumerable<T>.Argmin()`]()
    - [`IEnumerable<T>.Enumerate()`]()
    - [`IEnumerable<T>.Flatten()`]()
    - [`IEnumerable<T>.ForEach()`]()
    - [`IEnumerable<T>.NonNull()`]()
    - [`IEnumerable<T>.Product()`]()
    - [`IEnumerable<T>.ToLinkedList()`]()
    - [`IEnumerable<T>.ZipTuple()`]()
    - [`IEnumerable<KeyValuePair<TKey, TValue>>.OnKeys()`]()
    - [`IEnumerable<KeyValuePair<TKey, TValue>>.OnValues()`]()
    - [`IEnumerable<KeyValuePair<TKey, TValue>>.ToDictionary()`]()
* `Recore.Security.Cryptography`
    - [`Ciphertext()`]()
    - [`SecureCompare()`]()
* `Recore.Threading.Tasks`
    - [`Task.Synchronize()`]()
    - [`Task<T>.Synchronize()`]()


## Why Recore?

There are other packages offering more LINQ-style methods, better URI handling, or implementations of `Optional`.
These packages admittedly have richer feature sets than Recore has, and may be right for your project if you want that depth of functionality.

RecoreFX is designed to feel like a natural extension of CoreFX.
It's meant to be a one-stop shop, filling in the most common use cases without a steep learning curve.

## API documentation

<https://recorefx.github.io/>
