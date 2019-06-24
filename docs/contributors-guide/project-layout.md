# Project layout

The `msbuild/` directory at the repository root contains common configuration for source and test projects.

The `src/` directory of the repository is laid out according to the components that make up the RecoreFX library.  Each directory follows a regular structure:

```
/src/
  - Namespace
    - src
      - Namespace.csproj
      - File.cs
    - test
      - Namespace.Tests.csproj
      - FileTests.cs
    - build.props
```

Currently, the test projects have to be added to the .sln file manually because Visual Studio will not accept them.
Only the .csproj itself needs to be added -- you can move it to the "Tests" folder from within Visual Studio.