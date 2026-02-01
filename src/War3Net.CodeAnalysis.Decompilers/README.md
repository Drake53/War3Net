# War3Net.CodeAnalysis.Decompilers

## About

War3Net.CodeAnalysis.Decompilers is a library for regenerating Warcraft III map data files from JASS scripts. It parses the JASS script from a map and extracts structured map data that can be used to reconstruct or analyze map components. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Decompile GUI triggers and variables from JASS initialization functions
* Extract sound definitions with full audio properties (looping, 3D sound, fade rates, volume, pitch)
* Reconstruct camera setups with field values (zoom, rotation, angle of attack, clipping planes)
* Parse map regions with weather effects and ambient sounds
* Extract preplaced units, items, and start locations with all properties
* Generate imported file lists from MPQ archives

## How to Use

### Decompile map sounds from a map

```csharp
using War3Net.Build;
using War3Net.Build.Audio;
using War3Net.CodeAnalysis.Decompilers;

// Load a map with its script
var map = Map.Open("path/to/map.w3x");

// Create the decompiler
var decompiler = new JassScriptDecompiler(map);

// Decompile sounds
if (decompiler.TryDecompileMapSounds(MapSoundsFormatVersion.V3, out var sounds))
{
    foreach (var sound in sounds.Sounds)
    {
        Console.WriteLine($"Sound: {sound.Name}, File: {sound.FilePath}");
    }
}
```

### Decompile map triggers

```csharp
using War3Net.Build;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Decompilers;

var map = Map.Open("path/to/map.w3x");
var decompiler = new JassScriptDecompiler(map);

if (decompiler.TryDecompileMapTriggers(
    MapTriggersFormatVersion.V7,
    MapTriggersSubVersion.V4,
    out var triggers))
{
    foreach (var variable in triggers.Variables)
    {
        Console.WriteLine($"Variable: {variable.Name} ({variable.Type})");
    }
}
```

### Decompile map regions and cameras

```csharp
using War3Net.Build;
using War3Net.Build.Environment;
using War3Net.CodeAnalysis.Decompilers;

var map = Map.Open("path/to/map.w3x");
var decompiler = new JassScriptDecompiler(map);

// Decompile regions
if (decompiler.TryDecompileMapRegions(MapRegionsFormatVersion.V5, out var regions))
{
    foreach (var region in regions.Regions)
    {
        Console.WriteLine($"Region: {region.Name} at ({region.CenterX}, {region.CenterY})");
    }
}

// Decompile cameras
if (decompiler.TryDecompileMapCameras(MapCamerasFormatVersion.V0, useNewFormat: false, out var cameras))
{
    foreach (var camera in cameras.Cameras)
    {
        Console.WriteLine($"Camera: {camera.Name}, Distance: {camera.TargetDistance}");
    }
}
```

### Decompile preplaced units and items

```csharp
using War3Net.Build;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Decompilers;

var map = Map.Open("path/to/map.w3x");
var decompiler = new JassScriptDecompiler(map);

if (decompiler.TryDecompileMapUnits(
    MapWidgetsFormatVersion.V8,
    MapWidgetsSubVersion.V11,
    useNewFormat: false,
    out var units))
{
    foreach (var unit in units.Units)
    {
        Console.WriteLine($"Unit at ({unit.Position.X}, {unit.Position.Y})");
    }
}
```

## Main Types

The main types provided by this library are:

* `War3Net.CodeAnalysis.Decompilers.JassScriptDecompiler` - Primary class for decompiling JASS scripts back into map data structures

## Related Packages

* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts from Map objects
* [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) - Parse and render JASS source files
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives

## Feedback and contributing

War3Net.CodeAnalysis.Decompilers is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
