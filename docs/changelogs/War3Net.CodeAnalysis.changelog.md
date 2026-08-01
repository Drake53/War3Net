# War3Net.CodeAnalysis Changelog

All notable changes to the `War3Net.CodeAnalysis` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Added

- Added `IndentedTextWriter`, a `TextWriter` that writes indentation at the start of each line, with `Indent`/`Unindent` methods, a settable `IndentLevel`, a configurable indent string, and a `New(IndentedTextWriter)` method to create a new writer that copies the indent and newline strings of an existing one.
- Added `SeparatedSyntaxList.Create(TItem)` method, for creating a list with a single item and no separators.
- Added `SeparatedSyntaxList.CreateBuilder(TItem, int)` overload, to set the builder's initial capacity. It throws `ArgumentOutOfRangeException` when the capacity is less than 1.
- Added `SeparatedSyntaxList.IsEmpty` property.

### Changed

- `SeparatedSyntaxList.Create(ImmutableArray<TItem>, ImmutableArray<TSeparator>)` now returns the cached `Empty` instance instead of allocating a new list when `items` is empty.

### Fixed

- The `SeparatedList` parser no longer succeeds with an empty result when the item parser fails after consuming input; it now fails instead.
- The `IfThenElse`, `SeparatedList`, and `UntilWithLeading` parsers now dispose their pooled expected-token lists before throwing `InvalidOperationException` for a parser that consumed no input, so those pooled buffers are no longer leaked.

## [v5.8.0] - 2025-09-06

_No functional changes; version bumped to align with the rest of War3Net's v5.8.0 release._

## v5.6.1 - 2023-01-07

_No functional changes; version bumped to align with the rest of War3Net's v5.6.1 release._

## v5.6.0 - 2022-12-20

_No functional changes; version bumped to align with the rest of War3Net's v5.6.0 release._

## v5.5.5 - 2022-11-13

_No functional changes; version bumped to align with the rest of War3Net's v5.5.5 release._

## v5.5.3 - 2022-10-29

_No functional changes; version bumped to align with the rest of War3Net's v5.5.3 release._

## v5.5.2 - 2022-10-25

_No functional changes; version bumped to align with the rest of War3Net's v5.5.2 release._

## v5.5.0 - 2022-08-20

### Added

- Initial release, with the `ParserExtensions` methods `IfThenElse`, `SeparatedList`, and `UntilWithLeading` for [Pidgin](https://github.com/benjamin-hodgson/Pidgin) parsers, and the `SeparatedSyntaxList<TItem, TSeparator>` type (with its `Builder`) to represent the result of a `SeparatedList` parser.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
