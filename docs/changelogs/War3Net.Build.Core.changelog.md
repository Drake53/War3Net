# War3Net.Build.Core Changelog

All notable changes to the `War3Net.Build.Core` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

_No unreleased changes._

## [v6.0.3] - 2026-07-04

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

### Changed

- Updated `War3Net.IO.Mpq` from v6.0.2 to v6.0.3.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Breaking Changes

- The `EscapedStringProvider` class has been moved to `War3Net.CodeAnalysis.Jass`.

### Added

- Added `W3MathF` class.

### Fixed

- Fixed exception when deserializing `MapInfo.EditorVersion` as JSON string.

## v1.5.3 - 2020-10-29

### Added

- Add `UseNewFormat` property to `MapCustomTextTriggers`.

## v1.5.2 - 2020-10-29

### Added

- Support parsing and serializing `.wct` files.

## v1.5.1 - 2020-10-28

### Added

- Add `SoundFlags.UNK16` flag.

### Fixed

- Fix `MapTriggers` serializer for old format could serialize `TriggerItemType.RootCategory` items.

## v1.5.0 - 2020-10-27

### Added

- Support parsing and serializing `.wtg` files.
- Support additional format versions of `.w3i` files.
- Include 1.32.9 in `GamePatch` enum.

### Changed

- Update target framework from .NET Standard to .NET Core.

## v1.4.0 - 2020-09-14

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

## v1.3.7 - 2020-08-22

### Added

- Include 1.32.8 in `GamePatch` enum.

### Changed

- Preplaced units are now assigned to a global variable.

## v1.3.6 - 2020-08-09

### Added

- Initial release. Useful files were moved out of the `War3Net.Build` package into this new package, to reduce dependencies.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.3]: https://github.com/Drake53/War3Net/releases/tag/v6.0.3
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
