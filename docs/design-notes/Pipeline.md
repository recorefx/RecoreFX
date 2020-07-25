# Composer

## Why the name?
* Sure, it's not really composing functions. That's unfortunately clumsy to do in C# because of the weak type inference for generics.
* This works more like the pipeline operator `|>` in F#.
* I originally named this `Pipeline`, but felt like this term is already widely used and was too likely to clash with other types. CoreFX also already has the `System.IO.Pipelines` namespace.
* "Pipelining" is closely related to function composition. For example, the following code F# snippets are equivalent:

```fs
// Normal function calls
List.sum ((List.map square) [1..100])
```

```fs
// Pipelining
[1..100]
|> List.map square
|> List.sum
```

```fs
// Function composition
((List.map square) >> List.sum) [1..100]
```

## Why `Then`?
* Elsewhere in the code (`Optional`, for example), `Then` is used for a monad bind operation. In purely functional programming, function composition -- rather than the order of statements (as there are no statements) -- is the way to represent a sequence of operations. In other words, `g(f(x))` means *first* do `f`, *then* do `g`.
* Moreover, the monad bind operation is closely related to function composition -- hence the name `then` being used for the monad bind operation in JavaScript's `Promise` type.