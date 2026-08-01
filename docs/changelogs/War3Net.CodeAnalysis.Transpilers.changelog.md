# War3Net.CodeAnalysis.Transpilers Changelog

All notable changes to the `War3Net.CodeAnalysis.Transpilers` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v6.0.1 to v6.0.2.
- Updated `War3Net.CSharpLua` from v2.0.2 to v2.0.3.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Breaking Changes

- Both transpilers have been reworked for the new `War3Net.CodeAnalysis.Jass` v6.0.0 syntax tree. Since there is a `Transpile` overload per JASS syntax class, the overload set changed along with the syntax classes described in [the migration guide](../guides/jass-migration-guide-v5-to-v6.md).
- Most `Transpile` methods on `JassToCSharpTranspiler` gained overloads that take a leading and/or trailing `JassSyntaxTriviaList`, so that a parent node can pass down the trivia of the tokens it discards.

### Added

- The transpiled C# now preserves the whitespace and comments of the JASS source, which are exposed as trivia by the new syntax tree.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.8.0 to v6.0.0.
- Updated `War3Net.CSharpLua` from v2.0.1 to v2.0.2.

## [v5.8.0] - 2025-09-06

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.6.1 to v5.8.0.
- Updated `War3Net.CSharpLua` from v1.7.20 to v2.0.1.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.6.0 to v5.6.1.
- Updated `War3Net.CSharpLua` from v1.7.19 to v1.7.20.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.5.5 to v5.6.0.
- Updated `War3Net.CSharpLua` from v1.7.18 to v1.7.19.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.5.0 to v5.5.2.
- Updated `War3Net.CSharpLua` from v1.7.17 to v1.7.18.

## v5.5.0 - 2022-08-20

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.4.5 to v5.5.0.
- Updated `War3Net.CSharpLua` from v1.7.16 to v1.7.17.

## v5.4.5 - 2022-05-27

### Breaking Changes

- `Transpile(IDeclarationSyntax)` now takes an `ITopLevelDeclarationSyntax`, on both `JassToCSharpTranspiler` and `JassToLuaTranspiler`.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.4.1 to v5.4.5.
- Updated `War3Net.CSharpLua` from v1.7.15 to v1.7.16.

## v5.4.1 - 2022-02-13

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.4.0 to v5.4.1.

## v5.4.0 - 2022-02-13

### Breaking Changes

- `Transpile(JassCommentDeclarationSyntax)` and `Transpile(JassCommentStatementSyntax)` have been merged into `Transpile(JassCommentSyntax)`, and `Transpile(JassEmptyDeclarationSyntax)` and `Transpile(JassEmptyStatementSyntax)` into `Transpile(JassEmptySyntax)`.

### Added

- Added `PolyglotJassToLuaTranspiler`, which transpiles JASS scripts that contain embedded Lua code delimited by `//! beginusercode` and `//! endusercode`, and renders the result to a `TextWriter`.
- Added `JassToLuaTranspiler.Transpile(JassFunctionDeclaratorSyntax)`.
- The package now contains SourceLink metadata.

### Changed

- `JassToLuaTranspiler.Transpile(JassGlobalDeclarationListSyntax)` now throws a `NotSupportedException` for unrecognized global declarations, instead of a `SwitchExpressionException`.
- Updated `War3Net.CodeAnalysis.Jass` from v5.2.2 to v5.4.0.
- Updated `War3Net.CSharpLua` from v1.7.13 to v1.7.15.

## v5.2.8 - 2021-04-08

### Changed

- Updated `War3Net.CSharpLua` from v1.7.12 to v1.7.13.

## v5.2.7 - 2021-04-06

### Changed

- Updated `War3Net.CSharpLua` from v1.7.8 to v1.7.12.

## v5.2.6 - 2021-03-06

### Changed

- Updated `War3Net.CSharpLua` from v1.7.7 to v1.7.8.

## v5.2.5 - 2021-02-21

### Changed

- Updated `War3Net.CSharpLua` from v1.7.5 to v1.7.7.

## v5.2.3 - 2021-02-20

### Changed

- Updated `War3Net.CSharpLua` from v1.7.4 to v1.7.5.

## v5.2.2 - 2021-02-16

### Added

- Added `JassToCSharpTranspiler.ApplyCSharpLuaTemplateAttribute`, which emits a `@CSharpLua.Template` documentation comment on transpiled global declarations, and `JassToCSharpTranspiler.JassToLuaTranspiler`, which is the transpiler used to generate the template's Lua identifier.
- Added `SyntaxNodeExtensions.WithCSharpLuaTemplateAttribute<TSyntax>` in the new `War3Net.CodeAnalysis.Transpilers.Extensions` namespace.

## v5.2.1 - 2021-02-14

### Breaking Changes

- `JassToCSharpTranspiler` has been reimplemented against the `War3Net.CodeAnalysis.Jass` v5.2.x syntax tree: it is now a non-static class that must be instantiated, and its `Transpile` extension methods have been replaced by instance methods taking the new `Jass*Syntax` classes.
- Removed `JassTranspiler`, `JassTranspilerHelper`, `CompilationHelper`, `TokenTranspileFlags`, and `TranspileToEnumHandler`. As a result, the JASS-to-C# transpiler no longer wraps its output in a namespace and class declaration, no longer converts `common.j` handle types into C# enums, and no longer offers helpers to compile and emit the transpiled code (`CompileCSharpFromJass`, `PrepareCompilation`, `GetReferencesAndUsingDirectives`, `SerializeTo`).
- `JassToLuaTranspiler.Transpile(IVariableDeclarator, bool)` now takes an `IVariableDeclaratorSyntax`, following the rename in `War3Net.CodeAnalysis.Jass`.

### Added

- Added `IgnoreComments`, `IgnoreEmptyDeclarations`, `IgnoreEmptyStatements`, and `KeepFunctionsSeparated` options to `JassToLuaTranspiler`.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.2.0 to v5.2.1.
- Updated `War3Net.CSharpLua` from v1.7.1 to v1.7.4.

## v5.2.0 - 2021-01-24

### Breaking Changes

- `JassToLuaTranspiler` has been reimplemented against the `War3Net.CodeAnalysis.Jass` v5.2.0 syntax tree: `RegisterJassFile` now takes a `JassCompilationUnitSyntax` instead of a `FileSyntax`, and its `Transpile` methods take the new `Jass*Syntax` classes.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.1.0 to v5.2.0.

## v5.1.1 - 2020-12-24

### Fixed

- Native function declarations are now registered while transpiling to Lua, and not only by `RegisterJassFile`, so calls to a native declared in a file that was transpiled without being registered first no longer throw a `KeyNotFoundException`.

## v5.1.0 - 2020-12-22

### Breaking Changes

- `JassToLuaTranspiler` is now a non-static class that must be instantiated, and its `TranspileToLua` extension methods have been replaced by instance `Transpile` methods.
- Removed the obsolete `StringBuilder`-based `Transpile` extension methods, along with the `TranspileStringConcatenationHandler` class they relied on.
- The `Transpile` methods of `JassToCSharpTranspiler` now check the syntax node itself instead of the `Empty…Node` counterparts that were removed in `War3Net.CodeAnalysis.Jass` v5.1.0.

### Added

- Added `JassToLuaTranspiler.RegisterJassFile(FileSyntax)`, `RegisterFunctionReturnType(MethodInfo)`, and `RegisterGlobalVariableType(FieldInfo)`, which supply the type information needed to transpile expressions to Lua.
- Added a package readme.

### Changed

- Updated `War3Net.CodeAnalysis.Jass` from v5.0.0 to v5.1.0.

### Fixed

- The line delimiter at the start of a file is no longer dropped when transpiling to Lua.

## v5.0.0 - 2020-12-15

### Added

- Initial release. The transpiler code was moved out of the `War3Net.CodeAnalysis.Jass` package into this new package.
- `JassTranspiler` transpiles a parsed JASS `FileSyntax` into a C# `CompilationUnitSyntax`, wrapped in the given namespace and class, optionally emitting only the API as native function declarations.
- `JassToCSharpTranspiler` provides `Transpile` extension methods that convert individual JASS syntax nodes into Roslyn syntax nodes, with `TranspileToEnumHandler` and `CommonEnumTypesProvider` converting `common.j` handle types into C# enums.
- `JassToLuaTranspiler` provides `TranspileToLua` extension methods that convert JASS syntax nodes into `CSharpLua.LuaAst` nodes, with `TranspileStringConcatenationHandler` handling JASS' `+` operator on strings.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
