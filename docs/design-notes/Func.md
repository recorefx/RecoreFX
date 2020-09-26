# Func

## Why not `InvokeAsync()`?

If you have an `AsyncFunc`, you can pass it to `Invoke()` and await the result.
As far as I can tell, having an `InvokeAsync()` that internally `await`s the call
will just add that extra line with the `await` from inside `Recore.Func` to the stack trace.