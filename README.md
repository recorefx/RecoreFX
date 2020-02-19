# RecoreFX

[![CI](https://github.com/brcrista/RecoreFX/workflows/CI/badge.svg)](https://github.com/recorefx/RecoreFX/actions?query=workflow%3ACI)
[![Build Status](https://dev.azure.com/briancristante/RecoreFX/_apis/build/status/RecoreFX?branchName=master)](https://dev.azure.com/briancristante/RecoreFX/_build/latest?definitionId=11&branchName=master)

**RecoreFX** fills the most common needs for C# code after the .NET standard library.

Recore includes a variety of [collection](src/Recore.Collections.Generic) and [LINQ](src/Recore.Linq) extensions as well as some [best practices](src/Recore.Security.Cryptography/SecureCompare.cs) that most codebases end up implementing from scratch.
Recore also brings some F#-specific features, such as [`Optional`](src/Recore/Optional.cs) and [`Of`](src/Recore/Of.cs), to C# and other languages targeting .NET.

## Installation
Install from [NuGet](https://www.nuget.org/packages/RecoreFX/):

```
dotnet add package RecoreFX
```

## Why Recore?
There are other packages offering more LINQ-style methods, better URI handling, or implementations of `Optional`.
These packages admittedly have richer feature sets than Recore has, and may be right for your project if you want that depth of functionality.

RecoreFX is designed to feel like a natural extension of CoreFX.
It's meant to be a one-stop shop, filling in the most common use cases without a steep learning curve.

## API documentation
<https://recorefx.github.io/>
