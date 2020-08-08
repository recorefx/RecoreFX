# Releases

## Goals

The release process is governed by two principles:
1. All releases should have a Git tag with a name like `V$MAJOR.$MINOR.$PATCH`.
1. Branch history when new sub-versions need to be added.

This is essentially [trunk-based development](https://trunkbaseddevelopment.com).
While having a tag and a branch at the same commit is technically redundant,
GitHub looks at tags for listing releases.

The second one ensures that the "breaking change thresholds" are preserved for each version.
This way, the mainline branch can include any potentially breaking changes.

History ends up looking like this (omitting untagged commits):

```
|
* <- v1.0.0
|\
| * <- v1.1.0
| |\
| | * <- v1.1.1
| | |
| | * <- v1.1.2 (releases/v1.1)
| * <- v1.2.0 (releases/v1)
|
* <- v2.0.0 (main)
```

## How to release

Do this for every release:
- [ ] Bump the `AssemblyVersion`, `FileVersion`, and `Version` properties in `Recore.csproj`.
- [ ] Add a changelog under the `docs/changelogs` directory.
- [ ] Submit a PR with the above two changes.
- [ ] Once the PR as merged, create a release on GitHub.
    - [ ] Follow the instructions below for the tagging strategy.
    - [ ] Paste the changelog in the release description.
- [ ] Once the release is published, upload the package to NuGet.

### New major version

To release a new major version, just tag `main` at that point with a tag like `v1.0.0`.

### New minor version

When releasing a `.1` minor version (say `v1.1.0`), check out a new branch at `v1.0.0` named `releases/v1`.

```bash
git checkout v1.0.0
git checkout -b releases/v1
```

Add the commits that will go in that release on that branch.
Then, tag the `v1` branch with `v1.1.0` on the commit to be released.

For subsequent minor versions, just add the commits to the `releases/v1` branch and tag like `v1.1.0`.

### New patch version

Similar to the new minor version:

```bash
git checkout v1.1.0
git checkout -b releases/v1.1
```

then add the commits and tag like `v1.1.1`.