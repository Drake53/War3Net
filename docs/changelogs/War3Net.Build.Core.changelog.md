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

## v5.8.2 - 2025-09-27

### Added

- Added the game build data for patch 2.0.3 that was still missing after v5.8.0 (#82).

## [v5.8.1] - 2025-09-12

### Changed

- Updated `War3Net.IO.Mpq` and `War3Net.IO.Slk` from v5.8.0 to v5.8.1.

### Fixed

- Parsing a map with an unknown editor version no longer throws (#77).

## [v5.8.0] - 2025-09-06

### Added

- Added support for patch 2.0.3, and added the latest game patches and build data (#68, #73).
- Added support for Linux/WSL, including case-insensitive path handling (#69).
- Backported general improvements from the vJASS branch (#71).

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `War3Net.IO.Mpq`, and `War3Net.IO.Slk` from v5.6.1 to v5.8.0.

## v5.7.1 - 2023-01-19

### Fixed

- Fixed `MapBuilder` not adding skin object data files to the MPQ archive.

## v5.7.0 - 2023-01-08

### Breaking Changes

- Merged the separate map and campaign classes that held identical data into a single class each.
- The JSON deserialization methods no longer take a `TriggerData` parameter; it is only needed for binary deserialization.
- Moved `CampaignExtensions` into this package.

### Added

- Added `SkinObjectData` to the `Map` and `Campaign` classes.
- Added `FileExtension` constants.
- Added binary read/write extension methods for `MapCustomTextTriggers` that use the default encoding.

## v5.6.1 - 2023-01-07

### Added

- Added JSON serialization and deserialization support for the remaining `War3Net.Build.Core` types, including `MapTriggers`, backed by a set of custom `JsonConverter`s.

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `War3Net.IO.Mpq`, and `War3Net.IO.Slk` from v5.6.0 to v5.6.1.

### Fixed

- Fixed JSON serialization of `UnitData.RandomData`, and JSON deserialization of `UnitData`, `DoodadData`, and `TerrainTile`.

## v5.6.0 - 2022-12-20

### Breaking Changes

- Renamed the format version enum members for consistency.

### Added

- Added JSON serialization and deserialization for `MapInfo`, including `JsonMapInfoConverter` and a `JsonSerializerOptions` parameter on the serialize methods.
- Added patches 1.34.0 and 1.35.0 to `GamePatch`, along with the new retail game builds.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` and `War3Net.IO.Mpq` from v5.5.5 to v5.6.0, and `War3Net.IO.Slk` from v5.5.6 to v5.6.0.

## v5.5.7 - 2022-12-14

### Added

- Added `EditorVersion` v6115.

## v5.5.6 - 2022-11-30

### Changed

- Updated `War3Net.IO.Slk` from v5.5.5 to v5.5.6.

## v5.5.5 - 2022-11-13

### Breaking Changes

- Changed the underlying type of the `Tileset` enum to `byte`.

### Added

- Added `TerrainTypeProvider.GetDefaultTerrainType`.

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `War3Net.IO.Mpq`, and `War3Net.IO.Slk` from v5.5.3 to v5.5.5.

## v5.5.4 - 2022-10-29

### Breaking Changes

- Changed the underlying type of the `ImportedFileFlags` enum to `byte`.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `War3Net.IO.Mpq`, and `War3Net.IO.Slk` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Added

- Added support for reading and writing `.wgc` (campaign gameplay constants) files.
- Added the new 1.33.0 game builds.
- Added missing filenames to the `DiscoverFileNames` extension method.

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `War3Net.IO.Mpq`, and `War3Net.IO.Slk` from v5.5.0 to v5.5.2.

## v5.5.1 - 2022-08-23

### Added

- Added the `war3mapSkin` filenames to the `DiscoverFileNames` extension method.

### Fixed

- Fixed a bug in the `ObjectDataFormatVersion` v3 support added in v5.5.0.

## v5.5.0 - 2022-08-20

### Added

- Added support for patch 1.33.0 and the reforged world editor versions.
- Added support for `ObjectDataFormatVersion` v3.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` and `War3Net.IO.Mpq` from v5.4.5 to v5.5.0, and `War3Net.IO.Slk` from v5.4.0 to v5.5.0.

## v5.4.5 - 2022-05-27

### Breaking Changes

- Changed `MapInfo.EditorVersion` from `int` to the `EditorVersion` enum.
- Replaced `GamePatchVersionProvider` with `GameBuildsProvider`, backed by a new `GameBuilds.json` resource.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.4.1 to v5.4.5, and `War3Net.IO.Mpq` from v5.4.3 to v5.4.5.

### Fixed

- Fixed a parse error for `MapFlags`.

## v5.4.4 - 2022-05-23

### Fixed

- Fixed `.w3f` (campaign info) parsing.

## v5.4.3 - 2022-04-25

### Fixed

- Fixed an `ObjectDisposedException` on `MpqFile.MpqStream.BaseStream` when the `MpqFile` was created through the map extension methods.

## v5.4.2 - 2022-04-24

### Changed

- Updated `War3Net.IO.Mpq` from v5.4.0 to v5.4.3.

### Fixed

- `ObjectDataModification` no longer uses `ValueAsBool`/`ValueAsChar`, which produced incorrect values for some object data types.

## v5.4.1 - 2022-02-13

### Added

- Moved `MapBuilder` into this package.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.4.0 to v5.4.1.

## v5.4.0 - 2022-02-13

### Added

- Added `TriggerRenderer`, for rendering `MapTriggers` back to a script.
- `MapScriptBuilder` now uses the newly identified sound properties, and generates enemy priority flags for `PlayerData`.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` and `War3Net.IO.Mpq` from v5.3.0 to v5.4.0, and `War3Net.IO.Slk` from v0.1.3 to v5.4.0.

### Fixed

- Fixed the player colors.
- Fixed `MapScriptBuilder` global variable generation.

## v5.3.2 - 2022-01-16

### Fixed

- Fixed parsing of the 1.31.1 `triggerdata.txt`.

## v5.3.1 - 2022-01-16

### Added

- Added patch v1.32.10 to `GamePatch`.
- Added an extension method to read `TriggerData` from a `StringReader`.

### Fixed

- Fixed an exception thrown by `SingleOrDefault` in the `MpqArchiveBuilder` extension methods.

## v5.3.0 - 2022-01-16

### Breaking Changes

- Changed the nullability of `Map` and `Campaign.Info`, and refactored `TriggerData`.
- `TriggerFunction.Branch` is now a nullable `int`.
- Renamed a previously unknown `TriggerCategoryDefinition` property.

### Added

- Added `MapFiles` and `CampaignFiles` enums, so only selected files need to be parsed, plus `Map.TryOpen`/`Campaign.TryOpen` methods and support for reading a campaign from a folder.
- Added `MapFactory`, for creating a new `MapInfo`, `MapEnvironment`, and `MapPreviewIcons`.
- Added `KnownPlayerColor` enum, `FileExtension`-style constants for `war3map.j`/`war3map.lua`, and an extension method to localize the strings in `MapInfo`.
- Added `MapScriptBuilder.InitGlobals`, and support for decompiling custom variable types, the map initialization event, and `MapImportedFiles`.
- Most `War3Net.Build.Core` classes now override `ToString()`.
- Added `War3Net.IO.Slk` (v0.1.3) as a dependency, and the `UnitUI.slk` resource.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.2.2 to v5.3.0, and `War3Net.IO.Mpq` from v5.1.3 to v5.3.0.

### Fixed

- Fixed reading and writing `war3map.w3s` sounds in the reforged file format.
- `MapTriggers` no longer throws when `TriggerItemCount` is incorrect.
- Fixed `MapScriptBuilder` passing the wrong `SetGamePlacement` argument in the `config` function.
- Fixed a `NullReferenceException` by setting `AllyPriorityFlags` in the constructor, and added a `PlayerData(int)` constructor.

## v5.2.2 - 2021-02-16

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.2.1 to v5.2.2.

## v5.2.1 - 2021-02-16

### Added

- Added `MapScriptBuilder` methods to generate the C# API for generated global variables.
- Added `SetPlayers` extension method.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.0.0 to v5.2.1, and `War3Net.IO.Mpq` from v5.0.0 to v5.1.3.

## v5.2.0 - 2021-02-14

### Breaking Changes

- Removed the old `MapBuilder`, replaced by a new `MapBuilder` built on top of `MapScriptBuilder`; `LegacyMapBuilder` is re-implemented on top of it.

### Added

- Added `MapScriptBuilder` (initially `MapScriptFactory`), which generates a map's script from its data files, including variable generators and a `Build` method.
- Added extension methods to get and set campaign/map files, with overloads taking `Encoding` and `TriggerStrings` parameters.
- Added `War3Net.CodeAnalysis.Jass` (v5.0.0) as a dependency.

### Fixed

- Fixed `LegacyMapBuilder` using `TopDirectoryOnly` when adding asset files, so files in subfolders are now included.

## v5.1.0 - 2020-12-25

### Added

- Added `ImportedFiles` and `Script` to the `Campaign` and `Map` classes.
- Added `SaveWithPreArchiveData` extension method overloads taking an `MpqArchiveCreateOptions` parameter.

### Changed

- Updated `War3Net.IO.Mpq` from v1.2.2 to v5.0.0.

### Fixed

- Fixed inconsistent casing across several public members.

## v5.0.0 - 2020-12-14

### Breaking Changes

- All map and campaign data classes have been rewritten: parsing and serialization now live in binary reader/writer extension methods instead of on the classes themselves, and the classes are plain mutable data objects.
- Renamed the environment classes for consistency, and removed the obsolete file-based API.
- `MapEnvironment.Width`/`Height` are now one less than before, matching the world editor.
- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Added

- Added `Map` and `Campaign` classes that hold all of a map's/campaign's files, with constructors for building them from scratch.
- Added support for reading and writing `.imp` files.
- Added `EditorVersion` enum, and support for more `.doo` file format versions.
- Introduced `SubVersion` for `MapTriggers` and `MapCustomTextTriggers`.

### Changed

- Player ally priority flags now use a bitmask.
- Updated `War3Net.IO.Mpq` from v1.1.1 to v1.2.2.

## v1.7.2 - 2020-11-12

### Changed

- Updated `War3Net.IO.Mpq` from v1.2.1 to v1.2.2.

## v1.7.1 - 2020-11-12

### Changed

- Updated `War3Net.IO.Mpq` from v1.2.0 to v1.2.1.

## v1.7.0 - 2020-11-12

### Added

- Added `MpqArchiveBuilderExtensions`.

## v1.6.0 - 2020-11-11

### Added

- Support parsing and serializing `.w3c` (cameras) files.
- Support parsing and serializing `.wts` (trigger strings) files.
- Added the extension methods from the Map Adapter project.

### Changed

- Updated `War3Net.Common` from v0.3.0 to v0.3.1.

### Fixed

- Fixed parsing protected `war3map.w3r` files.

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
[v5.8.1]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.12
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
