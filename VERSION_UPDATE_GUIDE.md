# Version Update Script Guide

## Overview

The `update-versions.sh` script automates the process of updating package versions in `Directory.Build.props` based on semantic versioning (semver) principles. It automatically determines cascading version bumps for dependent packages.

## Usage

```bash
./update-versions.sh "package1:change_type,package2:change_type,..."
```

### Change Types

- `breaking` - Major version bump (x.0.0) - breaking changes
- `feature` - Minor version bump (x.y.0) - new features
- `fix` - Patch version bump (x.y.z) - bug fixes

### Examples

```bash
# Single package update
./update-versions.sh "War3Net.Common:feature"

# Multiple package updates
./update-versions.sh "War3Net.Common:feature,War3Net.IO.Mpq:fix"

# Breaking change example
./update-versions.sh "War3Net.Build.Core:breaking"
```

### AI Instruction Example

Update the package versions in this solution by determining the changes since the last release and then running the script.