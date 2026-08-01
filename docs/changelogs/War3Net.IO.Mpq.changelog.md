# War3Net.IO.Mpq Changelog

All notable changes to the `War3Net.IO.Mpq` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

_No unreleased changes._

## [v6.0.3] - 2026-07-04

### Added

- Added `Count` and `EnumerateHashes()` to `MpqArchive`.
- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

### Changed

- Changed `MpqArchive[int]` from internal to public.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- `Attributes.Crc32s` changed from `List<int>` to `List<uint>`, matching the actual unsigned nature of CRC32 values.
- Updated target framework from .NET 5.0 to .NET 6.0.

### Security

- Replaced the vulnerable `DotNetZip` dependency with `System.IO.Compression` and an internal CRC32 implementation.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

_No functional changes; version bumped to align with the rest of War3Net's v6.0.0 release._

## [v5.8.1] - 2025-09-12

### Fixed

- Fixed `MpqStream` not disposing its underlying stream, so `using` statements around an `MpqStream` now properly release the base stream.

## [v5.8.0] - 2025-09-06

### Added

- Added `MpqStreamFactory` class as the entry point for constructing `MpqStream` instances, replacing direct use of internal `MpqStream` constructors.
- Added `MpqStreamUtils` class with `Compress` and `Encrypt` methods, for compressing/encrypting raw data into the block-and-header layout used by MPQ files independently of building a full archive.
- Added `IMpqCompressor` interface and `MpqZLibCompressor` implementation, allowing a custom compression algorithm to be plugged into `MpqStreamUtils.Compress`.
- Added `MpqEncryptionUtils` class, exposing `CalculateEncryptionSeed` publicly.

### Fixed

- Fixed an encrypted single-unit `MpqStream` not being decrypted correctly because the check used the uncompressed size instead of the compressed size.
- Fixed `MpqFileFlags.BlockOffsetAdjustedKey` handling not being applied correctly in some cases.

## v5.6.1 - 2023-01-07

### Changed

- Updated `War3Net.IO.Compression` from v5.6.0 to v5.6.1.

## v5.6.0 - 2022-12-20

### Changed

- Updated `War3Net.IO.Compression` from v5.5.5 to v5.6.0.

## v5.5.5 - 2022-11-13

### Changed

- Updated `War3Net.IO.Compression` from v5.5.3 to v5.5.5.

## v5.5.3 - 2022-10-29

### Changed

- Updated `War3Net.IO.Compression` from v5.5.2 to v5.5.3.

## v5.5.2 - 2022-10-25

### Added

- Added `UserData` class, exposing the `"(user data)"` filename constant.

### Changed

- Improved parallel read performance by using `MemoryMappedFile` for archive access.
- `MpqHeader.Parse` gained an optional `leaveOpen` parameter.
- Updated `War3Net.IO.Compression` from v5.5.0 to v5.5.2.

## v5.5.0 - 2022-08-20

### Changed

- Updated `War3Net.IO.Compression` from v5.4.5 to v5.5.0.

## v5.4.5 - 2022-05-27

### Changed

- Updated `War3Net.IO.Compression` from v5.4.0 to v5.4.5.

### Fixed

- The CRC32 of a file is no longer computed when its `MpqStream` cannot be read or seeked, which threw when saving an archive with the `Crc32` attributes flag set.
- Fixed `MpqFile` comparing `MpqStream.FilePosition` against the relative file offset instead of the absolute one when deciding whether a file's encrypted block offsets need to be recalculated.
- `MpqStream` is now marked as unreadable when the file is encrypted but no encryption seed could be determined, instead of producing garbage data.

## v5.4.3 - 2022-03-20

### Changed

- Made the listfile/attributes/signature binary read/write extension methods public.
- `MpqFile` now checks `MpqStream.CanRead` instead of the removed `MpqStream.CanBeDecrypted`.
- Moved known `MpqCompressionType` values into a public extension method.

### Fixed

- `MpqStream` with the `SingleUnit` flag no longer buffers data eagerly in its constructor, and now validates `CanRead` and throws on read/seek (instead of in the constructor) when block positions are invalid, avoiding premature exceptions when opening certain archives.

## v5.4.0 - 2022-02-13

### Changed

- Updated `War3Net.IO.Compression` from v5.1.0 to v5.4.0.

## v5.3.0 - 2022-01-16

_No functional changes; version bumped to align with the rest of War3Net's v5.3.0 release._

## v5.2.1 - 2021-07-23

### Fixed

- Fixed incorrect hash table size calculation when creating an archive, which could produce a hash table larger than `MpqTable.MaxSize` or ignore the configured `HashTableSize` (fixes #14).

## v5.2.0 - 2021-04-27

### Added

- Implemented `MpqArchiveExtensions.VerifySignature`, including an overload that verifies against Blizzard's known public keys automatically.
- Added support for signing MPQ archives on creation via `MpqArchiveCreateOptions.SignaturePrivateKey`.
- Added support for the `Unk0x04` attributes flag in `AttributesFlags`/`Attributes`, including verification support in `VerifyAttributes`.

### Changed

- Doubled `MpqTable.MaxSize`.

## v5.1.3 - 2021-02-14

### Changed

- `MpqArchiveBuilder.SaveTo(string, ...)` overloads now use `FileProvider.CreateFileAndFolder` instead of `File.Create`, so parent folders are created automatically.

### Removed

- Removed obsolete `FileProvider.FileExists`, `FileProvider.GetFile`, and `FileProvider.EnumerateFiles` methods.

## v5.1.2 - 2021-01-09

### Changed

- `MpqArchiveBuilder` is no longer sealed.

## v5.1.1 - 2021-01-01

### Changed

- Minor fixes across `BlockTable`, `HashTable`, `MpqArchive`, `MpqArchiveBuilder`, and `MpqArchiveCreateOptions`.

## v5.1.0 - 2020-12-25

### Changed

- `MpqArchiveBuilder` refactored: constructors and `AddFile`/`RemoveFile`/`SaveTo` methods reworked around the new `MpqArchiveCreateOptions`-based creation API.

### Fixed

- Fixed inconsistent casing across several public members.
- Fixed encryption seed calculation.
- Various other minor MPQ fixes (`FileProvider`, `MpqEntry`, `MpqStream`, `StormBuffer`).

## v5.0.0 - 2020-12-14

### Breaking Changes

- `Attributes` changed from a static extension-method holder to an instantiable sealed class with `Unk`, `Flags`, `Crc32s`, and `DateTimes` properties.
- Added `AttributesFlags` enum.
- `MpqArchive` creation now takes an `MpqArchiveCreateOptions` object instead of separate `hashTableSize`/`blockSize`/`writeArchiveFirst` constructor parameters; removed the `DefaultBlockSize` constant.
- `MpqFile.Name` no longer throws when unset.

### Added

- `MpqArchiveBuilder.AddFile` gained an overload accepting explicit `MpqFileFlags`; new `SaveTo` overloads accepting `MpqArchiveCreateOptions`.

### Changed

- `MpqFile` now implements `IComparable`, `IComparable<MpqFile>`, and `IEquatable<MpqFile>`, backed by new `MpqFileComparer`, replacing the old `IsSameAs` method.

## v1.2.2 - 2020-11-12

### Added

- Added `leaveOpen` parameter to the `MpqArchive` constructor, `MpqArchive.Create`, and `MpqArchiveBuilder.SaveTo`, to control whether the underlying stream is disposed.

### Changed

- `MpqFile.MpqStream` property is now public.

## v1.2.1 - 2020-11-12

### Added

- Added `MpqArchiveBuilder.RemoveFile(ulong)` overload, alongside the existing `RemoveFile(string)`.

## v1.2.0 - 2020-11-11

### Added

- Added `MpqArchiveBuilder` class, for incrementally building or modifying an `MpqArchive` (supports adding/removing files and saving to a new archive).

### Changed

- `MpqArchive.HashTableSize` property is now public.

## v1.1.1 - 2020-10-27

### Breaking Changes

- Updated target framework from .NET Standard 2.1 to .NET Core 3.1.

### Changed

- Updated `War3Net.IO.Compression` from v1.0.0 to v1.0.1.

## v1.1.0 - 2020-09-14

### Breaking Changes

- `FileProvider.OpenNewWrite` has been renamed to `CreateFileAndFolder`.

### Added

- Added `Attributes` class with `VerifyAttributes` extension method, to verify the MPQ "(attributes)" file.
- Added file-existence and open methods to `MpqFile`.

### Changed

- Moved `FileProvider` class into the `War3Net.IO.Mpq` namespace.

### Fixed

- Fixed `MpqFile.OpenRead` to properly close the underlying `MpqStream` after copying its contents.

## v1.0.1 - 2020-08-10

### Fixed

- Various MPQ library bugfixes in `MpqArchive`, `MpqHeader`, and `StormBuffer`.

## v1.0.0 - 2020-07-10

### Added

- Added `MpqOrphanedFile` class to represent entries in the hash table with no corresponding filename.
- Added `TryGetHashString` method.

### Fixed

- Fixed "Unable to read beyond the end of the stream" exception in `BlockTable`.
- Fixed "Unable to determine encryption seed" exception.
- Fixed "Invalid enum" exception in `MpqArchive`.
- Fixed `MpqParserException` when parsing Wurst maps.
- Fixed exception when listfile contains garbage data.
- `MpqEntry.Filename` is now correctly null if no `MpqHash` references the entry.
- Orphaned `MpqEntries` no longer use deleted `MpqHashes`.
- Improved `MpqHeader` exception messages.

## v0.1.0 - 2019-10-15

### Added

- Initial release, based on Foole's MPQ library (itself based on StormLib), with support for reading and creating MPQ archives:
  - `MpqArchive` for opening, creating, repairing, and restoring archives, and for replacing individual files.
  - `MpqFile`, `MpqEntry`, `MpqHash`, and `MpqStream` for working with the files inside an archive, including compression and encryption.
  - `MpqHeader`, `HashTable`, `BlockTable`, and `ListFile` for the archive's internal structures.
  - `MpqLocale`, `MpqLocaleProvider`, and `MpqFileFlags` for file metadata.
  - `MpqParserException`, thrown when an archive cannot be parsed.

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.3]: https://github.com/Drake53/War3Net/releases/tag/v6.0.3
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.1]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.12
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
