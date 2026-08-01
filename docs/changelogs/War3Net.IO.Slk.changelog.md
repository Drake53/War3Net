# War3Net.IO.Slk Changelog

All notable changes to the `War3Net.IO.Slk` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

### Changed

- Updated `War3Net.Common` from v6.0.1 to v6.0.2.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

_No functional changes; version bumped to align with the rest of War3Net's v6.0.0 release._

## [v5.8.1] - 2025-09-12

### Changed

- Table dimensions on the `B` record are now parsed with invariant culture.

### Fixed

- The `X` field on `C` records is now optional; when omitted, the cell's column falls back to the previously parsed column, so .slk files using this shorthand no longer fail to parse.
- A `C` record with a missing `X`, `Y`, or `K` field now throws a descriptive `InvalidDataException`/`NotSupportedException` instead of a `NullReferenceException`.

## [v5.8.0] - 2025-09-06

### Fixed

- Fixed `ArgumentOutOfRangeException` when a cell value consists of a single `"` character.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.Common` from v5.6.0 to v5.6.1.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.Common` from v5.5.5 to v5.6.0.

## v5.5.6 - 2022-11-30

### Fixed

- `SylkParser.SetCellContent` now parses cell coordinates and numeric values with `CultureInfo.InvariantCulture`, so files are no longer parsed incorrectly (or rejected) under cultures that use `,` as decimal separator.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.Common` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.Common` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- `SylkSerializer` now writes using `UTF8EncodingProvider.StrictUTF8` instead of constructing its own `UTF8Encoding`, adding a dependency on `War3Net.Common` (v5.5.2).

## v5.5.0 - 2022-08-20

_No functional changes; version bumped to align with the rest of War3Net's v5.5.0 release._

## v5.4.0 - 2022-02-13

_No functional changes; version bumped to align with the rest of War3Net's v5.4.0 release._

## v0.1.3 - 2021-11-24

### Changed

- The `NotSupportedException` thrown for unparsable values now includes the offending value in its message.

### Fixed

- `SylkParser` now accepts the `#REF!` error value and parses it as `0` (like `#VALUE!`), fixing a `NotSupportedException` when parsing files such as `unitweapons.slk` (fixes #21).

## v0.1.2 - 2021-10-31

### Breaking Changes

- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Added

- Added `SylkTable.Combine(SylkTable, int, int)` overload, for joining two tables on 0-indexed column indices instead of column names.

### Changed

- Documented `Rows`, `Columns`, and the cell indexer as being 0-indexed.

### Fixed

- Fixed `SylkTable.Shrink` clipping off one row and one column too many, which dropped the last value of a table (fixes #16).
- Fixed `SylkTable.Combine` producing incorrect results: it now sizes the resulting table from `Width`/`Height` instead of `Columns`/`Rows`, maps the other table's rows onto the joined rows correctly, and writes `newColumn` to the resulting table instead of mutating the source table.
- `SylkTable.Combine` now throws `ArgumentNullException` when `other` is `null`.
- Setting a cell to `null` now recalculates `Rows` and `Columns` instead of leaving them stale.

## v0.1.1 - 2020-10-27

### Breaking Changes

- Updated target framework from .NET Standard 2.1 to .NET Core 3.1.

## v0.1.0 - 2020-09-14

### Added

- Initial release, with `SylkParser`, `SylkTable`, and `SylkSerializer` for reading, manipulating, and writing SLK (SYLK) data files.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.1]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.12
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
