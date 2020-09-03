---
uid: Recore.AsyncFunc`1
example:
- *content
---

Say you have a method with the signature

```cs
Task DoThing(int x, string s, Func<Task<string>> thing)
```

You can refactor this to

```cs
Task DoThing(int x, string s, AsyncFunc<string> thing)
```