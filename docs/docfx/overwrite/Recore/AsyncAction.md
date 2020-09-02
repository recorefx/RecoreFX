---
uid: Recore.AsyncAction
example:
- *content
---

Say you have a method with the signature

```cs
Task DoThing(int x, string s, Func<Task> thing)
```

You can refactor this to

```cs
Task DoThing(int x, string s, AsyncAction thing)
```