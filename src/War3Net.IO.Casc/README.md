# War3Net.IO.Casc

A .NET library for reading Blizzard's CASC (Content Addressable Storage Container) archives, used in modern Blizzard games like World of Warcraft, Diablo III, Overwatch, and StarCraft II.

## Features

- ✅ **Complete CASC Support** - Read files from local CASC storages
- ✅ **BLTE Decompression** - Full support for BLTE compression including multi-chunk files
- ✅ **Encryption Support** - Salsa20 decryption for encrypted content
- ✅ **Multiple Access Methods** - Open files by name, content key, encoded key, or file data ID
- ✅ **Index File Support** - Parse both v1 and v2 index formats
- ✅ **Encoding Manifest** - Full encoding file support for key resolution
- ✅ **Root Handlers** - Extensible root file handling for different games
- ✅ **Progress Reporting** - Built-in progress reporting with cancellation support
- ✅ **War3Net Integration** - Seamless integration with other War3Net libraries

## Installation

```xml
<PackageReference Include="War3Net.IO.Casc" Version="1.0.0" />
```

## Quick Start

### Basic Usage

```csharp
using War3Net.IO.Casc;

// Open a CASC archive
using var archive = new CascArchive(@"C:\Games\World of Warcraft\Data");

// Check if a file exists
if (archive.FileExists("Interface\\Icons\\inv_sword_01.blp"))
{
    // Open and read the file
    using var stream = archive.OpenFile("Interface\\Icons\\inv_sword_01.blp");
    var data = new byte[stream.Length];
    stream.Read(data, 0, data.Length);
    
    // Process the file data...
}
```

### Advanced Storage Options

```csharp
using War3Net.IO.Casc.Storage;
using War3Net.IO.Casc.Enums;

// Open with specific locale
var storage = CascStorage.OpenStorage(
    @"C:\Games\World of Warcraft\Data",
    CascLocaleFlags.EnUS | CascLocaleFlags.EnGB
);

// Access storage features
Console.WriteLine($"Features: {storage.Features}");
Console.WriteLine($"Product: {storage.Product?.CodeName}");
Console.WriteLine($"Build: {storage.Product?.BuildNumber}");
```

## Detailed Usage Examples

### 1. Opening Files by Different Methods

```csharp
using War3Net.IO.Casc;
using War3Net.IO.Casc.Structures;
using War3Net.IO.Casc.Enums;

var archive = new CascArchive(@"C:\Games\WoW\Data");

// Method 1: Open by file name
using (var stream = archive.OpenFile("DBFilesClient\\Item.db2"))
{
    // Read item database
}

// Method 2: Open by content key (CKey)
var ckey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
using (var stream = archive.OpenFile(ckey))
{
    // Read file by content key
}

// Method 3: Open by encoded key (EKey)
var ekey = EKey.Parse("ABCDEF1234567890AB");
using (var stream = archive.OpenFile(ekey))
{
    // Read file by encoded key
}

// Method 4: Open by file data ID
uint fileDataId = 123456;
using (var stream = archive.OpenFile(fileDataId))
{
    // Read file by ID
}
```

### 2. Working with Encryption Keys

```csharp
using War3Net.IO.Casc;
using War3Net.IO.Casc.Crypto;

var archive = new CascArchive(@"C:\Games\WoW\Data");

// Add a single encryption key
ulong keyName = 0xFA505078126ACB3E;
byte[] key = new byte[] { /* 16-byte key */ };
archive.AddEncryptionKey(keyName, key);

// Load keys from a file
// File format: KeyName(hex) Key(hex)
// Example: FA505078126ACB3E 1234567890ABCDEF1234567890ABCDEF
CascEncryption.LoadKeysFromFile("encryption_keys.txt");

// Generate a key from a string (for some games)
var generatedKey = CascEncryption.ComputeSalsa20Key("some_key_string");
CascEncryption.AddKnownKey(keyName, generatedKey);
```

### 3. Progress Reporting

```csharp
using War3Net.IO.Casc.Progress;
using War3Net.IO.Casc.Storage;

// Create a progress reporter
var progressReporter = new CascProgressReporter();

// Subscribe to events
progressReporter.ProgressChanged += (sender, e) =>
{
    Console.WriteLine($"{e.GetFormattedMessage()}");
    
    // Cancel if needed
    if (ShouldCancel())
    {
        e.Cancel = true;
    }
};

progressReporter.StatusChanged += (sender, message) =>
{
    Console.WriteLine($"Status: {message}");
};

progressReporter.ErrorOccurred += (sender, error) =>
{
    Console.WriteLine($"Error: {error}");
};

// Use with storage opening
var args = new StorageOpenArgs
{
    LocalPath = @"C:\Games\WoW\Data",
    ProgressReporter = progressReporter,
    LocaleFlags = CascLocaleFlags.EnUS
};

// The reporter will receive progress updates during loading
var storage = CascStorage.OpenStorage(args.LocalPath, args.LocaleFlags);
```

### 4. Searching and Enumerating Files

```csharp
using War3Net.IO.Casc;

var archive = new CascArchive(@"C:\Games\WoW\Data");

// Find files by pattern
var icons = archive.FindFiles("Interface\\Icons\\*.blp");
foreach (var entry in icons)
{
    Console.WriteLine($"Found: {entry.FileName} (Size: {entry.FileSize})");
}

// Enumerate all files
foreach (var entry in archive)
{
    if (entry.IsEncrypted)
    {
        Console.WriteLine($"Encrypted: {entry.FileName}");
    }
}

// Get file information
var fileEntry = archive.GetEntry("some_file.txt");
Console.WriteLine($"Content Key: {fileEntry.CKey}");
Console.WriteLine($"Encoded Key: {fileEntry.EKey}");
Console.WriteLine($"File Size: {fileEntry.FileSize}");
Console.WriteLine($"Compressed Size: {fileEntry.CompressedSize}");
Console.WriteLine($"Locale Flags: {fileEntry.LocaleFlags}");
Console.WriteLine($"Content Flags: {fileEntry.ContentFlags}");
```

### 5. Working with Root Handlers

```csharp
using War3Net.IO.Casc.Root;
using War3Net.IO.Casc.Storage;

// Custom root handler implementation
public class CustomRootHandler : RootHandlerBase
{
    public override void Parse(Stream stream)
    {
        // Parse custom root format
        using var reader = new BinaryReader(stream);
        
        while (stream.Position < stream.Length)
        {
            var entry = new RootEntry
            {
                FileName = reader.ReadString(),
                CKey = reader.ReadCKey(),
                FileDataId = reader.ReadUInt32()
            };
            
            AddEntry(entry);
        }
    }
}

// Use custom root handler
var storage = CascStorage.OpenStorage(@"C:\Games\CustomGame\Data");
// Set custom root handler if needed
```

### 6. Direct Storage Access

```csharp
using War3Net.IO.Casc.Storage;
using War3Net.IO.Casc.Structures;

// Open storage directly for low-level access
var storage = CascStorage.OpenStorage(@"C:\Games\WoW\Data");

// Access by encoded key
var ekey = EKey.Parse("1234567890");
using (var stream = storage.OpenFileByEKey(ekey))
{
    // Read data
}

// Access by content key
var ckey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
using (var stream = storage.OpenFileByCKey(ckey))
{
    // Read data
}

// Add encryption keys to storage
storage.AddEncryptionKey(0x1234567890ABCDEF, encryptionKey);
```

### 7. BLTE Decompression

```csharp
using War3Net.IO.Casc.Compression;

// Check if data is BLTE-encoded
byte[] rawData = File.ReadAllBytes("some_file.dat");
if (BLTEDecoder.IsBLTE(rawData))
{
    // Decompress BLTE data
    byte[] decompressed = BLTEDecoder.Decode(rawData);
    
    // Process decompressed data...
}

// Stream-based decompression
using (var input = File.OpenRead("compressed.blte"))
using (var output = new MemoryStream())
{
    BLTEDecoder.Decode(input, output);
    var decompressedData = output.ToArray();
}
```

### 8. Index File Management

```csharp
using War3Net.IO.Casc.Index;

// Load index files manually
var indexManager = new IndexManager();
indexManager.LoadIndexFiles(@"C:\Games\WoW\Data\data");

// Find entry by encoded key
var ekey = EKey.Parse("1234567890");
if (indexManager.TryFindEntry(ekey, out var entry))
{
    Console.WriteLine($"Data file: data.{entry.DataFileIndex:D3}");
    Console.WriteLine($"Offset: {entry.DataFileOffset}");
    Console.WriteLine($"Size: {entry.EncodedSize}");
}
```

### 9. Encoding File Access

```csharp
using War3Net.IO.Casc.Encoding;

// Parse encoding file
var encoding = EncodingFile.ParseFile(@"C:\Games\WoW\Data\config\encoding");

// Look up encoded key for content key
var ckey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
var ekey = encoding.GetEKey(ckey);

// Look up content key for encoded key
var ekey2 = EKey.Parse("1234567890");
var ckey2 = encoding.GetCKey(ekey2);

// Get entry details
if (encoding.TryGetEntry(ckey, out var entry))
{
    Console.WriteLine($"Content size: {entry.ContentSize}");
    Console.WriteLine($"EKey count: {entry.EKeyCount}");
    foreach (var ek in entry.EKeys)
    {
        Console.WriteLine($"  EKey: {ek}");
    }
}
```

## Error Handling

```csharp
using War3Net.IO.Casc;

try
{
    var archive = new CascArchive(@"C:\Games\WoW\Data");
    var stream = archive.OpenFile("some_file.txt");
}
catch (DirectoryNotFoundException ex)
{
    Console.WriteLine($"CASC storage not found: {ex.Message}");
}
catch (CascFileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.FileName}");
    Console.WriteLine($"CKey: {ex.CKey}");
    Console.WriteLine($"EKey: {ex.EKey}");
}
catch (CascEncryptionException ex)
{
    Console.WriteLine($"Missing encryption key: 0x{ex.KeyName:X16}");
}
catch (CascParserException ex)
{
    Console.WriteLine($"Parse error in {ex.FileName}: {ex.Reason}");
}
catch (CascException ex)
{
    Console.WriteLine($"CASC error: {ex.Message}");
}
```

## Performance Tips

1. **Reuse Archive Instances** - Opening a CASC archive involves loading index files and manifests. Reuse the same instance for multiple file operations.

2. **Use Appropriate Access Methods** - Direct key access (CKey/EKey) is faster than name-based lookup.

3. **Batch Operations** - When reading multiple files, keep the archive open and read all files in sequence.

4. **Memory Management** - For large files, consider using streaming instead of loading entire files into memory.

```csharp
// Good: Reuse archive
using (var archive = new CascArchive(path))
{
    foreach (var fileName in fileList)
    {
        using var stream = archive.OpenFile(fileName);
        ProcessFile(stream);
    }
}

// Bad: Opening archive for each file
foreach (var fileName in fileList)
{
    using (var archive = new CascArchive(path))
    using (var stream = archive.OpenFile(fileName))
    {
        ProcessFile(stream);
    }
}
```

## Supported Games

The library has been designed to support CASC archives from:

- World of Warcraft (Retail and Classic)
- Diablo III
- Overwatch
- StarCraft II
- Heroes of the Storm
- Warcraft III: Reforged

Note: Game-specific root handlers may need to be implemented for full file name resolution in some games.

## Architecture

The library is organized into the following namespaces:

- `War3Net.IO.Casc` - Main API classes (CascArchive, CascEntry)
- `War3Net.IO.Casc.Compression` - BLTE compression/decompression
- `War3Net.IO.Casc.Crypto` - Encryption/decryption support
- `War3Net.IO.Casc.Encoding` - Encoding manifest parsing
- `War3Net.IO.Casc.Enums` - All enumeration types
- `War3Net.IO.Casc.Index` - Index file management
- `War3Net.IO.Casc.Progress` - Progress reporting
- `War3Net.IO.Casc.Root` - Root file handlers
- `War3Net.IO.Casc.Storage` - Low-level storage access
- `War3Net.IO.Casc.Structures` - Data structures
- `War3Net.IO.Casc.Utilities` - Helper utilities

## Limitations

- **Read-Only** - The library currently only supports reading CASC archives, not creating or modifying them
- **Local Storage** - Online/CDN storage support is not yet implemented
- **Root Handlers** - Only basic root handler is implemented; game-specific handlers may be needed

## Contributing

Contributions are welcome! Areas that could use improvement:

- Additional game-specific root handlers
- Online/CDN storage support
- Download/Install manifest parsing
- Performance optimizations
- Additional documentation and examples

## License

This library is part of the War3Net project and is licensed under the MIT license.

## Credits

- Based on the CascLib C++ implementation by Ladislav Zezula
- Part of the War3Net project by Drake53

## See Also

- [War3Net.IO.Mpq](../War3Net.IO.Mpq) - MPQ archive support
- [War3Net.IO.Compression](../War3Net.IO.Compression) - Compression algorithms
- [CascLib](https://github.com/ladislav-zezula/CascLib) - Original C++ implementation