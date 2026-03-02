# War3Net.Build

## About

War3Net.Build is a library for generating complete Warcraft III map scripts (JASS) from map data and compiling C# source code to Lua for use in Warcraft III maps. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Generate complete JASS map scripts from `Map` objects
* Compile C# code to war3map.lua using CSharpLua
* Render GUI triggers to JASS code
* Customizable script generation with virtual methods for extensibility
* Build MPQ archives with all map assets

## How to Use

### Generate a JASS map script

```csharp
using War3Net.Build;

// Load or create your map
var map = Map.Open("path/to/map.w3x");

// Create the script builder and configure it
var scriptBuilder = new MapScriptBuilder();
scriptBuilder.SetDefaultOptionsForMap(map);

// Generate the JASS script
string jassScript = scriptBuilder.Build(map);
```

### Generate a map script for CSharpLua (Lua output)

```csharp
using War3Net.Build;

var map = Map.Open("path/to/map.w3x");

var scriptBuilder = new MapScriptBuilder();
scriptBuilder.SetDefaultOptionsForCSharpLua(lobbyMusic: "Sound\\Music\\mp3Music\\War3IntroX.mp3");

string jassScript = scriptBuilder.Build(map);
```

### Render a trigger to JASS

```csharp
using System.IO;
using War3Net.Build;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;

var writer = new IndentedTextWriter(new StringWriter());
var triggerData = TriggerData.Default;
var variables = map.Triggers.Variables;

var renderer = new TriggerRenderer(writer, triggerData, variables);
renderer.RenderTrigger(triggerDefinition);
```

## Main Types

The main types provided by this library are:

* `War3Net.Build.MapScriptBuilder` - Generates complete JASS map scripts from Map objects with customizable output
* `War3Net.Build.TriggerRenderer` - Renders individual trigger definitions into JASS code
* `War3Net.Build.TriggerRendererContext` - Context for trigger rendering operations
* `War3Net.Build.TrigFunctionIdentifierBuilder` - Helper for building unique trigger function identifiers
* `War3Net.Build.BuildResult` - Result of a build operation with success status and diagnostics
* `War3Net.Build.CompileResult` - Result of a compilation operation

## Related Packages

* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files
* [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) - Parse and render JASS source files
* [War3Net.CodeAnalysis.Transpilers](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers) - Transpile JASS to C# or Lua
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives

## Feedback and contributing

War3Net.Build is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
