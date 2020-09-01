# RecoreFX

[![CI](https://github.com/brcrista/RecoreFX/workflows/CI/badge.svg)](https://github.com/recorefx/RecoreFX/actions?query=workflow%3ACI)
[![Build Status](https://dev.azure.com/briancristante/RecoreFX/_apis/build/status/RecoreFX?branchName=main)](https://dev.azure.com/briancristante/RecoreFX/_build/latest?definitionId=11&branchName=main)

**RecoreFX** fills the most common needs for C# code after the .NET standard library.

Recore includes a variety of [collection](src/Recore.Collections.Generic) and [LINQ](src/Recore.Linq) extensions as well as some [best practices](src/Recore.Security.Cryptography/SecureCompare.cs) that most codebases end up implementing from scratch.
Recore also brings some F#-specific features, such as [`Optional`](src/Recore/Optional.cs) and [`Of`](src/Recore/Of.cs), to C# and other languages targeting .NET.

## Installation

Install from [NuGet](https://www.nuget.org/packages/RecoreFX/):

```
dotnet add package RecoreFX
```

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
    - [`IDictionary<TKey, TValue>.ValueOrDefault()`](https://recorefx.github.io/api/Recore.Collections.Generic.IDictionaryExtensions.html#Recore_Collections_Generic_IDictionaryExtensions_ValueOrDefault__2_Dictionary___0___1____0_)
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

## Why Recore?

There are other packages offering more LINQ-style methods, better URI handling, or implementations of `Optional`.
These packages admittedly have richer feature sets than Recore has, and may be right for your project if you want that depth of functionality.

RecoreFX is designed to feel like a natural extension of CoreFX.
It's meant to be a one-stop shop, filling in the most common use cases without a steep learning curve.

## API documentation

<https://recorefx.github.io/>
