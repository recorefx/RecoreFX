# Contributor's Guide

## Design principles

The design principles of RecoreFX are:
1. Generally useful
2. Common sense-ness
3. Follows the programming paradigm of standard C#
 
The types in RecoreFX are meant to be **generally useful**.  This means that they can be used in any C# code, regardless of application. It shouldn't matter if it's a console app, Web app, script, or even another library.

C# takes a pragmatic approach to programming. It is an everyman's and everywoman's programming language. It is flexible yet safe, aiming to maximizing ease of understanding and expressivity while scaling out to large codebases. Above all, developer productivity must be honored, both for long-time fans and for new programmers ramping up on a project. These are the virtues of **common sense-ness**.

RecoreFX **follows the programming paradigm of standard C#**. While C# started out as a purely object-oriented language, it's grown some functional features as well: LINQ, lambda expressions, local functions, and pattern matching, to name a few. And to be sure, a lot of people write C# in a purely procedural style. RecoreFX's API is meant to fit into this paradigm: an object-oriented core, functional when it suits it, and always possible to drop down to a procedural level when necessary.

## Style guide

See [contributors-guide/coding-style.md](contributors-guide/coding-style.md).

## Versioning

This project uses [semantic versioning](https://semver.org/).

See <https://docs.microsoft.com/dotnet/standard/library-guidance/versioning>.

## Releases

See [contributors-guide/releases.md](contributors-guide/releases.md).