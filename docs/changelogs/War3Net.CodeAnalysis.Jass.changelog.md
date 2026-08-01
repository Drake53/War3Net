# War3Net.CodeAnalysis.Jass Changelog

All notable changes to the `War3Net.CodeAnalysis.Jass` package, newest version first.

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

### Breaking Changes

- Breaking changes have been documented in [the migration guide](../guides/jass-migration-guide-v5-to-v6.md).

## [v5.8.0] - 2025-09-06

### Added

- `ExpressionSyntaxExtensions.TryGetIntegerExpressionValue` now also handles `JassHexadecimalLiteralExpressionSyntax`, so hexadecimal literals are recognized when decompiling a script into native editor files (#52).

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.6.1 to v5.8.0.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.6.0 to v5.6.1.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.5.5 to v5.6.0.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- Updated `War3Net.CodeAnalysis` and `War3Net.Common` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Shared syntax-tree types moved to the new `War3Net.CodeAnalysis` package, which is now a dependency.
- Updated `War3Net.Common` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Breaking Changes

- Renamed `IDeclarationSyntax` to `ITopLevelDeclarationSyntax`.

### Changed

- Optimized the whitespace parser, and stopped parsing trailing whitespace automatically after a keyword.
- `StatementParser` now uses `Parser.Rec()` instead of `ExpressionParser.Build()`.
- Updated `War3Net.Common` from v5.4.0 to v5.4.5.

## v5.4.1 - 2022-02-13

### Added

- Added syntax support required by the `war3mapUnits.doo` decompiler.

## v5.4.0 - 2022-02-13

### Breaking Changes

- Merged the separate statement and declaration classes for empty and comment syntax into a single class each.
- Replaced `ICustomScriptAction` with `IStatementLine`, `IGlobalLine`, and `IDeclarationLine`.

### Added

- Added `JassSyntaxFactory.ConditionFunctionDeclarator`, for functions that return a `boolean`.
- `TriggerRenderer` now supports Lua triggers.

### Changed

- `OperatorType` conversions now throw when the operator is invalid.
- Updated `War3Net.Common` from v5.0.2 to v5.4.0.

### Fixed

- Fixed `JassCommentStatementSyntax.Equals(IStatementSyntax)`.

## v5.3.1 - 2022-01-19

### Added

- Added `IInvocationSyntax`.
- Added `JassReturnStatementSyntax.Empty`.
- Moved `IExpressionSyntaxExtensions` here from the decompilers project.

### Fixed

- Fixed several syntax classes having an `internal` constructor, making them impossible to construct from outside the assembly.
- Comment syntax equality now compares the comment text instead of only the node type.

## v5.3.0 - 2022-01-16

### Added

- Added `JassSyntaxFacts` class.
- Added `JassRenamer`, for renaming declarations in a parsed script.
- Added `JassSyntaxFactory.ParseFunctionDeclaration`, `TryParse` methods, binary and unary operator parsers, and factory methods for exit statements and parenthesized expressions.
- All JASS syntax classes now override `ToString()`.
- `JassRenderer` now supports custom script actions.

### Fixed

- `FourCCLiteralExpressionParser` no longer throws on malformed input.
- Fixed `JassSyntaxFactory` literal expression inconsistencies compared to `ParseExpression`.

## v5.2.2 - 2021-02-16

### Added

- Added an option to apply the `CSharpLua.Template` attribute to transpiled global declarations.

### Changed

- Updated `War3Net.Common` from v5.0.0 to v5.0.2.

## v5.2.1 - 2021-02-14

### Breaking Changes

- Removed the JASS syntax classes that were marked obsolete in v5.2.0.

### Added

- Added a new `JassRenderer` implementation for the new syntax tree, and more `JassSyntaxFactory` methods.

## v5.2.0 - 2021-01-24

### Breaking Changes

- Replaced the JASS syntax tree, parser, and `JassSyntaxFactory` with a new implementation; the previous classes are marked obsolete.

### Added

- Added new JASS syntax classes and an updated set of keywords and symbols.
- Added `JassSyntaxFactory` parser methods and syntax factory methods for variables.

## v5.1.0 - 2020-12-22

### Added

- Added nullable reference type annotations to the syntax classes.
- Added `ToJassTypeKeyword` extension method for `Type`.

### Changed

- Removed the `EmptyNode` properties from the syntax classes.
- Updated `War3Net.Common` from v0.3.2 to v5.0.0.

### Fixed

- Fixed parsing failing when the file does not end with an empty line.
- Fixed several `JassRenderer` bugs.

## v5.0.0 - 2020-12-14

### Breaking Changes

- The transpilers have been moved to the new `War3Net.CodeAnalysis.Transpilers` package.
- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Added

- Added a dependency on `War3Net.Common` (v0.3.2).
- Added more `JassSyntaxFactory` methods.

## v2.1.0 - 2020-11-12

_No functional changes; version bumped to align with the rest of War3Net's 2020-11-12 release._

## v2.0.1 - 2020-11-11

### Fixed

- Several `JassToLuaTranspiler` bugfixes.

## v2.0.0 - 2020-10-27

### Breaking Changes

- Updated target framework from .NET Standard 2.1 to .NET Core 3.1.

### Added

- Added `JassToLuaTranspiler`, including handling of string concatenation.
- Added `JassParser` parse method overloads.

### Changed

- Changed several internal methods to public.

## v1.2.2 - 2020-09-14

### Breaking Changes

- Updated target framework from .NET Standard 2.0 to .NET Standard 2.1.

## v1.2.1 - 2020-06-01

### Added

- The JASS API transpiler now generates the `@CSharpLua.Ignore` attribute for classes.

## v1.2.0 - 2020-01-10

### Removed

- Removed the dependency on the `War3Net.CodeAnalysis.Common` package; the attributes it provided are no longer applied by the transpiler.

## v1.1.0 - 2020-01-03

### Changed

- JASS fields and functions are now transpiled using `@CSharpLua.Template` instead of `NativeLuaMemberAttribute`.

### Fixed

- Fixed the renamed `__inherits__` keyword.
- Fixed exceptions when a function has zero arguments.

## v1.0.3 - 2019-11-20

### Added

- Added more `JassSyntaxFactory` methods.

## v1.0.2 - 2019-10-08

_No functional changes; package metadata updated._

## v1.0.1 - 2019-10-06

### Breaking Changes

- The `JassApi` files (`common.j` and `Blizzard.j`) and the transpiler attributes have been moved out of this package; the attributes now live in `War3Net.CodeAnalysis.Common`.

### Added

- Functions are now automatically transpiled as natives when `NativeLuaMemberAttribute` is applied.

## v1.0.0 - 2019-09-30

### Added

- Initial release, with a tokenizer, parser, syntax tree, and renderer for JASS:
  - `JassParser` and `JassSyntaxFactory` for parsing JASS source into a syntax tree.
  - `JassRenderer` and `JassRendererOptions` for rendering a syntax tree back to JASS source.
  - `JassTranspiler` and `JassTranspilerHelper` for transpiling JASS to C#, including support for the attributes used by the subsequent C#-to-Lua step.
  - A source code obfuscator for JASS.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
