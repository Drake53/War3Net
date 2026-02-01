# War3Net.CodeAnalysis.Transpilers

## About

War3Net.CodeAnalysis.Transpilers is a .NET library for transpiling JASS (the scripting language used by Warcraft III) to C# or Lua. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Transpile JASS source code to C#
* Transpile JASS source code to Lua
* Handle embedded Lua code in JASS scripts using polyglot transpilation
* Optional CSharpLua template attribute generation for C# output
* Configurable handling of comments and empty declarations

## How to Use

### Transpile JASS to C#

```csharp
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Transpilers;

// Parse JASS source code
string jassCode = File.ReadAllText("common.j");
var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(jassCode);

// Transpile to C#
var transpiler = new JassToCSharpTranspiler();
IEnumerable<MemberDeclarationSyntax> members = transpiler.Transpile(compilationUnit);
```

### Transpile JASS to Lua

```csharp
using CSharpLua;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Transpilers;

// Parse JASS source code
string jassCode = File.ReadAllText("war3map.j");
var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(jassCode);

// Configure and use the transpiler
var transpiler = new JassToLuaTranspiler
{
    IgnoreComments = false,
    IgnoreEmptyDeclarations = true,
    KeepFunctionsSeparated = true,
};

// Register type information from native/common.j files
transpiler.RegisterJassFile(commonJCompilationUnit);
transpiler.RegisterJassFile(blizzardJCompilationUnit);

// Transpile to Lua
var luaCompilationUnit = transpiler.Transpile(compilationUnit);

// Render to file
using var fileStream = File.Create("war3map.lua");
using var writer = new StreamWriter(fileStream);

var rendererSettings = new LuaSyntaxGenerator.SettingInfo { Indent = 4 };
var renderer = new LuaRenderer(rendererSettings, writer);
renderer.RenderCompilationUnit(luaCompilationUnit);
```

### Transpile polyglot JASS with embedded Lua

```csharp
using CSharpLua;
using War3Net.CodeAnalysis.Transpilers;

// Create the transpiler and renderer
var transpiler = new JassToLuaTranspiler();
var rendererSettings = new LuaSyntaxGenerator.SettingInfo();
using var writer = new StringWriter();

var polyglotTranspiler = new PolyglotJassToLuaTranspiler(
    transpiler,
    rendererSettings,
    writer);

// Transpile JASS containing //! beginusercode and //! endusercode blocks
string polyglotJass = File.ReadAllText("war3map.j");
polyglotTranspiler.Transpile(polyglotJass);

string luaOutput = writer.ToString();
```

## Main Types

The main types provided by this library are:

* `War3Net.CodeAnalysis.Transpilers.JassToCSharpTranspiler` - Transpiles JASS syntax trees to C# syntax trees using Roslyn
* `War3Net.CodeAnalysis.Transpilers.JassToLuaTranspiler` - Transpiles JASS syntax trees to Lua syntax trees
* `War3Net.CodeAnalysis.Transpilers.PolyglotJassToLuaTranspiler` - Handles JASS files with embedded Lua code blocks

## Related Packages

* [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) - Parse and render JASS source files
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps

## Feedback and contributing

War3Net.CodeAnalysis.Transpilers is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.