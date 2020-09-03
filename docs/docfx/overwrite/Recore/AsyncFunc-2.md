---
uid: Recore.AsyncFunc`2
example:
- *content
---

Say you have a method with the signature

```cs
Task DoThing(int x, string s, Func<int, Task<string>> thing)
```

You can refactor this to

```cs
Task DoThing(int x, string s, AsyncFunc<int, string> thing)
```