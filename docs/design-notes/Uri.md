# AbsoluteUri and RelativeUri

## Why none of the other `TryCreate()` overloads?

[`TryCreate()`](https://docs.microsoft.com/en-us/dotnet/api/system.uri.trycreate?view=netcore-3.1) has the following overloads:

```cs
public static bool TryCreate (string uriString, UriKind uriKind, out Uri result);
public static bool TryCreate (Uri baseUri, string relativeUri, out Uri result);
public static bool TryCreate (Uri baseUri, Uri relativeUri, out Uri result);
```

The overloads that provide `baseUri` [must always return an absolute URI](https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Runtime/tests/System/Uri.CreateUriTests.cs#L108).
Therefore, we can convert that output to an `AbsoluteUri` with `AsAbsoluteUri()`.