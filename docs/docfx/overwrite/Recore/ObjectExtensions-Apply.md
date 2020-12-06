---
uid: Recore.ObjectExtensions.Apply``2(``0,System.Func{``0,``1})
example:
- *content
---

This method is useful for calling a function with postfix syntax:

```cs
var syncClient = new ServiceCollection()
    .Apply(ConfigureServices)
    .Apply(x => x.BuildServiceProvider())
    .Apply(x => x.GetService<SyncClient>());
```

It can also be used to simplify null-propagation logic
when the `?.` operator cannot be used:

```cs
// Before
var route = contentEndpoint is null ? null : $"{contentEndpoint}?path={forwardSlashPath}";

// After
var route = contentEndpoint?.Apply(x => $"{x}?path={forwardSlashPath}");
```