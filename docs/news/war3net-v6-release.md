# War3Net v6.0.0 — Rebuilding the JASS Engine

*Released: January 25, 2026*

**War3Net v6.0.0** is a **stepping-stone release** that begins a major rework of the JASS code analysis engine. This is foundational work—not a finished product.

> **Note:** The JASS rework is ongoing. Expect additional breaking changes in future releases as the new architecture matures.

---

## What's New

### Lossless JASS Parsing

The core of v6: a redesigned syntax model in `War3Net.CodeAnalysis.Jass`. **Tokens and trivia** make it possible to parse and re-render JASS with byte-for-byte accuracy—preserving whitespace, comments, and exact source formatting.

- **`JassSyntaxToken`** — Tokens with leading/trailing trivia
- **`JassSyntaxTrivia`** — Whitespace, newlines, and comments
- **`JassSyntaxKind`** — Unified enum for literals, operators, and keywords

### Streamlined Script Building

`War3Net.Build`'s `MapScriptBuilder` gets a cleaner API:
- Methods write directly to `IndentedTextWriter` for better performance
- Removed Roslyn C# syntax export for global variables—use `GenerateGlobals` and transpile manually instead

### Other Changes

- **`W3MathF`** — New class for floating-point math
- **`EscapedStringProvider`** — Moved to `War3Net.CodeAnalysis.Jass`
- **Bugfix** — `MapInfo.EditorVersion` JSON deserialization

---

## Why This Release Matters

v6 lays the groundwork for one of War3Net's two major initiatives on the [roadmap](https://github.com/Drake53/War3Net#roadmap).

**JASS Language Suite** — The new token/trivia system is the first step toward fault-tolerant parsing, semantic analysis, and LSP support. Future goals include intelligent script adaptation across game versions and a VSCode extension with full IDE features for JASS.

---

## The Road to v6

This release has been years in the making. Work began in 2022 with an ambitious goal: expand the library with a vJASS parser built on top of the existing JASS parser. That effort quickly revealed fundamental limitations in the original JASS implementation—limitations that couldn't be papered over.

What followed was a lengthy process of backporting architectural changes from the vJASS work into the core JASS parser. The token/trivia system, the unified syntax kinds, the shift from interfaces to abstract classes—all of these emerged from lessons learned while tackling vJASS's more complex grammar. The original syntax classes were kept simple to mirror CSharpLua's Lua syntax classes. The rework, by contrast, draws heavy inspiration from Roslyn.

The final push to release came with assistance from AI coding agents, which helped accelerate the tedious boilerplate changes that come with any large-scale refactor: updating dozens of syntax node classes, ensuring consistent patterns across the codebase, and handling the mechanical work that would otherwise consume months of development time.

---

## Migration

v6 includes breaking changes:
- Interfaces → abstract classes (`IExpressionSyntax` → `JassExpressionSyntax`)
- Specific literals → generic `JassLiteralExpressionSyntax` with `JassSyntaxKind`
- Operator enums → token kinds (`BinaryOperatorType.Add` → `JassSyntaxKind.PlusToken`)

See the full [migration guide](https://github.com/Drake53/War3Net/blob/master/docs/guides/jass-migration-guide-v5-to-v6.md).

---

Questions or feedback? [Open an issue](https://github.com/Drake53/War3Net/issues).
