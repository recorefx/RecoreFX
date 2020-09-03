---
uid: Recore.Optional`1
example:
- *content
---

Here are a few examples for how to work with an optional value.

`Optional` has public constructors that you can use, but the easiest way to create an instance is to use the helper methods:

```cs
Optional<string> opt;         // Optional is a value type, so this defaults to empty
opt = Optional.Of("hello");   // Creates an Optional with the value "hello"
opt = Optional<string>.Empty; // Now it's empty again
```

`Switch()` is the main way to work with Optional.
It's akin to a switch statement.

```cs
opt.Switch(
    x => Console.WriteLine("Message: " + x),
    () => Console.WriteLine("no message"));
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
If the original `Optional` is empty, the new one will also be empty.

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