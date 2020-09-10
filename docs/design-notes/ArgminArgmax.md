# Argmin / Argmax

## Why return a tuple with the max / min along with the argmax / argmin?
Sure, perhaps it sounds like a clumsy name on the surface: why would you call `Argmax` to get the max?
But if you just wanted the max by itself, you would just call the standard LINQ `Max`.
So in every sane case where you're calling `Argmax`, you do in fact want the argmax.
Since you have to compute the max along the way, it saves you another trip through the sequence
to return the max.

Of course, the ideal solution would have been for the built-in methods to just work like `Argmax` / `Argmin`.

## Is `Argmax().Max` and `Argmin().Min` always equivalent to `Max()` and `Min()`?

The short answer is yes, but there is a corner case.
For the overloads that operate on a list of nullables,
the `Max()` or `Min()` of an empty sequence is null,
whereas for a list of value types, trying to call `Max()` or `Min()` on an empty sequence results in an `InvalidOperationException`.

The confusing bit comes with the overloads that take a `selector` that returns a nullable value.
The `Max()` or `Min()` of an empty sequence here is still null.
But now, the input type is not necessarily nullable.
One option is just to make the input type nullable to accommodate this one special case, but that complicates things quite a bit because you need to provide overloads for value and reference types separately.
Another option is to return `default` in this case.
This keeps the implementation the most similar to `Max()` and `Min()`, but doesn't make much sense when you actually hit that case.

The best option in my opinion is to do what the generic `Max(IEnumerable<TSource>)` implementation does:
if `TSource` is a value type, throw `InvalidOperationException`; if it is a reference type, return `null`.