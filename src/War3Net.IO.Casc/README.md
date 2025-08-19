# War3Net.IO.Casc

A .NET library for reading Blizzard's CASC (Content Addressable Storage Container) archives, used in modern Blizzard games including Warcraft III: Reforged, World of Warcraft, Diablo III, Overwatch, and StarCraft II.

## Features

- ✅ **Complete CASC Support** - Read files from local CASC storages
- ✅ **BLTE Decompression** - Support for BLTE compression with ZLib (LZMA, LZ4, and Zstandard placeholders included)
- ✅ **Encryption Support** - Salsa20 decryption for encrypted content with built-in key database
- ✅ **Multiple Access Methods** - Open files by name, content key, encoded key, or file data ID
- ✅ **Index File Support** - Parse both v1 and v2 index formats
- ✅ **Encoding Manifest** - Full encoding file support for key resolution
- ✅ **Root Handlers** - Extensible root file handling for different games
- ✅ **Progress Reporting** - Built-in progress reporting with cancellation support
- ✅ **Security Hardening** - Path sanitization with Unicode normalization attack prevention
- ✅ **Thread Safety** - Concurrent access support with proper locking
- ✅ **Memory Efficient** - ArrayPool usage for large allocations
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
using var archive = new CascArchive(@"C:\Games\Warcraft III\Data");

// Check if a file exists
if (archive.FileExists("war3.w3mod\\units\\human\\footman\\footman.mdx"))
{
    // Open and read the file
    using var stream = archive.OpenFile("war3.w3mod\\units\\human\\footman\\footman.mdx");
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
    @"C:\Games\Warcraft III\Data",
    CascLocaleFlags.EnUS | CascLocaleFlags.EnGB);

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

var archive = new CascArchive(@"C:\Games\Warcraft III\Data");

// Method 1: Open by file name
using (var stream = archive.OpenFile("war3.w3mod\\units\\unitdata.slk"))
{
    // Read unit data
}

// Method 2: Open by content key (CKey)
var cKey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
using (var stream = archive.OpenFile(cKey))
{
    // Read file by content key
}

// Method 3: Open by encoded key (EKey)
var eKey = EKey.Parse("ABCDEF1234567890AB");
using (var stream = archive.OpenFile(eKey))
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

var archive = new CascArchive(@"C:\Games\Warcraft III\Data");

// Add a single encryption key
ulong keyName = 0xFA505078126ACB3E;
byte[] key = new byte[] { /* 16-byte key */ };
archive.AddEncryptionKey(keyName, key);

// Add encryption key from hex string
archive.AddStringEncryptionKey(keyName, "1234567890ABCDEF1234567890ABCDEF");

// Import keys from a file
// File format: KeyName(hex)=Key(hex) or KeyName(hex) Key(hex)
// Example: FA505078126ACB3E=1234567890ABCDEF1234567890ABCDEF
int keysImported = archive.ImportKeysFromFile("encryption_keys.txt");

// Import keys from string
string keyList = @"
FA505078126ACB3E=BDC51862ABED79B2DE48C8E7E66C6200
# Comments are supported
FF813F7D062AC0BC=AA0B5C77F088CCC2D39049BD267F066D
";
int imported = archive.ImportKeysFromString(keyList);

// Load global keys (affects all archives)
CascEncryption.LoadKeysFromFile("encryption_keys.txt");

// Generate a key from a string (for some games)
var generatedKey = CascEncryption.ComputeSalsa20Key("some_key_string");
CascEncryption.AddKnownKey(keyName, generatedKey);

// Check if a key is available
if (CascEncryption.HasKey(keyName))
{
    var keyData = CascEncryption.GetKey(keyName);
}

// Note: The library includes many built-in encryption keys for:
// - Warcraft III Reforged
// - World of Warcraft
// - Overwatch
// - StarCraft II
// - Battle.net App
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
    LocalPath = @"C:\Games\Warcraft III\Data",
    ProgressReporter = progressReporter,
    LocaleFlags = CascLocaleFlags.EnUS,
};

// The reporter will receive progress updates during loading
var storage = CascStorage.OpenStorage(args.LocalPath, args.LocaleFlags);
```

### 4. Searching and Enumerating Files

```csharp
using War3Net.IO.Casc;

var archive = new CascArchive(@"C:\Games\Warcraft III\Data");

// Find files by pattern
var models = archive.FindFiles("war3.w3mod\\units\\*.mdx");
foreach (var entry in models)
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
var fileEntry = archive.GetEntry("war3.w3mod\\scripts\\common.j");
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
                FileDataId = reader.ReadUInt32(),
            };
            
            AddEntry(entry);
        }
    }
}

// Use custom root handler
var storage = CascStorage.OpenStorage(@"C:\Games\CustomGame\Data");
// Set custom root handler if needed
```

### 6. CDN Client Usage

```csharp
using War3Net.IO.Casc.Cdn;
using System.Net.Http;

// Method 1: Download from CDN using default servers
var cdnClient = new CdnClient("eu", "tpr/war3");

// Download a config file by hash
var configHash = "1234567890abcdef1234567890abcdef";
var configData = await cdnClient.DownloadConfigAsync(configHash);

// Download a data file by hash
var dataHash = "abcdef1234567890abcdef1234567890";
var dataFile = await cdnClient.DownloadDataAsync(dataHash);

// Download a patch file by hash
var patchHash = "fedcba0987654321fedcba0987654321";
var patchData = await cdnClient.DownloadPatchAsync(patchHash);

// Method 2: Get CDN configuration from Blizzard servers
using var httpClient = new HttpClient();

// Download versions file to get config hashes
var versionsUrl = "http://eu.patch.battle.net:1119/w3/versions";
var versionsStream = await httpClient.GetStreamAsync(versionsUrl);
var versions = VersionConfig.Parse(versionsStream);

// Get version entry for region
var euEntry = versions.GetEntry("eu");
Console.WriteLine($"Build config: {euEntry.BuildConfig}");
Console.WriteLine($"CDN config: {euEntry.CdnConfig}");
Console.WriteLine($"Version: {euEntry.VersionsName}");

// Download CDN servers configuration
var cdnsUrl = "http://eu.patch.battle.net:1119/w3/cdns";
var cdnsStream = await httpClient.GetStreamAsync(cdnsUrl);
var cdns = CdnServersConfig.Parse(cdnsStream);

// Get CDN entry for region
var cdnEntry = cdns.GetEntry("eu");
Console.WriteLine($"CDN Path: {cdnEntry.Path}");
foreach (var host in cdnEntry.Hosts)
{
    Console.WriteLine($"CDN Host: {host}");
}

// Method 3: Create CDN client with specific servers
var cdnHosts = new List<string>
{
    "https://level3.blizzard.com",
    "https://blzddist1-a.akamaihd.net",
    "http://level3.blizzard.com",  // HTTP fallback
};

using var customClient = new CdnClient(cdnHosts, "tpr/war3");

// Download build configuration using hash from versions
var buildConfigData = await customClient.DownloadConfigAsync(euEntry.BuildConfig);
var buildConfig = System.Text.Encoding.UTF8.GetString(buildConfigData);

// Download CDN configuration
var cdnConfigData = await customClient.DownloadConfigAsync(euEntry.CdnConfig);
var cdnConfig = System.Text.Encoding.UTF8.GetString(cdnConfigData);

// Method 4: Online storage with CDN support
using War3Net.IO.Casc.Storage;

// Open online storage (downloads from CDN as needed)
var onlineStorage = await OnlineCascStorage.OpenWar3Async(
    region: "eu",
    localCachePath: @"C:\CascCache",
    progressReporter: new CascProgressReporter());

// Files are downloaded from CDN on-demand
using (var stream = onlineStorage.OpenFile("war3.w3mod\\units\\human\\footman\\footman.mdx"))
{
    // File is downloaded from CDN if not cached locally
}

// CDN client automatically retries with:
// - Multiple CDN servers (failover)
// - Exponential backoff with jitter
// - HTTPS with HTTP fallback
// - Configurable timeouts
```

### 7. Direct Storage Access

```csharp
using War3Net.IO.Casc.Storage;
using War3Net.IO.Casc.Structures;

// Open storage directly for low-level access
var storage = CascStorage.OpenStorage(@"C:\Games\Warcraft III\Data");

// Access by encoded key
var eKey = EKey.Parse("1234567890");
using (var stream = storage.OpenFileByEKey(eKey))
{
    // Read data
}

// Access by encoded key with streaming (for large files)
using (var stream = storage.OpenFileByEKey(eKey, useStreaming: true))
{
    // Stream data without loading entire file into memory
}

// Access by content key
var cKey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
using (var stream = storage.OpenFileByCKey(cKey))
{
    // Read data
}

// Access with checksum validation
using (var stream = storage.OpenFileByCKey(cKey, validateChecksum: true))
{
    // Read data with MD5 validation
}

// Add encryption keys to storage
storage.AddEncryptionKey(0x1234567890ABCDEF, encryptionKey);

// Import keys from string
storage.ImportKeysFromString("keyname=keyvalue\nkeyname2=keyvalue2");
```

### 8. BLTE Decompression

```csharp
using War3Net.IO.Casc.Compression;

// Check if data is BLTE-encoded
byte[] rawData = File.ReadAllBytes("some_file.dat");
if (BlteDecoder.IsBlte(rawData))
{
    // Decompress BLTE data
    byte[] decompressed = BlteDecoder.Decode(rawData);
    
    // Process decompressed data...
}

// Stream-based decompression
using (var input = File.OpenRead("compressed.blte"))
using (var output = new MemoryStream())
{
    BlteDecoder.Decode(input, output);
    var decompressedData = output.ToArray();
}

// Configure recursion depth for nested BLTE frames
BlteDecoder.MaxRecursionDepth = 30; // Default is 20

// Note: The decoder includes safety checks:
// - Maximum frame size: 100MB per frame
// - Maximum total size: 500MB for all frames
// - Recursion depth protection against malformed data
```

### 9. Index File Management

```csharp
using War3Net.IO.Casc.Index;

// Load index files manually
var indexManager = new IndexManager();
indexManager.LoadIndexFiles(@"C:\Games\Warcraft III\Data\data");

// Find entry by encoded key
var eKey = EKey.Parse("1234567890");
if (indexManager.TryFindEntry(eKey, out var entry))
{
    Console.WriteLine($"Data file: data.{entry.DataFileIndex:D3}");
    Console.WriteLine($"Offset: {entry.DataFileOffset}");
    Console.WriteLine($"Size: {entry.EncodedSize}");
}
```

### 10. Encoding File Access

```csharp
using War3Net.IO.Casc.Encoding;

// Parse encoding file
var encoding = EncodingFile.ParseFile(@"C:\Games\Warcraft III\Data\config\encoding");

// Look up encoded key for content key
var cKey = CascKey.Parse("1234567890ABCDEF1234567890ABCDEF");
var eKey = encoding.GetEKey(cKey);

// Look up content key for encoded key
var ekey2 = EKey.Parse("1234567890");
var ckey2 = encoding.GetCKey(ekey2);

// Get entry details
if (encoding.TryGetEntry(cKey, out var entry))
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
    var archive = new CascArchive(@"C:\Games\Warcraft III\Data");
    var stream = archive.OpenFile("war3.w3mod\\scripts\\blizzard.j");
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

4. **Memory Management** - For large files, consider using streaming instead of loading entire files into memory. The library uses ArrayPool for allocations over 80KB to reduce GC pressure.

5. **Thread Safety** - The library is thread-safe for concurrent reads. Multiple threads can safely read different files from the same archive instance.

6. **Encryption Keys** - Pre-load all required encryption keys at startup to avoid runtime key loading overhead.

```csharp
// Good: Reuse archive
using var archive = new CascArchive(path);
foreach (var fileName in fileList)
{
    using var stream = archive.OpenFile(fileName);
    ProcessFile(stream);
}

// Bad: Opening archive for each file
foreach (var fileName in fileList)
{
    using var archive = new CascArchive(path);
    using var stream = archive.OpenFile(fileName);
    ProcessFile(stream);
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

- `War3Net.IO.Casc` - Main API classes (CascArchive, CascEntry, CascStream)
- `War3Net.IO.Casc.Cdn` - CDN client implementation (partial)
- `War3Net.IO.Casc.Compression` - BLTE compression/decompression (BlteDecoder, BlteHeader, BlteFrame)
- `War3Net.IO.Casc.Crypto` - Encryption/decryption support (CascEncryption, Salsa20)
- `War3Net.IO.Casc.Encoding` - Encoding manifest parsing (EncodingFile, EncodingEntry)
- `War3Net.IO.Casc.Enums` - All enumeration types (CascOpenFlags, CascLocaleFlags, etc.)
- `War3Net.IO.Casc.Index` - Index file management (IndexManager, IndexFile)
- `War3Net.IO.Casc.Progress` - Progress reporting (CascProgressReporter)
- `War3Net.IO.Casc.Root` - Root file handlers (BasicRootHandler, TextRootHandler)
- `War3Net.IO.Casc.Storage` - Low-level storage access (CascStorage, OnlineCascStorage)
- `War3Net.IO.Casc.Structures` - Data structures (CascKey, EKey, CascFindData)
- `War3Net.IO.Casc.Utilities` - Helper utilities (PathSanitizer, HashHelper, ChecksumValidator)

## Limitations

- **Read-Only** - The library currently only supports reading CASC archives, not creating or modifying them
- **CDN Support** - Basic CDN client is implemented for downloading files, but full online storage integration is still in progress
- **Root Handlers** - Only basic root handler is implemented; game-specific handlers may be needed
- **Compression Formats** - Currently supports ZLib compression; LZMA, LZ4, and Zstandard decompression are not yet implemented but have placeholders

## Contributing

Contributions are welcome! Areas that could use improvement:

- Additional game-specific root handlers
- Complete online/CDN storage support implementation
- Download/Install manifest parsing
- LZMA, LZ4, and Zstandard compression support
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