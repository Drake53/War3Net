# War3Net.IO.Slk

## About

War3Net.IO.Slk is a .NET library for reading and writing SLK (Symbolic Link/SYLK) files, a spreadsheet format used by Warcraft III to store game data such as unit properties, abilities, and UI configuration. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Parse SLK files into in-memory table structures
* Serialize tables back to SLK format
* Query columns by header name for easy data access
* Combine multiple tables with join operations
* Support for multiple data types (strings, integers, floats, booleans)
* Efficient row-wise iteration with LINQ support
* Automatic table size optimization with shrink operations

## How to Use

### Parse an SLK file

```csharp
using War3Net.IO.Slk;

// Parse an SLK file from a stream
using var stream = File.OpenRead("UnitUI.slk");
var table = new SylkParser().Parse(stream);

// Access table dimensions
Console.WriteLine($"Columns: {table.Columns}, Rows: {table.Rows}");
```

### Access cells by column name

```csharp
using War3Net.IO.Slk;

var table = new SylkParser().Parse(stream);

// Find column index by header name (first row contains headers)
int unitIdColumn = table["unitUIID"].Single();
int shadowColumn = table["buildingShadow"].Single();

// Iterate through data rows (skip header row at index 0)
foreach (var row in table.Skip(1))
{
    string unitId = (string)row[unitIdColumn];
    string shadow = row[shadowColumn] as string;

    if (!string.IsNullOrEmpty(shadow))
    {
        Console.WriteLine($"Unit {unitId} has shadow: {shadow}");
    }
}
```

### Create and serialize a table

```csharp
using War3Net.IO.Slk;

// Create a new table with dimensions
var table = new SylkTable(3, 4); // 3 columns, 4 rows

// Set header row
table[0, 0] = "ID";
table[1, 0] = "Name";
table[2, 0] = "Value";

// Set data rows
table[0, 1] = "A001";
table[1, 1] = "First Item";
table[2, 1] = 100;

// Serialize to a stream
using var output = File.Create("output.slk");
new SylkSerializer(table).SerializeTo(output, leaveOpen: false);
```

### Combine two tables

```csharp
using War3Net.IO.Slk;

var unitsTable = new SylkParser().Parse(unitsStream);
var abilitiesTable = new SylkParser().Parse(abilitiesStream);

// Join tables on matching column values
var combined = unitsTable.Combine(
    abilitiesTable,
    thisColumn: "unitId",
    otherColumn: "abilityUnit");
```

## Main Types

The main types provided by this library are:

* `War3Net.IO.Slk.SylkParser` - Parses SLK file streams into table objects
* `War3Net.IO.Slk.SylkTable` - In-memory representation of a tabular spreadsheet with cell access and query operations
* `War3Net.IO.Slk.SylkSerializer` - Serializes table objects back to SLK format

## Related Packages

* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives

## Feedback and contributing

War3Net.IO.Slk is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
