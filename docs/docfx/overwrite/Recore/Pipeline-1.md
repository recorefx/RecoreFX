---
uid: Recore.Pipeline`1
example:
- *content
---

Without @Recore.Pipeline`1:

```cs
var result = Baz(Bar(Foo(value)));
```

With @Recore.Pipeline`1:

```cs
var result = Pipeline.Of(value)
    .Then(Foo)
    .Then(Bar)
    .Then(Baz)
    .Result;
```