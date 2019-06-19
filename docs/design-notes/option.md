# Option

## Why `readonly struct`?
- The type is meant to safely handle null. Making it a value type means instances of the type itself can't be null.
- `ref` structs are "stack-only" values and can't live as instances of another type (unless that type is also a `ref` struct).

## Why `where T : class`?
- This simplifies handling of empty options.
Without this constraint, you have to maintain additional state to know whether the option is set or not.
- Removing this constraint in the future would not be a breaking change.

## Why the names of methods?
- `Switch`: also considered `Match`, `Choose`, `Pick`, `Select`
    - `Match` was what I originally called this since the method is doing what pattern matching does in other languages.  However, pattern matching is a language feature, not an `Option` feature.
    - The name of the method has to describe the two usually anonymous functions that follow. `Switch` accomplishes this by evoking the `switch` statement in the language.
    - It's also worth noting that C# has implemented pattern matching with the `switch` statement.
- `Try`: also considered `Map`
    - Again, going with the name of an established concept. `Map` isn't so bad, but wouldn't be evocative for users unfamiliar with functional programming.
    - `Try` clearly evokes that something still happens in the empty case, even though only one function is passed.
- `Then`: also considered `Bind`
    - Again, `Bind` isn't evocative for users unfamiliar with functional programming.
    - `Then` evokes a monad bind that many people are familiar with, that of Promises in JavaScript. .NET's own `ContinueWith` is a little too clumsy and more specific to asynchronous programming.