# Defer

## Why `sealed class`?
Since the type is disposable, it is meant to be referenced through the `IDisposable` interface.
If this were a value type, it would get boxed when referenced through the interface.

## Why not define a finalizer to ensure the callback has been called?
Throwing an exception from a finalizer will terminate the process,
so it wouldn't be a good idea to invoke an arbitrary action in this case.

Also, finalizers increase memory overhead, since the object will always survive the first GC attempt
and go to the next generation.