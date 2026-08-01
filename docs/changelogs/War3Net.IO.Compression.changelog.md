# War3Net.IO.Compression Changelog

All notable changes to the `War3Net.IO.Compression` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- The `compressionLevel` parameter of `ZLibCompression.Compress` changed from `Ionic.Zlib.CompressionLevel` to `System.IO.Compression.CompressionLevel`.
- Updated target framework from .NET 5.0 to .NET 6.0.

### Added

- Added `Crc32Checksum` class with a `Compute(Stream)` method, backed by `System.IO.Hashing`.

### Changed

- The default compression level used by `ZLibCompression.Compress(Stream, int, bool)` changed from `BestCompression` to `Optimal`.
- Updated `War3Net.Common` from v6.0.1 to v6.0.2.

### Security

- Replaced the vulnerable `DotNetZip` dependency: ZLib compression/decompression now uses `System.IO.Compression.ZLibStream`, and BZip2 decompression uses `SharpZipLib` again.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

_No functional changes; version bumped to align with the rest of War3Net's v6.0.0 release._

## [v5.8.0] - 2025-09-06

### Added

- Added `ZLibCompression.Compress(Stream, int, CompressionLevel, bool)` overload, to control the compression level (the existing overload keeps using `BestCompression`).

### Changed

- Renamed the `bytes` parameter of `ZLibCompression.Compress` to `bytesToCompress`.
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

### Added

- Added optional `throwOnLessBytesThanExpected` parameter to both `ZLibCompression.Decompress` overloads.

### Changed

- `ZLibCompression.Decompress` now throws an `ArgumentException` when the decompressed data is shorter than `expectedLength`, instead of silently returning a zero-padded array. Pass `throwOnLessBytesThanExpected: false` to get an array resized to the number of bytes actually read.
- Updated `War3Net.Common` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Updated `DotNetZip` from v1.15.0 to v1.16.0, to fix a package version conflict.
- Updated `War3Net.Common` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Changed

- Updated `War3Net.Common` from v5.4.0 to v5.4.5.

## v5.4.0 - 2022-02-13

### Changed

- Updated `War3Net.Common` from v5.0.2 to v5.4.0.

## v5.1.0 - 2021-02-14

### Changed

- `BZip2Compression.Decompress` now uses `DotNetZip` instead of `SharpZipLib`, removing the `SharpZipLib` dependency.
- Updated `War3Net.Common` from v5.0.1 to v5.0.2.

## v5.0.1 - 2020-12-25

### Changed

- Updated `War3Net.Common` from v5.0.0 to v5.0.1.

## v5.0.0 - 2020-12-14

### Breaking Changes

- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Changed

- Updated `DotNetZip` from v1.13.8 to v1.15.0 and `SharpZipLib` from v1.2.0 to v1.3.1.
- Updated `War3Net.Common` from v0.3.2 to v5.0.0.

## v1.0.2 - 2020-11-11

### Changed

- Updated `War3Net.Common` from v0.3.0 to v0.3.2.

## v1.0.1 - 2020-10-27

### Breaking Changes

- Updated target framework from .NET Standard 2.1 to .NET Core 3.1.

### Changed

- Updated `War3Net.Common` from v0.2.0 to v0.3.0.

## v1.0.0 - 2020-09-14

### Breaking Changes

- Updated target frameworks from .NET Framework 4.5/.NET Standard 2.0 to .NET Standard 2.1.
- Renamed the compression classes: `MpqHuffman` to `HuffmanCoding`, `MpqWavCompression` to `AdpcmCompression`, `Deflate` to `ZLibCompression`, and `PKLibDecompress` to `PKLibCompression`.
- `PKLibCompression` is now a static class; its instance API (constructor plus `Explode`) has been replaced by `Decompress`.
- `Deflate.TryCompress` has been replaced by `ZLibCompression.Compress`, which returns a `Stream` containing the compressed data instead of writing to a caller-supplied stream and returning the compressed size.
- `HuffmanCoding.Decompress` now returns `byte[]` instead of `MemoryStream`.
- The `expectedLength` parameters changed from `int` to `uint`.
- Removed the `CompressionType` enum; it moved to `War3Net.IO.Mpq` as `MpqCompressionType`.
- Removed the `StreamExtensions` class; it moved to `War3Net.Common`, which is now a package dependency.

### Added

- Added `BZip2Compression` class, for decompressing BZip2 compressed data.
- Added `byte[]` overloads of `Decompress` alongside the existing `Stream` overloads on every compression class.
- `BitStream` is now public, implements `IDisposable`, and gained a constructor with a `leaveOpen` parameter.

### Changed

- Updated `DotNetZip` from v1.13.4 to v1.13.8.

### Fixed

- Fixed PKLib decompression producing incorrect results: the `_sPosition1`/`_sPosition2` decode tables were generated from static fields that had not been initialized yet, and are now built in a static constructor.

## v0.1.0 - 2019-10-15

### Added

- Initial release, with `Deflate` (ZLib compression and decompression), `PKLibDecompress`, `MpqHuffman`, and `MpqWavCompression` (IMA ADPCM) for the compression methods used in MPQ archives, plus the `CompressionType` enum.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
