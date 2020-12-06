# Token

In Recore v1, this was a subtype of `Of<string>` for simplicity.
This is undesirable with nullable references enabled, since `Of<T>.Value` is nullable.
Therefore, I changed it to be a record type.