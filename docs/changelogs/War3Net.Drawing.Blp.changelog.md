# War3Net.Drawing.Blp Changelog

All notable changes to the `War3Net.Drawing.Blp` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target frameworks; the package now targets `net6.0`, `net6.0-windows`, `net10.0`, and `net10.0-windows`.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target frameworks from .NET 5.0 to .NET 6.0 (`net5.0`/`net5.0-windows` to `net6.0`/`net6.0-windows`).

### Changed

- Updated `War3Net.Common` from v6.0.1 to v6.0.2.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

_No functional changes; version bumped to align with the rest of War3Net's v6.0.0 release._

## [v5.9.0] - 2025-10-30

### Added

- Added `BlpEncoder`, which writes BLP1 files with JPEG compression from BGRA pixel data, including automatic mipmap generation.
- Added `Blp1EncodingOptions`, to configure whether mipmaps are generated, the number of mipmap levels, the JPEG quality, and the extra flags field.

### Changed

- `JpegLibrary` is now referenced on the `-windows` target frameworks as well, so the encoder is available on every target framework.

## [v5.8.0] - 2025-09-06

### Changed

- Updated `War3Net.Common` from v5.6.1 to v5.8.0.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.Common` from v5.6.0 to v5.6.1.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.Common` from v5.5.5 to v5.6.0.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.Common` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.Common` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Changed

- Updated `War3Net.Common` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Updated `War3Net.Common` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Changed

- Updated `War3Net.Common` from v5.4.0 to v5.4.5.

## v5.4.1 - 2022-04-08

### Fixed

- Fixed `GetPixels` returning the wrong channel order for palettized BLPs: the colour palette is already read in BGRA order, but the result was swapped anyway when `bgra` was `true`, and left untouched when it was `false`. The decoder now reports the channel order of the decoded data, and the swap only happens when it doesn't match the requested order.
- Fixed `GetBitmapSource` producing incorrect colours for BLPs with `Direct` content: palettized images had their red and blue channels swapped, and images with colour encoding 3 were created with the 24bpp `Rgb24` format while the pixel data is 32bpp. Both now decode to BGRA and use `Bgra32`.
- Fixed `GetBitmapSource` for BLP-JPEG images using the pixel format reported by the JPEG decoder, which does not match the (non-standard) channel layout used by BLP1. The format is now chosen from the number of bytes per pixel: `Bgr24` for 3, `Bgra32` otherwise.

## v5.4.0 - 2022-02-13

### Added

- Added the `net5.0` target framework alongside `net5.0-windows`. On the non-Windows target, BLP-JPEG images are decoded with `JpegLibrary` instead of WPF, which is a new package dependency for that target framework.

### Changed

- `GetBitmapSource` is only available on the `net5.0-windows` target framework.
- Updated `War3Net.Common` from v5.0.0 to v5.4.0.

## v5.0.0 - 2020-12-14

### Breaking Changes

- Updated target framework from .NET Core 3.1 to .NET 5.0 (`net5.0-windows`).

### Changed

- Updated `War3Net.Common` from v0.3.0 to v5.0.0.

## v2.0.1 - 2020-10-27

### Breaking Changes

- The underlying type of the public `FileFormatVersion` enum changed from `uint` to `int`.

### Changed

- Header parsing now uses the `BinaryReader` extension methods from `War3Net.Common`, which is a new package dependency.
- An unrecognized file format version now throws `NotSupportedException`, and an unrecognized content type now throws `InvalidDataException`, instead of a plain `Exception`.

## v2.0.0 - 2020-09-14

### Breaking Changes

- Updated target frameworks from .NET Framework 4.6/.NET Standard 1.3/.NET Standard 2.0 to .NET Core 3.1. The package now always uses WPF, and is Windows-only.
- Removed `GetSKBitmap`, and with it the `SkiaSharp` dependency.
- Removed `GetBitmap`, and with it the `System.Drawing.Common` dependency.

### Added

- Added `Width` and `Height` properties.
- `GetPixels` now supports BLPs with JPEG content, by decoding them through `GetBitmapSource`. Previously it read the JPEG data as if it were palettized, and was documented as unsupported for that content type.
- `FileFormatVersion` is now public.

### Changed

- Opening a BLP-JPEG whose alpha depth is not 0 or 8 no longer throws `NotSupportedException`.

## v1.1.0 - 2019-07-09

### Added

- Implemented `Direct` (palettized and DXT compressed) content support in `GetSKBitmap` and `GetBitmapSource`; both previously threw `NotImplementedException` for that content type.
- The `mipMapLevel` parameter of `GetSKBitmap`, `GetBitmap`, and `GetBitmapSource` now defaults to `0`.

### Fixed

- Fixed `GetBitmapSource` returning inverted colours for BLP-JPEG images: it returned the decoded JPEG frame as-is, without inverting the channel values as BLP1 requires.

## v1.0.0 - 2019-07-08

### Added

- Initial release, based on [SereniaBLPLib](https://github.com/WoW-Tools/SereniaBLPLib). `BlpFile` reads BLP1 and BLP2 images (BLP0 is not supported) with JPEG, palettized, and DXT1/DXT3/DXT5 compressed content, and exposes `MipMapCount`, `GetPixels`, `GetSKBitmap`, `GetBitmap` (not available on .NET Standard 1.3), and `GetBitmapSource` (.NET Framework 4.6 only).

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.9.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.10.30
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
