# Argmin / Argmax

## Why return a tuple with the max / min along with the argmax / argmin?
Sure, perhaps it sounds like a clumsy name on the surface: why would you call `Argmax` to get the max?
But if you just wanted the max by itself, you would just call the standard LINQ `Max`.
So in every sane case where you're calling `Argmax`, you do in fact want the argmax.
Since you have to compute the max along the way, it saves you another trip through the sequence
to return the max.

Of course, the ideal solution would have been for the built-in methods to just work like `Argmax` / `Argmin`.
