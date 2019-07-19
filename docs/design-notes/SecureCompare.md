# SecureCompare

## Why the name `SafeEquals`?
* Normal `Equals` risks calling `Object.Equals`, which is still visible on the type, by accident.
* Other names considered: `CheckAllEqual`, `TimeInvariantEquals`
  * `SafeEquals` best captures that normal `Equals` is "unsafe" but runs faster (like a C# `unsafe` block).

## Why accept `Byte[]` instead of just `Span<byte>`?
* `Span<T>` is not available in .NET Standard 2.0, so `Byte[]` is needed to support older runtimes (including all .NET Framework versions).