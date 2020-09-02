---
uid: Recore.ObjectExtensions.StaticCast*
example:
- *content
---


Useful for overcoming those pesky variance issues:

```cs
Task<IEnumerable<object>> GetObjectsAsync()
{
    var result = new[] { "hello" };
    
    // error CS0029 because the static type of `result` is an array, not an `IEnumerable`
    // return Task.FromResult(result);

    // 👍
    return Task.FromResult(result.StaticCast<IEnumerable<object>>());
}
```