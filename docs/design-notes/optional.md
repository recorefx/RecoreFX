# Optional

## Why `readonly struct`?
- The type is meant to safely handle null. Making it a value type means instances of the type itself can't be null.
- `ref` structs are "stack-only" values and can't live as instances of another type (unless that type is also a `ref` struct).

## Why `where T : class`?
- This simplifies handling of empty optionals.
Without this constraint, you have to maintain additional state to know whether the optional is set or not.
- Removing this constraint in the future would not be a breaking change.

## Why the name?
- Also considered: `Option`, `Maybe`
  - `Option` or `Optional` seems to be more widespread. I haven't seen `Maybe` outside of Haskell.
  - F#'s type is named `Option`, would make sense if C#'s were named similarly.
  - However, Java and C++ both use `optional`.
  - Also, .NET has `Nullable`, so `Optional` would parallel that.
  - `Optional<int>` sounds more natural than `Option<int>`.

## What about the methods?
- `Empty`: also considered `None`
    - `None` is Python's concept for null, which I think sounds natural
    - `Empty` matches Java's `optional`
    - `Empty` matches `string.Empty`, `Array.Empty<T>()`, and `Enumerable.Empty<T>()`
    - Why is `Empty` a method? Why doesn't it return a single instance?
      - Since `Optional<T>` is a value type, you can't have a single instance.
      -  While `string.Empty` is a property, `Array.Empty<T>()` and `Enumerable.Empty<T>()` are extensions because properties can't be generic. While `Optional<T>.Empty` could be a property, `Optional.Empty<T>()` complements `Optional.Of<T>()` and hints that an allocation is happening. Also, you can pass the method as a delegate.
      - C#'s type inference is not strong enough to infer `T` from the return type alone, but perhaps one day it will be.
    - `Empty` consolidates concepts.  When talking about optionals in prose, it's more natual to say "empty optional" than "none optional."
    - I considered having an `EmptyType` with a single instance that was implicitly convertible to `Optional<T>` for any `T`. This would function like `null` for `Optional` and follows `nullopt` / `nullopt_t` from C++.  However, I'm leaving it out for the following reasons:
        - Adds complexity to the interface for little benefit
        - Incurs compile time and run time cost to convert
        - Takes special handling for equality to work in all cases
        - It would not be a breaking change to add it in the future
- `Switch`: also considered `Match`, `Choose`, `Pick`, `Select`
    - `Match` was what I originally called this since the method is doing what pattern matching does in other languages.  However, pattern matching is a language feature, not an `Optional` feature.
    - The name of the method has to describe the two usually anonymous functions that follow. `Switch` accomplishes this by evoking the `switch` statement in the language.
    - It's also worth noting that C# has implemented pattern matching with the `switch` statement.
- `OnValue`: also considered `Map`, `Try`
    - Again, going with the name of an established concept. `Map` isn't so bad, but wouldn't be evocative for users unfamiliar with functional programming.
    - `Try` clearly evokes that something still happens in the empty case, even though only one function is passed.
    - `OnValue` mirros `IfValue`, the void-returning analog
    - `Try` is already associated with exceptions, which don't come into play here
- `Then`: also considered `Bind`
    - Again, `Bind` isn't evocative for users unfamiliar with functional programming.
    - `Then` evokes a monad bind that many people are familiar with, that of Promises in JavaScript. .NET's own `ContinueWith` is a little too clumsy and more specific to asynchronous programming.