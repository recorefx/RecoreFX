---
uid: Recore.AsyncAction`1
example:
- *content
---

Say you have a method with the signature

```cs
Task DoThing(int x, string s, Func<int, Task> thing)
```

You can refactor this to

```cs
Task DoThing(int x, string s, AsyncAction<int> thing)
```