# RecoreFX

[![CI](https://github.com/brcrista/RecoreFX/workflows/CI/badge.svg)](https://github.com/recorefx/RecoreFX/actions?query=workflow%3ACI)
[![Build Status](https://dev.azure.com/briancristante/RecoreFX/_apis/build/status/RecoreFX?branchName=master)](https://dev.azure.com/briancristante/RecoreFX/_build/latest?definitionId=8&branchName=master)

**RecoreFX** fills the most commonly-needed missing features from the .NET standard library.

Recore includes a variety of [collection](src/Recore.Collections.Generic) and [LINQ](src/Recore.Linq) extensions as well as some [best practices](src/Recore.Security.Cryptography/SecureCompare.cs) that most codebases end up implementing from scratch.
Recore also brings some F#-specific features, such as [`Optional`](src/Recore/Optional.cs) and [`Of`](src/Recore/Of.cs), to C# and other languages targeting .NET.

## Why Recore?
There are many other great libraries out there for .NET, and Recore duplicates some of their functionality.
Recore seeks to fill in the most commonly needed missing features from CoreFX.

Recore is meant to feel like a natural extension of CoreFX and is built on the same principles.

## API documentation
<https://recorefx.github.io/>
