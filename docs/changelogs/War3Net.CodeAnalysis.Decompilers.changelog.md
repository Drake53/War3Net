# War3Net.CodeAnalysis.Decompilers Changelog

All notable changes to the `War3Net.CodeAnalysis.Decompilers` package, newest version first.

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

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v6.0.1 to v6.0.2.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Breaking Changes

- Updated `War3Net.CodeAnalysis.Jass` from v5.8.0 to v6.0.0. The `TryDecompile*` overloads that take a `JassFunctionDeclarationSyntax` now expect the redesigned syntax classes; those breaking changes have been documented in [the migration guide](../guides/jass-migration-guide-v5-to-v6.md).
- Updated `War3Net.Build.Core` from v5.8.2 to v6.0.0.

### Changed

- Comments and blank lines are now read from syntax trivia instead of being handled as statements, following the syntax tree redesign in `War3Net.CodeAnalysis.Jass` v6.0.0.

## v5.8.2 - 2025-09-27

### Changed

- Updated `War3Net.Build.Core` from v5.8.1 to v5.8.2.

## [v5.8.1] - 2025-09-12

### Changed

- Updated `War3Net.Build.Core` from v5.8.0 to v5.8.1.

## [v5.8.0] - 2025-09-06

### Breaking Changes

- `TryDecompileMapUnits(JassFunctionDeclarationSyntax, ...)` takes an additional `createAllItemsFunction` parameter, and the parameterless-function overload now also requires the script to contain a `CreateAllItems` function.

### Added

- Preplaced items are now decompiled from the `CreateAllItems` function into `war3mapUnits.doo`, supporting both `CreateItem` and `BlzCreateItemWithSkin` calls (#54).

### Changed

- Updated `War3Net.Build.Core` from v5.7.1 to v5.8.0, and `War3Net.CodeAnalysis.Jass` from v5.6.1 to v5.8.0.

### Fixed

- Decompiled units, items, and start locations are now assigned an incrementing `CreationNumber` instead of all sharing the same default value (#53).

## v5.7.1 - 2023-01-19

### Changed

- Updated `War3Net.Build.Core` from v5.7.0 to v5.7.1.

## v5.7.0 - 2023-01-08

### Breaking Changes

- `TryDecompileMapImportedFiles` now outputs an `ImportedFiles` instead of a `MapImportedFiles`, following the merge of the map and campaign classes in `War3Net.Build.Core` v5.7.0.

### Changed

- Updated `War3Net.Build.Core` from v5.6.1 to v5.7.0.

### Fixed

- The imported files decompiler now also ignores the map's skin object data files, so they are no longer listed as imported files.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.6.0 to v5.6.1.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.5.5 to v5.6.0.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.4.1 to v5.4.5.

## v5.4.2 - 2022-02-18

### Added

- Added dedicated decompilers for the individual JASS syntax kinds (unary, binary and parenthesized expressions, all literal kinds, function and variable references, and `set`/`call`/`if`/`loop`/`return` statements), widening the range of scripts that can be decompiled into GUI triggers.
- Support decompiling the `IfThenElse` action, in addition to the already supported `IfThenElseMultiple`.
- Support decompiling the `WaitForCondition` and `ReturnAction` actions.
- Hero attributes are now decompiled from `SetHeroStr`, `SetHeroAgi`, and `SetHeroInt` calls into `UnitData`.

### Changed

- `TriggerDataContext.TriggerParams` now contains an additional entry, keyed by `string.Empty`, holding the trigger params of all variable types.
- Trigger conditions functions are now decompiled through the same code path as the statement lists of other functions.

### Fixed

- Fixed `NullReferenceException` in `TryDecompileMapUnits` when the script does not contain a `CreateAllUnits`, `config`, or `InitCustomPlayerSlots` function; it now returns `false`.
- Fixed `KeyNotFoundException` when decompiling a call to a function that is not present in the trigger data.
- Fixed string literals not being decompiled to a string parameter when the trigger data contains no custom string types.
- Fixed variable and array reference expressions being decompiled incorrectly when multiple matching trigger params were found.

## v5.4.1 - 2022-02-13

### Added

- Added `TryDecompileMapUnits`, which decompiles preplaced units and start locations from the `CreateAllUnits`, `config`, and `InitCustomPlayerSlots` functions into `war3mapUnits.doo`.

### Changed

- Updated `War3Net.Build.Core` and `War3Net.CodeAnalysis.Jass` from v5.4.0 to v5.4.1.

### Fixed

- The sounds, cameras, and regions decompilers no longer fail when the function body contains comments or blank lines.

## v5.4.0 - 2022-02-13

### Added

- Initial release, with `JassScriptDecompiler` for regenerating war3map files from a map's JASS script:
  - `TryDecompileMapTriggers` regenerates `war3map.wtg`, including trigger categories, variables, events, conditions, and actions, `for` and `foreach` loops, if-then-else, and custom script code.
  - `TryDecompileMapSounds` regenerates `war3map.w3s` from the `InitSounds` function, including music variables.
  - `TryDecompileMapCameras` regenerates `war3map.w3c`.
  - `TryDecompileMapRegions` regenerates `war3map.w3r`, including weather effects and ambient sounds.
  - `TryDecompileMapImportedFiles` regenerates `war3map.imp` from an MPQ archive.
- Added `TriggerDataContext`, which exposes lookup dictionaries for the types, params, calls, conditions, and actions of a `TriggerData` object.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.3]: https://github.com/Drake53/War3Net/releases/tag/v6.0.3
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.1]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.12
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
