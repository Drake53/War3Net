# Version Update Script Guide

## Overview

The `tools/update-versions.sh` script automates the process of updating package versions in `Directory.Packages.props` based on semantic versioning (semver) principles. It automatically determines cascading version bumps for dependent packages.

## Usage

```bash
./tools/update-versions.sh "package1:change_type,package2:change_type,..." [--skip-nuget-check]
```

### Change Types

- `breaking` - Major version bump (x.0.0) - breaking changes
- `feature` - Minor version bump (x.y.0) - new features
- `fix` - Patch version bump (x.y.z) - bug fixes

### Examples

```bash
# Single package update
./tools/update-versions.sh "War3Net.Common:feature"

# Multiple package updates
./tools/update-versions.sh "War3Net.Common:feature,War3Net.IO.Mpq:fix"

# Breaking change example
./tools/update-versions.sh "War3Net.Build.Core:breaking"
```

### Published Version Skipping

GitHub releases are tagged with the highest version published in that release. To keep these tags unique, the script queries NuGet.org and never assigns a version that was previously published by *any* publishable package: after the semver bump, it patch-increments past any taken version. For example, if package A already published 1.0.1, a `fix` bump of package B from 1.0.0 lands on 1.0.2.

Versions that are only pending in `Directory.Packages.props` (not yet on NuGet.org) are not skipped, so packages bumped before the same release may share a version.

Use `--skip-nuget-check` to skip the NuGet.org queries (e.g. offline). This risks assigning an already-published version, which breaks release tagging.

### AI Instruction Example

Update the package versions in this solution by determining the changes since the last release and then running the script.