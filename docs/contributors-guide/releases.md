# Releases

## Versioning
This project uses [semantic versioning](https://semver.org/).

## Branching strategy
This project uses [trunk-based development](https://trunkbaseddevelopment.com/).
Once changes are accepted in a (hopefully compact) PR,
they are merged directly to master.

When it is time to release a new version (major.minor) of the project,
a branch is made off of master for that version.
This branch is named `release/major.minor`.

This is a more general approach than just tagging commits on master
(as a branch with no additional commits is basically the same as a tag)
and makes it easy to backport changes to an old major.minor version.

When backporting a change, the change should usually go in master first
and then be cherry-picked to the `release/major.minor` branch.
Patch versions are then formed off of the subsequent commits
to the `release/major.minor` branch.
Since versions are immutable and there are no verion specifiers after "patch,"
patch versions can be identified simply with tags. 