# War3Net.Common

## About

War3Net.Common provides shared utilities for the [War3Net](https://github.com/Drake53/War3Net) modding library. It contains extension methods, encoding providers, and type conversion utilities commonly used when working with Warcraft III binary file formats.

## Key features

* Extension methods for `BinaryReader` and `BinaryWriter` to handle Warcraft III-specific data types
* Rawcode conversion between 4-character strings and 32-bit integers
* Type-safe enum conversion with validation for binary data parsing
* Stream utilities for efficient copying and reading operations
* Color format conversions (RGBA, BGRA)
* Pre-configured UTF-8 encoding providers without byte order marks

## How to Use

### Convert rawcodes

Warcraft III uses 4-character "rawcodes" to identify units, abilities, and other objects. These are stored as 32-bit integers in binary files.

```csharp
using War3Net.Common.Extensions;

// Convert a rawcode string to integer
int unitId = "hpea".FromRawcode(); // Human Peasant

// Convert an integer back to rawcode string
string rawcode = unitId.ToRawcode(); // "hpea"
```

### Read binary data with extension methods

```csharp
using System.IO;
using War3Net.Common.Extensions;

using var reader = new BinaryReader(stream);

// Read a null-terminated UTF-8 string
string name = reader.ReadChars();

// Read a fixed-length string
string fixedString = reader.ReadString(32);

// Read a 32-bit boolean (0 or 1)
bool enabled = reader.ReadBool();

// Read color values
Color rgbaColor = reader.ReadColorRgba();
Color bgraColor = reader.ReadColorBgra();

// Read typed enum values
MyEnum value = reader.ReadInt32<MyEnum>();
```

### Type-safe enum conversion

```csharp
using War3Net.Common;

// Convert raw binary values to enums with validation
var myEnum = EnumConvert<MyEnum>.FromInt32(value);

// Use raw conversion when validation is not needed
var rawEnum = EnumConvert<MyEnum>.FromInt32Raw(value);
```

### Use UTF-8 encoding without BOM

```csharp
using War3Net.Common.Providers;

// Lenient encoding (does not throw on invalid bytes)
Encoding utf8 = UTF8EncodingProvider.UTF8;

// Strict encoding (throws on invalid bytes)
Encoding strictUtf8 = UTF8EncodingProvider.StrictUTF8;
```

## Main Types

The main types provided by this library are:

* `War3Net.Common.EnumConvert<TEnum>` - Type-safe conversion from primitive types to enums with validation
* `War3Net.Common.Extensions.BinaryReaderExtensions` - Extension methods for reading Warcraft III data types
* `War3Net.Common.Extensions.BinaryWriterExtensions` - Extension methods for writing Warcraft III data types
* `War3Net.Common.Extensions.StreamExtensions` - Extension methods for stream operations
* `War3Net.Common.Extensions.Int32Extensions` - Rawcode and color conversion from integers
* `War3Net.Common.Extensions.StringExtensions` - Rawcode conversion from strings
* `War3Net.Common.Providers.UTF8EncodingProvider` - Pre-configured UTF-8 encodings

## Related Packages

* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives
* [War3Net.IO.Slk](https://www.nuget.org/packages/War3Net.IO.Slk) - Read and write SLK files

## Feedback and contributing

War3Net.Common is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.