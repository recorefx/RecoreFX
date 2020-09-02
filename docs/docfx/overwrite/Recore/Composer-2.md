---
uid: Recore.Composer`2
example:
- *content
---

Without @Recore.Composer`2:

```cs
var result = Baz(Bar(Foo(value)));
```

With @Recore.Composer`2:

```cs
var result = new Composer<string, int>(Foo)
    .Then(Bar)
    .Then(Baz)
    .Func(value);
```