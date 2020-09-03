---
uid: Recore.Unit
example:
- *content
---

Imagine a type `ApiResult<T>` that wraps the deserialized JSON response from a REST API.
Without `Unit`, you'd have to define a separate, non-generic `ApiResult` type for when the response doesn't have a body:

```cs
ApiResult postResult = await PostPersonAsync(person);
ApiResult<Person> getResult = await GetPersonAsync(id);
```

With `Unit`, you can just reuse the same type:

```cs
ApiResult<Unit> postResult = await PostPersonAsync(person);
ApiResult<Person> getResult = await GetPersonAsync(id);
```

In the definition of `PostPersonAsync()`, just return `Unit.Value`:

```cs
ApiResult<Unit> PostPersonAsync(Person person)
{
    // ...
    return new ApiResult<Unit>(Unit.Value);
}
```