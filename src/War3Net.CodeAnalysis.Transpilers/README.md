## War3Net.CodeAnalysis.Transpilers

[![NuGet downloads](https://img.shields.io/nuget/dt/War3Net.CodeAnalysis.Transpilers.svg)](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers)
[![NuGet version](https://img.shields.io/nuget/v/War3Net.CodeAnalysis.Transpilers.svg)](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers)
[![NuGet prerelease](https://img.shields.io/nuget/vpre/War3Net.CodeAnalysis.Transpilers.svg)](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers/absoluteLatest)

## Description

War3Net.CodeAnalysis.Transpilers is a library that allows you to transpile JASS source code to C# or lua.

## Examples

The following example shows how to transpile a .j file to a .lua file:
```csharp
using CSharpLua;
using System.IO;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Transpilers;

var transpiler = new JassToLuaTranspiler();
transpiler.RegisterJassFile(JassParser.ParseFile(@"path\to\common.j"));
transpiler.RegisterJassFile(JassParser.ParseFile(@"path\to\Blizzard.j"));

var luaCompilationUnit = transpiler.Transpile(JassParser.ParseFile(@"path\to\war3map.j"));

using (var fileStream = File.Create(@"path\to\war3map.lua"))
{
    using (var writer = new StreamWriter(fileStream))
    {
        var luaRendererOptions = new LuaSyntaxGenerator.SettingInfo
        {
            Indent = 4,
        };
        
        var luaRenderer = new LuaRenderer(luaRendererOptions, writer);
        luaRenderer.RenderCompilationUnit(luaCompilationUnit);
    }
}
```
