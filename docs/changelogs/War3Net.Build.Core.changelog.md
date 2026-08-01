# War3Net.Build.Core Changelog

All notable changes to the `War3Net.Build.Core` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## v6.0.3

### Changed

- Updated `War3Net.IO.Mpq` from v6.0.2 to v6.0.3.

## v6.0.2

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

## v6.0.1

_No functional changes; package readme and metadata updated for nuget.org presentation._

## v6.0.0

### Breaking Changes

- The `EscapedStringProvider` class has been moved to `War3Net.CodeAnalysis.Jass`.

### Added

- Added `W3MathF` class.

### Fixed

- Fixed exception when deserializing `MapInfo.EditorVersion` as JSON string.

## v1.5.3

### Added

- Add `UseNewFormat` property to `MapCustomTextTriggers`.

## v1.5.2

### Added

- Support parsing and serializing `.wct` files.

## v1.5.1

### Added

- Add `SoundFlags.UNK16` flag.

### Fixed

- Fix `MapTriggers` serializer for old format could serialize `TriggerItemType.RootCategory` items.

## v1.5.0

### Added

- Support parsing and serializing `.wtg` files.
- Support additional format versions of `.w3i` files.
- Include 1.32.9 in `GamePatch` enum.

### Changed

- Update target framework from .NET Standard to .NET Core.

## v1.4.0

### Breaking Changes

- `FileProvider` class has been moved to `War3Net.IO.Mpq` namespace.
- Renamed `GamePatchVersionProvider.GetPatchVersion` to `GetGameVersion`.

### Added

- Added `GetTerrainTypes` and `GetCliffTypes` methods to `MapEnvironment`.
- Added `GetGamePatch` method to `GamePatchVersionProvider`.

### Changed

- Updated `War3Net.IO.Mpq` and `CSharpLua` packages.

### Fixed

- Fix parse error in `war3mapUnits.doo` when random data mode is -1.

## v1.3.10

### Added

- Add event `OnArchiveBuilding` to `MapBuilder`.

## v1.3.9

### Fixed

- Fix lua global declarations.

## v1.3.8

### Fixed

- Fix string interpolation uses incorrect format string.

## v1.3.7

### Added

- Include 1.32.8 in `GamePatch` enum.

### Changed

- Preplaced units are now assigned to a global variable.

## v1.3.6

_Initial version._
