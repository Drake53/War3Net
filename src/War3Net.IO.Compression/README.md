# War3Net.IO.Compression

## About

War3Net.IO.Compression is a .NET library providing compression and decompression algorithms used in MPQ (MoPaQ) archives. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library for Warcraft III.

## Key features

* Compress and decompress data using ZLib (DEFLATE)
* Decompress PKLib (PKWARE) compressed data
* Decompress Huffman encoded data
* Decompress BZip2 compressed data
* Decompress IMA ADPCM audio data (mono and stereo)

## How to Use

### Compress data with ZLib

```csharp
using War3Net.IO.Compression;

// Compress a stream
using var inputStream = File.OpenRead("input.dat");
using var compressedStream = ZLibCompression.Compress(inputStream, (int)inputStream.Length, leaveOpen: false);

// Write compressed data to file
using var outputStream = File.Create("output.dat");
compressedStream.Position = 0;
compressedStream.CopyTo(outputStream);
```

### Decompress data with ZLib

```csharp
using War3Net.IO.Compression;

byte[] compressedData = GetCompressedData();
uint expectedLength = 1024; // Expected decompressed size

byte[] decompressedData = ZLibCompression.Decompress(compressedData, expectedLength);
```

### Decompress PKLib data

```csharp
using War3Net.IO.Compression;

byte[] compressedData = GetPKLibCompressedData();
uint expectedLength = 2048;

byte[] decompressedData = PKLibCompression.Decompress(compressedData, expectedLength);
```

### Decompress Huffman encoded data

```csharp
using War3Net.IO.Compression;

byte[] compressedData = GetHuffmanCompressedData();

byte[] decompressedData = HuffmanCoding.Decompress(compressedData);
```

### Decompress IMA ADPCM audio

```csharp
using War3Net.IO.Compression;

byte[] compressedAudio = GetADPCMCompressedAudio();
int channelCount = 2; // 1 for mono, 2 for stereo

byte[] decompressedAudio = AdpcmCompression.Decompress(compressedAudio, channelCount);
```

## Main Types

The main types provided by this library are:

* `War3Net.IO.Compression.ZLibCompression` - Compress and decompress using ZLib (DEFLATE)
* `War3Net.IO.Compression.PKLibCompression` - Decompress PKLib (PKWARE) compressed data
* `War3Net.IO.Compression.HuffmanCoding` - Decompress Huffman encoded data
* `War3Net.IO.Compression.BZip2Compression` - Decompress BZip2 compressed data
* `War3Net.IO.Compression.AdpcmCompression` - Decompress IMA ADPCM audio data

## Related Packages

* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files

## Feedback and contributing

War3Net.IO.Compression is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.