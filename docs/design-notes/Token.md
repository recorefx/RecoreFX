# Token

I used to have this as its own type, but I refactored it to be a subtype of `Of<string>` since it's really using that pattern and a lot of code could be eliminated.

I also used to have overloads for its methods taking `string` and an implicit conversion operator to `string` so `Token` would feel more like a subtype of `string`.
I decided I didn't want to add all that to `Of<T>`, so I deleted it here.
Though, it wouldn't be a breaking change to add that later if it's desirable.