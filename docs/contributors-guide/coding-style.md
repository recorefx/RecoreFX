# C# Coding Style

The coding style largely follows the [C# coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) and the [CoreFX style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md). For anything not specified, reference those guidelines.

The guidelines here are considered most important.
Many of them agree with the above styles, but some are different.

1. **Naming**
  * Standard C# / .NET naming:
    * Type names are `PascalCase`.
    * Public members are `PascalCase`.
    * Private members are `camelCase`.
    * Parameters and local variables are `camelCase`.

2. **Formatting**
  * In general, use the default formatting from Visual Studio.
  * Indent using spaces with a width of 4 per level.
  * Avoid needless whitespace: trim all whitespace after the last non-whitespace character on a line (VS Code's "Trim trailing whitespace" command is helpful).
  * Do not use more than one consecutive blank line.

3. **Braces**
  * Use Allman-style braces.
  * `if`, `else`, `for`, `foreach`, `while`, `do`, and `using` statements must all have braces.  The one exception is when two or more `using` statements govern the same block of code.

4. **Comments**
  * Comments must be in clear, correct, idiomatic English.
  * All public classes must have a doc comment with at least a one-line summary.
  * All public methods must have a doc comment with at least a one-line summary.
  * Prefer multiple single-line comments (`//`) over block comments (`/* */`).
  * Single-line comments must have a single space after the `//`.
  * Please comment frequently. The best comments explain *why* something is done the way it is.

5. **Type declarations**
  * Always use C# keywords (`string`) over BCL types (`String`).
  * `var` is always acceptable, but not required.
  * Whether to use `var` or an explicit type is up to the author. The one exception is when assigning a newly-created instance of a class or struct to a variable.
  In that case, `var` must always be used. (`var foo = new Foo();`)
