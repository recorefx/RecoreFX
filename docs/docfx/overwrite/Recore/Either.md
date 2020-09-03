---
uid: Recore.Either`2
example:
- *content
---

@Recore.Either`2 creates a type with a value that can be one of two types.
In this, it's similar to @Recore.Optional`1,
which you can think of like `Either<T, null>` with some extra functionality.

If you're familiar with TypeScript, you can think of @Recore.Either`2 as a union type:

```ts
TLeft | TRight
```

You create an instance of @Recore.Either`2 with one of its constructors, but the implicit conversion operator is pretty convenient too:

```cs
Either<string, int> either = "hello";
```

Like @Recore.Optional`1, the main way to work with
@Recore.Either`2 is with
`Switch()`:

```cs
either.Switch(
    l => Console.WriteLine($"Value is a string: {l}"),
    r => Console.WriteLine($"Value is an int: {r}"));
```

you can also return a value:

```cs
var message = either.Switch(
    l => $"Value is a string: {l}",
    r => $"Value is an int: {r}");
```

Compared to @Recore.Optional`1, though,
where `Switch()` is more of a last resort when no higher-level idiom is available,
@Recore.Either`2
leans on `Switch()` heavily.
You can think of @Recore.Either`2 as being lower-level than
@Recore.Optional`1.

@Recore.Either`2 also has
`OnLeft()` and `OnRight()`,
analogous to @Recore.Optional`1's
`OnValue()`. (Note that there's no `OnEmpty()`; you can just use `if (!opt.HasValue)` for that.)

```cs
Either<string, bool> newEither = either.OnRight(x => x > 0);
```