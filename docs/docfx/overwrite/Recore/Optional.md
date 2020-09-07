---
uid: Recore.Optional`1
example:
- *content
---

Here are a few examples for how to work with an optional value.

@Recore.Optional`1 has public constructors that you can use, but the easiest way to create an instance is to use the helper methods:

```cs
Optional<string> opt;         // Optional is a value type, so this defaults to empty
opt = "hello";                // Creates an Optional with the value "hello"
opt = Optional<string>.Empty; // Now it's empty again
```

`Switch()` is the main way to work with Optional.
It's akin to a switch statement.

```cs
opt.Switch(
    x => Console.WriteLine("Message: " + x),
    () => Console.WriteLine("No message"));
```

You can also return a value like a switch expression.
In this case, both legs of the `Switch()` must return the same type.

```cs
int messageLength = opt.Switch(
    x => x.Length,
    () => -1);
```

But, a more idiomatic way to handle the above case is to use `OnValue()`.
This will map `Optional<T>` to `Optional<U>`.
If the original @Recore.Optional`1 is empty, the new one will also be empty.

```cs
Optional<int> messageLength = opt.OnValue(x => x.Length);
```

We can work with the value procedurally, but this doesn't get us access to the value.
For that, we can use `AssertValue()`.
It will try to retrieve the value and throw `InvalidOperationException` if there's no value.

```cs
if (opt.HasValue)
{
    string value = opt.AssertValue();
}
```

A safer way to get the value out is to use `ValueOr()`, which requires you to pass a fallback value.

```cs
string message = opt.ValueOr(default);
```

There's a special case where `OnValue()` and `Then()` can cause their callback to work differently than it does outside the `Optional` context. Consider these two calls:

```cs
int? value = null;

new Optional<int>(value ?? 0)
new Optional<int?>(value).OnValue(x => x ?? 0)
```

Normally, you'd expect these two lines to be equivalent for any function.
But if `value` is `null`, they won't be the same.

The same thing goes for `Then()`:

```cs
Optional<int> NullableToOptional(int? n) => Optional.Of(n ?? 0);

NullableToOptional(value)
new Optional<int?>(value).Then(NullableToOptional)
```

The problem is that "null coalescing" operations (by which I mean any function that turns `null` into a non-`null` value, including the `??` operator) won't work as expected when passed through `Optional<T>`.
This is because they will never receive `null` as an input.

(A note to functional-programming aficionados: the first example has implications for associativity.
Passing a composed function to `OnValue()` may produce a different result than two separate `OnValue()` calls if null coalescing is involved.
The second example is the monad law of left identity.)

Since any reference type can be `null`, there's no way to fix it except to avoid it.
Just know that you **can't perform null-coalescing inside of an `Optional` context**.
Rather, use `Optional`'s own operations.
Every example I can think of stands out as a bug if you examine it under this principle.

For more information, [this writeup](https://www.sitepoint.com/how-optional-breaks-the-monad-laws-and-why-it-matters) explores the issue in the context of Java's `optional` type.