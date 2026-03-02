# War3Net.Build.Core

## About

War3Net.Build.Core is a .NET library for parsing and serializing Warcraft III map and campaign files. It provides complete support for reading, modifying, and writing all war3map and war3campaign archive formats. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Parse and serialize all war3map file formats (environment, units, doodads, triggers, object data, etc.)
* Parse and serialize war3campaign files
* Support for binary, JSON, and text serialization formats
* `Map` and `Campaign` classes for working with complete archives
* `MapBuilder` for creating and modifying map archives
* Selective loading with `MapFiles` and `CampaignFiles` flags for optimal performance
* Support for both classic Warcraft III and Reforged format versions

## How to Use

### Open and read a map

```csharp
using War3Net.Build;

// Open a map file
var map = Map.Open("path/to/map.w3x");

// Access map metadata
string mapName = map.Info?.MapName;
int playerCount = map.Info?.Players.Count ?? 0;

// Access placed units
var units = map.Units?.Units;
foreach (var unit in units ?? [])
{
    Console.WriteLine($"Unit {unit.TypeId} at ({unit.Position.X}, {unit.Position.Y})");
}
```

### Load only specific map components

```csharp
using War3Net.Build;

// Load only the components you need for better performance
var map = Map.Open("path/to/map.w3x", MapFiles.Info | MapFiles.Units | MapFiles.Doodads);
```

### Modify and save a map

```csharp
using War3Net.Build;

// Open an existing map
var map = Map.Open("path/to/map.w3x");

// Modify map properties
if (map.Info is not null)
{
    map.Info.MapName = "My Modified Map";
    map.Info.MapDescription = "A map modified with War3Net";
}

// Save the modified map
var builder = new MapBuilder(map);
builder.Build("path/to/output.w3x");
```

### Read individual map files using extension methods

```csharp
using System.IO;
using War3Net.Build.Extensions;

// Read a specific map file directly
using var stream = File.OpenRead("war3map.w3e");
using var reader = new BinaryReader(stream);

var environment = reader.ReadMapEnvironment();
Console.WriteLine($"Map size: {environment.Width}x{environment.Height}");
```

### Work with campaign files

```csharp
using War3Net.Build;

// Open a campaign file
var campaign = Campaign.Open("path/to/campaign.w3n", CampaignFiles.All);

// Access campaign metadata
string campaignName = campaign.Info?.CampaignName;
var maps = campaign.Info?.MapButtons;
```

## Main Types

The main types provided by this library are:

* `War3Net.Build.Map` - Represents a complete Warcraft III map with all its data files
* `War3Net.Build.Campaign` - Represents a complete Warcraft III campaign
* `War3Net.Build.MapBuilder` - Fluent builder for creating and modifying map archives
* `War3Net.Build.MapFiles` - Flags enum for selective map component loading
* `War3Net.Build.CampaignFiles` - Flags enum for selective campaign component loading
* `War3Net.Build.Info.MapInfo` - Map metadata including name, author, players, and forces
* `War3Net.Build.Info.CampaignInfo` - Campaign metadata
* `War3Net.Build.Environment.MapEnvironment` - Terrain data including tiles, cliffs, and water
* `War3Net.Build.Widget.MapUnits` - Placed units in the map
* `War3Net.Build.Widget.MapDoodads` - Placed doodads (decorative objects) in the map
* `War3Net.Build.Script.MapTriggers` - GUI trigger definitions
* `War3Net.Build.Object.UnitObjectData` - Custom unit modifications
* `War3Net.Build.Object.AbilityObjectData` - Custom ability modifications

## Related Packages

* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) - Parse and render JASS source files
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives
* [War3Net.IO.Slk](https://www.nuget.org/packages/War3Net.IO.Slk) - Parse SLK (data table) files

## Feedback and contributing

War3Net.Build.Core is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.