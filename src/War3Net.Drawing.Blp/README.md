# War3Net.Drawing.Blp

## About

War3Net.Drawing.Blp is a .NET library for reading and writing BLP texture files, an image format used by Blizzard games including Warcraft III and World of Warcraft. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Read BLP1 files (Warcraft III: Reign of Chaos and The Frozen Throne)
* Read BLP2 files (World of Warcraft)
* Encode images to BLP1 format with JPEG compression
* Support for multiple compression types (JPEG, palettized, DXT1/DXT3/DXT5)
* Extract pixel data in BGRA or RGBA format
* Access all mipmap levels within a BLP file
* Automatic mipmap generation when encoding
* Windows-specific `BitmapSource` conversion (WPF)

## How to Use

### Read a BLP file and extract pixel data

```csharp
using War3Net.Drawing.Blp;

// Open and read the BLP file
using var stream = File.OpenRead("texture.blp");
using var blpFile = new BlpFile(stream);

// Get image dimensions
int width = blpFile.Width;
int height = blpFile.Height;

// Extract pixel data (BGRA format by default)
byte[] pixels = blpFile.GetPixels(mipMapLevel: 0, out int w, out int h);
```

### Extract pixel data in RGBA format

```csharp
using War3Net.Drawing.Blp;

using var stream = File.OpenRead("texture.blp");
using var blpFile = new BlpFile(stream);

// Get pixels in RGBA order instead of BGRA
byte[] rgbaPixels = blpFile.GetPixels(mipMapLevel: 0, out int w, out int h, bgra: false);
```

### Access mipmap levels

```csharp
using War3Net.Drawing.Blp;

using var stream = File.OpenRead("texture.blp");
using var blpFile = new BlpFile(stream);

// Get the number of available mipmaps
int mipMapCount = blpFile.MipMapCount;

// Extract a specific mipmap level (0 = largest, higher = smaller)
for (int level = 0; level < mipMapCount; level++)
{
    byte[] mipMapPixels = blpFile.GetPixels(level, out int mipWidth, out int mipHeight);
    // Process mipmap...
}
```

### Encode an image to BLP1 format

```csharp
using War3Net.Drawing.Blp;

// Prepare BGRA pixel data (4 bytes per pixel)
int width = 256;
int height = 256;
byte[] bgraPixels = new byte[width * height * 4];
// ... populate pixel data ...

// Configure encoding options
var options = new Blp1EncodingOptions
{
    GenerateMipmaps = true,
    JpegQuality = 85,
};

// Encode to BLP1
var encoder = new BlpEncoder(options);
using var outputStream = File.Create("output.blp");
encoder.Encode(outputStream, width, height, bgraPixels);
```

### Convert to BitmapSource (Windows WPF)

```csharp
using War3Net.Drawing.Blp;
using System.Windows.Media.Imaging;

using var stream = File.OpenRead("texture.blp");
using var blpFile = new BlpFile(stream);

// Get a WPF BitmapSource (Windows only)
BitmapSource bitmap = blpFile.GetBitmapSource(mipMapLevel: 0);
```

## Main Types

The main types provided by this library are:

* `War3Net.Drawing.Blp.BlpFile` - Reads and decodes BLP image files
* `War3Net.Drawing.Blp.BlpEncoder` - Encodes images to BLP1 format with JPEG compression
* `War3Net.Drawing.Blp.Blp1EncodingOptions` - Configuration options for BLP1 encoding
* `War3Net.Drawing.Blp.FileFormatVersion` - BLP format version identifiers (BLP0, BLP1, BLP2)

## Related Packages

* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.IO.Mpq](https://www.nuget.org/packages/War3Net.IO.Mpq) - Read and write MPQ archives

## Feedback and contributing

War3Net.Drawing.Blp is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.