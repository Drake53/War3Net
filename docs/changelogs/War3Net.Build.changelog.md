# War3Net.Build Changelog

All notable changes to the `War3Net.Build` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

_No unreleased changes._

## [v6.0.3] - 2026-07-04

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

### Changed

- Updated `War3Net.Build.Core` from v6.0.2 to v6.0.3.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Breaking Changes

- `MapScriptBuilder` methods have been renamed and now write to an `IndentedTextWriter` instead of returning a syntax class.
- `TriggerRenderer` and `TriggerRendererContext` now expect an `IndentedTextWriter` instead of `JassRenderer`/`TextWriter`.
- `MapScriptBuilder`'s C# api methods have been removed, you can now use the `GenerateGlobals` methods and manually transpile to C#.

## v5.8.2 - 2025-09-27

### Changed

- Updated `War3Net.Build.Core` from v5.8.1 to v5.8.2.

## [v5.8.1] - 2025-09-12

### Changed

- Updated `War3Net.Build.Core` from v5.8.0 to v5.8.1.

## [v5.8.0] - 2025-09-06

### Added

- Added support for Linux/WSL, including case-insensitive path handling (#69).
- Backported general improvements from the vJASS branch (#71).

### Changed

- Updated `War3Net.Build.Core` from v5.7.1 to v5.8.0, and `War3Net.CodeAnalysis.Transpilers` from v5.6.1 to v5.8.0.

## v5.7.1 - 2023-01-19

### Changed

- Updated `War3Net.Build.Core` from v5.7.0 to v5.7.1.

## v5.7.0 - 2023-01-08

### Breaking Changes

- Followed the `War3Net.Build.Core` v5.7.0 merge of the separate map and campaign classes that held identical data.

### Added

- Added `CampaignExtensions.GetInfoFile`, `GetImportedFilesFile`, `SetImportedFilesFile`, and `LocalizeInfo`, matching the methods already available on `MapExtensions`.

### Changed

- Updated `War3Net.Build.Core` from v5.6.1 to v5.7.0.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.6.0 to v5.6.1.

## v5.6.0.1 - 2022-12-28

### Fixed

- Fixed a `NullReferenceException` in `MapScriptBuilder`.

## v5.6.0 - 2022-12-20

### Breaking Changes

- Renamed the format version enum members for consistency, following `War3Net.Build.Core` v5.6.0.

### Changed

- Updated `War3Net.Build.Core` from v5.5.7 to v5.6.0, and `War3Net.CodeAnalysis.Transpilers` from v5.5.5 to v5.6.0.

### Fixed

- Fixed the `MapScriptBuilder` check for whether 24 players are supported.

## v5.5.7 - 2022-12-14

### Changed

- Updated `War3Net.Build.Core` from v5.5.6 to v5.5.7.

## v5.5.6 - 2022-11-30

### Changed

- Updated `War3Net.Build.Core` from v5.5.5 to v5.5.6.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Changed

- Updated `War3Net.Build.Core` from v5.4.2 to v5.4.5, and `War3Net.CodeAnalysis.Transpilers` from v5.4.1 to v5.4.5.
- Adapted to the `GamePatchVersionProvider` to `GameBuildsProvider` replacement in `War3Net.Build.Core` v5.4.5.

## v5.4.2 - 2022-04-24

### Changed

- Updated `War3Net.Build.Core` from v5.4.1 to v5.4.2.

### Fixed

- `UnitDataExtensions` no longer uses `ValueAsBool`/`ValueAsChar`, which produced incorrect values for some object data types.

## v5.4.1 - 2022-02-13

### Breaking Changes

- `MapBuilder` has been moved to `War3Net.Build.Core`.

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Transpilers` from v5.4.0 to v5.4.1.

## v5.4.0 - 2022-02-13

### Added

- Added `TriggerRenderer`, for rendering `MapTriggers` back to a script, including support for Lua triggers.
- `MapScriptBuilder` now detects units referenced by triggers, and generates destructable global variables.
- `MapScriptBuilder` now uses the newly identified sound properties, and generates enemy priority flags for `PlayerData`.

### Changed

- `RenderAction`/`RenderCondition` take a `TriggerFunctionParameter` instead of a `TriggerFunction`.
- `UnitDataExtensions.IsBuilding` now checks the unit object data.
- Updated `War3Net.Build.Core` from v5.3.0 to v5.4.0, and `War3Net.CodeAnalysis.Transpilers` from v5.2.8 to v5.4.0.

### Fixed

- Fixed `MapScriptBuilder` global and widget variable generation, and a trigger naming bug.

## v5.3.0 - 2022-01-16

### Added

- `MapScriptBuilder` now generates trigger functions, `InitGlobals`, and user-defined global variables, and handles the `InitCustomTeams` special case where the function is generated but never invoked.
- Added support for decompiling `MapImportedFiles`, and constants for `war3map.j` and `war3map.lua`.

### Changed

- Added null checks for `MapInfo` and `MapEnvironment` throughout.
- Updated `War3Net.Build.Core` from v5.2.2 to v5.3.0.

### Fixed

- Fixed `MapScriptBuilder` handling of sounds with the `Music` flag, unit creation functions, `CreateNeutralPassive`, unit method conditions, and the `SetGamePlacement` argument in the `config` function.
- Fixed `MapExtensions.SetFile` for script files.
- `CreateCameras` now checks the new file format.

## v5.0.4 - 2021-04-09

### Added

- `MapScriptBuilder` now generates the tech tree and upgrades.

## v5.0.3 - 2021-04-08

### Changed

- Updated `War3Net.CodeAnalysis.Transpilers` from v5.2.7 to v5.2.8.

## v5.0.2 - 2021-04-07

### Fixed

- Fixed the `war3mapUnits.doo` case being unreachable because the switch was case-sensitive.

## v5.0.1 - 2021-04-06

### Changed

- Updated `War3Net.CodeAnalysis.Transpilers` from v5.2.6 to v5.2.7.

## v5.0.0 - 2021-03-06

### Breaking Changes

- Removed the old `MapBuilder` and the obsolete script builder classes; map scripts are now generated through `MapScriptBuilder` and the new `MapBuilder`, with `LegacyMapBuilder` re-implemented on top of it.
- Removed the `War3Api` dependency, and replaced the `War3Net.CodeAnalysis.Jass` dependency with `War3Net.CodeAnalysis.Transpilers` (v5.2.6).
- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Added

- Added extension methods to get and set campaign/map files, and more constants.
- Added support for the new package decompile options, `ScriptCompilerOptions.LobbyMusic`, and setting `compiler.IsExportMetadata`.

### Changed

- Updated `War3Net.Build.Core` from v1.5.3 to v5.2.2.

### Fixed

- Fixed `LegacyMapBuilder` using `TopDirectoryOnly` when adding asset files, so files in subfolders are now included.
- Fixed being unable to set a map's `Info`/`Environment` through the `SetFile` extension method when they were null.

## v1.7.0 - 2020-11-12

### Changed

- Updated `War3Net.Build.Core` from v1.5.0 to v1.5.3.

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

## v1.3.10 - 2020-08-30

### Added

- Add event `OnArchiveBuilding` to `MapBuilder`.

## v1.3.9 - 2020-08-22

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

- Include 1.32.6 and 1.32.7 in `GamePatch` enum.

### Changed

- Created new project `War3Net.Build.Core`, and moved useful files there to reduce dependencies.

## v1.3.5 - 2020-07-11

### Added

- Can now parse and serialize `war3map.w3s` file format version 3. Meaning of the added data not yet known, nor stored in the `MapSounds` object.

### Changed

- Object data file parsers no longer validate the object modifications.
- Updated `War3Net.Common` and `War3Net.IO.Mpq` packages.

### Fixed

- Changed/added some exception messages.

## v1.3.4 - 2020-06-01

### Added

- Added `DecompilePackageLibs` to `ScriptCompilerOptions`.

### Changed

- Updated `War3Net.CodeAnalysis.Jass`, `CSharpLua`, and `War3Api` packages.

## v1.3.3 - 2020-05-09

### Added

- Include latest Reforged patches in `GamePatch` enum.

### Fixed

- Added default value and property for `ObjectData` format version, so its value is not stuck at 0, which is invalid.

## v1.3.2 - 2020-04-25

### Added

- Added setter indexer to `ObjectDataModification`.
- Added property `MapObjectData` to `ScriptCompilerOptions`.

## v1.3.1 - 2020-04-21

### Changed

- Update `CSharpLua` and MPQ packages.

### Fixed

- Can now run `MapBuilder`'s `Build` method multiple times, without needing to restart the application.

## v1.3.0 - 2020-04-12

### Added

- Support `.w3u`, `.w3t`, `.w3b`, `.w3d`, `.w3a`, `.w3h`, `.w3q`, `.w3o`, and `.w3f` files.
- Added `ObjectData` and `TargetPatch` properties to `ScriptCompilerOptions`. Not setting the `TargetPatch` will generate a new warning diagnostic.
- `FileProvider` can now search recursively in MPQ archives (useful for campaigns). Also added `FileExists` method.
- Added property `HasSkin` to unit and doodad data.
- Include latest Reforged patches in `GamePatch` enum.
- Added `SetGameVersion` method to `MapInfo`.

### Changed

- `CreateAllDestructables` can now generate the dead and withZ variants of `CreateDestructable`.

## v1.2.0 - 2020-03-08

### Added

- Support `war3map.mmp` files.
- Support water tinting color.

## v1.1.4 - 2020-03-04

### Added

- Added setters for `Sound` properties.
- Added get/set methods in `MapSounds` to handle sound collection.

### Changed

- Update `CSharpLua` to v1.5.10, and `War3Api` to v1.32.2.

## v1.1.3 - 2020-03-01

### Fixed

- Fix `MapSounds` reforged file format was not parsed correctly.

## v1.1.2 - 2020-02-10

### Fixed

- Fix `MapRegions` containing regions without sound generates invalid syntax.

## v1.1.1 - 2020-02-09

### Fixed

- Replace `Regex.Escape`, which escapes too many characters (e.g. `.`).
- Update `CSharpLua` to v1.5.9, which has some more bug fixes.

## v1.1.0 - 2020-02-08

### Breaking Changes

- `MapSounds` constructor now takes `MapSoundsFormatVersion` instead of `uint`.

### Added

- Added `FormatVersion` enum and property for all map files.
- Added `GamePatch` enum.

### Changed

- Default `FormatVersion` for `MapInfo` set back to v1.31 format.
- `MapInfo` property setters will now check if the property is available for the current `FormatVersion`.
- Map script generator will set unit/item/destructable skin if it's different from its type ID.

### Fixed

- `MapDoodads` is now parsed correctly, similar to `MapUnits`.

## v1.0.2

### Breaking Changes

- Subversion type for `.doo` headers changed from `uint` to `MapWidgetsSubVersion` enum.

### Added

- Added reforged sound channels (not tested).
- Added setters for most `MapUnits` properties, and added the `Skin` property.

## v1.0.1

### Added

- Can now parse and serialize `war3map.w3s` file format version 2. Meaning of the added data not yet known, nor stored in the `MapSounds` object.
- Added overload for `PlayerData.Create` method, making it easier to create a copy of an existing playerData object.

## v1.0.0 - 2020-01-28

### Breaking Changes

- Strings from `MapInfo.MapName`, `MapInfo.MapDescription`, and `Options.LobbyMusic` are now automatically escaped in the map script.
  - This change introduced a bug, that has been fixed in v1.1.1.

### Changed

- Updated `War3Api` and `MapInfo.Default` for reforged.

### Fixed

- Unit and doodad rotation data from `.doo` files is now correctly converted from radians to degrees.
- `MapUnits` and `MapDoodads` `IEnumerable` constructor now sets the `.doo` header to its default value.
- Sounds paths in `war3map.w3s` files are now correctly escaped.

## v0.2.1 - 2019-10-21

### Added

- Added `ScriptCompilerOptions(IEnumerable<string>)` constructor.

### Changed

- The CSharp.lua CoreSystem libraries are no longer imported into the compilation automatically; `ScriptCompilerOptions.Libraries` is used as-is.

## v0.2.0 - 2019-10-21

### Added

- Added `TerrainType` enum.

### Changed

- Updated the `War3Net.CSharpLua` dependency from v1.1.2 to the floating v1.2.* range.

## v0.1.2 - 2019-10-16

### Changed

- Updated the `War3Net.CSharpLua` package version.

## v0.1.1 - 2019-10-16

### Fixed

- Fixed the dependency on the CSharp.lua project not being declared correctly when packing.

## v0.1.0 - 2019-10-15

### Added

- Initial release, for building a Warcraft III map file from source:
  - `MapBuilder`, for assembling a map from its data files and assets, including loading assets from MPQ archives.
  - `ScriptCompiler` and `ScriptCompilerOptions`, for compiling a map script from JASS, Lua, or C#, with support for importing custom Lua libraries and compiling C# in debug mode.
  - `FunctionBuilder` classes, for generating the `main` and `config` functions.
  - Parsing and serialization for the `war3map.w3i` file, with methods to manipulate player and force settings.
  - `Tileset` enum, provider classes for light and sound environments, and `FileProvider` with locale support.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.3]: https://github.com/Drake53/War3Net/releases/tag/v6.0.3
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.1]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.12
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
