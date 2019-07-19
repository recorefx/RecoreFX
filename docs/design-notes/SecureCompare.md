# SecureCompare

## Why the name `TimeInvariantEquals`?
* Normal `Equals` risks calling `Object.Equals`, which is still visible on the type, by accident.
* Other names considered: `CheckAllEqual`, `SafeEquals`
  * `SafeEquals` best captures that normal `Equals` is "unsafe" but runs faster (like a C# `unsafe` block).
  * "Safety" is usually applied to logical correctness, not confidentiality (for example, "type safety," "memory safety").
  * `TimeInvariantEquals` is the most descriptive. It explains exactly what the method is supposed to do.

## Why accept `Byte[]` instead of just `Span<byte>`?
* `Span<T>` is not available in .NET Standard 2.0, so `Byte[]` is needed to support older runtimes (including all .NET Framework versions).