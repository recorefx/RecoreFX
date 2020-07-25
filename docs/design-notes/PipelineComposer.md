# Pipeline and Composer

## Motivation
* "Pipelining" and function composition are closely related.
  For example, the following code F# snippets are equivalent:

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

## Why the names?
I also considered:
* Naming them both `Pipeline`
* Naming them both `Composer`
* `Flow` (as in Lodash)

### Naming them both `Pipeline` / `Composer`
I actually moved away from `Pipeline` at first because I felt the term was too overloaded in software.
Even CoreFX already has a `System.IO.Pipelines` namespace.

I had settled on naming them both `Composer` until I got to really using this in the sample app.
I thought that naming them the same thing would simplify concepts.
I actually realized it just made things more confusing, especially with how they differ on eager vs. lazy function invocation.

`Composer` really makes it sound like it should be lazy, so it's confusing to call `Pipeline` that.
Also, I kind of had forgotten the type that is now `Composer` even existed when they were named the same thing.

### `Flow` for `Pipeline`
Lodash's [`flow`](https://lodash.com/docs/4.17.15#flow) function looks similar to `Pipeline` and `Composer`.
It actually works more like `Composer`.
I didn't choose it for two reasons:
* The `flow` metaphor maps better to a function than to a type.
  Naming a type `Flow` and then calling a method on it just doesn't, well, flow right.
* Same reason as above for naming them both `Composer`.

## Why `Then`?
* Elsewhere in the code (`Optional`, for example), `Then` is used for a monad bind operation. In purely functional programming, function composition -- rather than the order of statements (as there are no statements) -- is the way to represent a sequence of operations. In other words, `g(f(x))` means *first* do `f`, *then* do `g`.
* Moreover, the monad bind operation is closely related to function composition -- hence the name `then` being used for the monad bind operation in JavaScript's `Promise` type.

## Why doesn't `Composer` have a `Composer.Of` helper method?