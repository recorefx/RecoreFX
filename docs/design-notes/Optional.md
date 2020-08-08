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
    - Should `Empty` be a property or method?
      - Since `Optional<T>` is a value type, you can't have a single instance.
      -  While `string.Empty` is a property, `Array.Empty<T>()` and `Enumerable.Empty<T>()` are extensions because properties can't be generic.
      - `Optional.Empty<T>()` complements `Optional.Of<T>()` and hints that a new object is being created. Also, you can pass the method as a delegate.
      - On the other hand, `Span<T>.Empty` is a property and most closely matches this case.
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

## Why no `OnEmpty()` or `IfEmpty`?

These don't really add any value. You can just do:

```cs
if (!optional.HasValue)
{
    // ...
}
```

For `OnValue()` and `IfValue()`, you need a way to get to the underlying value.

# Why not make Optional<T> implement IEnumerable<T>?
- I saw someone explain optional types once as a list that can have either 0 or 1 element. I thought that was an interesting explanation.
- It was straightforward to implement and I thought it was cool to unify the two concepts, so I went for it in a prelease version of `Optional<T>`.
- After trying it out, I found it had two big drawbacks:
  - All of the `IEnumerable<T>` extension methods polluted IntelliSense and made it hard to figure out what you could actually do with `Optional<T>`
  - Iterating over `Optional<T>`'s "elements" breaks its encapsulation and null safety in the same way that putting a `Value` property on it would.

```cs
// We might as well add .Value now!
optional.First()
```

It's still easy to convert `Optional<T>` to `IEnumerable<T>`:

```cs
var optional = optional.Of("hello");

optional.Switch(
    x => new[] { x },
    () => Enumerable.Empty<string>());
```

Since this isn't quite as easy as `new[] { x }` for a non-optional value, I'll add a `ToEnumerable()` method.