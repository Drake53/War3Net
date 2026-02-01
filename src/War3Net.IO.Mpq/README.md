# War3Net.IO.Mpq

## About

War3Net.IO.Mpq is a .NET library for reading and writing MPQ (MoPaQ) archives, a container format used by Blizzard games including Warcraft III. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Open and read files from existing MPQ archives
* Create new MPQ archives with customizable options
* Modify existing archives by adding or removing files
* Support for file compression (ZLib, PKLib, BZip2, LZMA)
* Support for file encryption with automatic seed calculation
* Handle multiple file locales within the same archive
* Access files in nested archives (archives within archives)
* Automatic listfile and attributes management

## How to Use

### Read files from an archive

```csharp
using War3Net.IO.Mpq;

// Open an existing archive
using var archive = MpqArchive.Open("path/to/archive.mpq", loadListFile: true);

// Check if a file exists
if (archive.FileExists("war3map.w3i"))
{
    // Open and read a file
    using var stream = archive.OpenFile("war3map.w3i");
    // Process the stream...
}
```

### Create a new archive

```csharp
using War3Net.IO.Mpq;

// Create files to add to the archive
var file1 = MpqFile.New(File.OpenRead("script.j"), "war3map.j");
var file2 = MpqFile.New(new MemoryStream(data), "war3map.w3i");

// Configure archive options
var options = new MpqArchiveCreateOptions
{
    BlockSize = 3,
    HashTableSize = 16,
    ListFileCreateMode = MpqFileCreateMode.Overwrite,
    AttributesCreateMode = MpqFileCreateMode.Overwrite,
};

// Create the archive
using var archive = MpqArchive.Create(
    "output.mpq",
    new[] { file1, file2 },
    options);
```

### Modify an existing archive

```csharp
using War3Net.IO.Mpq;

// Open the original archive
using var archive = MpqArchive.Open("map.w3x", loadListFile: true);

// Create a builder from the archive
var builder = new MpqArchiveBuilder(archive);

// Add a new file
var newFile = MpqFile.New(new MemoryStream(newData), "newfile.txt");
builder.AddFile(newFile);

// Remove an existing file
builder.RemoveFile("oldfile.txt");

// Save the modified archive
builder.SaveTo("modified.w3x");
```

### Set file compression and encryption

```csharp
using War3Net.IO.Mpq;

var file = MpqFile.New(stream, "encrypted.dat");

// Set compression type
file.CompressionType = MpqCompressionType.ZLib;

// Set target flags for compression and encryption
file.TargetFlags = MpqFileFlags.Exists
    | MpqFileFlags.CompressedMulti
    | MpqFileFlags.Encrypted;
```

## Main Types

The main types provided by this library are:

* `War3Net.IO.Mpq.MpqArchive` - Represents an MPQ archive for reading or creating
* `War3Net.IO.Mpq.MpqArchiveBuilder` - Builder for modifying existing archives
* `War3Net.IO.Mpq.MpqArchiveCreateOptions` - Configuration options for archive creation
* `War3Net.IO.Mpq.MpqFile` - Represents a file to be added to an archive
* `War3Net.IO.Mpq.MpqStream` - Stream for reading file contents from an archive
* `War3Net.IO.Mpq.MpqEntry` - Metadata entry for a file in the block table
* `War3Net.IO.Mpq.MpqHash` - Hash table entry for file lookup
* `War3Net.IO.Mpq.MpqFileFlags` - Flags for file compression, encryption, and existence
* `War3Net.IO.Mpq.MpqCompressionType` - Compression algorithms (ZLib, PKLib, BZip2, etc.)
* `War3Net.IO.Mpq.MpqLocale` - Locale identifiers for multi-language support

## Related Packages

* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files
* [War3Net.IO.Compression](https://www.nuget.org/packages/War3Net.IO.Compression) - Compression algorithms for MPQ files

## Feedback and contributing

War3Net.IO.Mpq is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.