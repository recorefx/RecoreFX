---
uid: Recore.Of`1
example:
- *content
---


```cs
class Address : Of<string> {}

var address = new Address { Value = "1 Microsoft Way" };
Console.WriteLine(address); // prints "1 Microsoft Way"
```

You can add @Recore.OfJsonAttribute so that the type is serialized
in the same was as the `T` type:

```cs
using System.Text.Json;

[OfJson(typeof(JsonAddress), typeof(string))]
class JsonAddress : Of<string> {}

var jsonAddress = new JsonAddress { Value = "1 Microsoft Way" };
Console.WriteLine(JsonSerializer.Serialize(address)); // {"value":"1 Microsoft Way"}
Console.WriteLine(JsonSerializer.Serialize(jsonAddress)); // "1 Microsoft Way"
```