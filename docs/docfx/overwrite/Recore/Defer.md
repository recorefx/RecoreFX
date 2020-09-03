---
uid: Recore.Defer
example:
- *content
---

@Recore.Defer is for ad-hoc [RAII](https://en.wikipedia.org/wiki/Resource_acquisition_is_initialization).

Say you want to perform some action before you exit a method, regardless of whether you return normally or throw an exception. This is usually something like releasing a resource that was acquired in the method.

The classic way to do this in C# is with `try-finally`:

```cs
try
{
    Console.WriteLine("Doing stuff");
}
finally
{
    Console.WriteLine("Running cleanup");
}
```

This isn't bad, but it adds an extra level of indentation and 6 extra lines for the `try-finally`.
With @Recore.Defer and C# 8's new `using` declarations, we can do it more simply:

```cs
using var cleanup = new Defer(() => Console.WriteLine("Running cleanup"));
Console.WriteLine("Doing stuff");
```