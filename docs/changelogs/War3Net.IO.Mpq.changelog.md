# War3Net.IO.Mpq Changelog

## v6.0.3
### Changes
- Added `Count` and `EnumerateHashes()` to `MpqArchive`.
- Changed `MpqArchive[int]` from internal to public.

## v6.0.2
### Breaking changes
- `Attributes.Crc32s` changed from `List<int>` to `List<uint>`, matching the actual unsigned nature of CRC32 values.
- Updated target framework from .NET 5.0 to .NET 6.0.
### Changes
- Replaced the vulnerable DotNetZip dependency with `System.IO.Compression` and an internal CRC32 implementation.

## v6.0.1
No functional changes; package readme and metadata updated for nuget.org presentation.

## v6.0.0
No functional changes; version bumped to align with the rest of War3Net's v6.0.0 release.

## v5.8.1
### Bugfixes
- Fixed `MpqStream` not disposing its underlying stream, so `using` statements around an `MpqStream` now properly release the base stream.

## v5.8.0
### Changes
- Added `MpqStreamFactory` class as the entry point for constructing `MpqStream` instances, replacing direct use of internal `MpqStream` constructors.
- Added `MpqStreamUtils` class with `Compress` and `Encrypt` methods, for compressing/encrypting raw data into the block-and-header layout used by MPQ files independently of building a full archive.
- Added `IMpqCompressor` interface and `MpqZLibCompressor` implementation, allowing a custom compression algorithm to be plugged into `MpqStreamUtils.Compress`.
- Added `MpqEncryptionUtils` class, exposing `CalculateEncryptionSeed` publicly.
### Bugfixes
- Fixed an encrypted single-unit `MpqStream` not being decrypted correctly because the check used the uncompressed size instead of the compressed size.
- Fixed `MpqFileFlags.BlockOffsetAdjustedKey` handling not being applied correctly in some cases.

## v5.5.2
### Changes
- Improved parallel read performance by using `MemoryMappedFile` for archive access.

## v5.5.0
### Changes
- `MpqStream` now uses the `CopyTo` extension method internally.
- `MpqHeader.Parse` gained an optional `leaveOpen` parameter.
### Bugfixes
- Fixed a typo affecting behavior in the Mpq read path.

## v5.4.5
### Changes
- Made the listfile/attributes/signature binary read/write extension methods public.
- `MpqFile` now checks `MpqStream.CanRead` instead of the removed `MpqStream.CanBeDecrypted`.
- Moved known `MpqCompressionType` values into a public extension method.
### Bugfixes
- `MpqStream` with the `SingleUnit` flag no longer buffers data eagerly in its constructor, and now validates `CanRead` and throws on read/seek (instead of in the constructor) when block positions are invalid, avoiding premature exceptions when opening certain archives.

## v5.2.1
### Bugfixes
- Fixed incorrect hash table size calculation when creating an archive, which could produce a hash table larger than `MpqTable.MaxSize` or ignore the configured `HashTableSize` (fixes #14).

## v5.2.0
### Changes
- Implemented `MpqArchiveExtensions.VerifySignature`, including an overload that verifies against Blizzard's known public keys automatically.
- Added support for signing MPQ archives on creation via `MpqArchiveCreateOptions.SignaturePrivateKey`.
- Added support for the `Unk0x04` attributes flag in `AttributesFlags`/`Attributes`, including verification support in `VerifyAttributes`.
- Doubled `MpqTable.MaxSize`.

## v5.1.3
### Changes
- Removed obsolete `FileProvider.FileExists`, `FileProvider.GetFile`, and `FileProvider.EnumerateFiles` methods.
- `MpqArchiveBuilder.SaveTo(string, ...)` overloads now use `FileProvider.CreateFileAndFolder` instead of `File.Create`, so parent folders are created automatically.
### Bugfixes
- Minor nullable-reference and encoding-usage cleanups in `MpqStream`.

## v5.1.2
### Changes
- `MpqArchiveBuilder` is no longer sealed.

## v5.1.1
### Changes
- Minor fixes across `BlockTable`, `HashTable`, `MpqArchive`, `MpqArchiveBuilder`, and `MpqArchiveCreateOptions`.

## v5.1.0
### Changes
- `MpqArchiveBuilder` refactored: constructors and `AddFile`/`RemoveFile`/`SaveTo` methods reworked around the new `MpqArchiveCreateOptions`-based creation API.
### Bugfixes
- Fixed inconsistent casing across several public members.
- Fixed encryption seed calculation.
- Various other minor MPQ fixes (`FileProvider`, `MpqEntry`, `MpqStream`, `StormBuffer`).

## v5.0.0
### Breaking changes
- `Attributes` changed from a static extension-method holder to an instantiable sealed class with `Unk`, `Flags`, `Crc32s`, and `DateTimes` properties.
- Added `AttributesFlags` enum.
- `MpqArchive` creation now takes an `MpqArchiveCreateOptions` object instead of separate `hashTableSize`/`blockSize`/`writeArchiveFirst` constructor parameters; removed the `DefaultBlockSize` constant.
- `MpqFile.Name` no longer throws when unset.
### Changes
- `MpqFile` now implements `IComparable`, `IComparable<MpqFile>`, and `IEquatable<MpqFile>`, backed by new `MpqFileComparer`, replacing the old `IsSameAs` method.
- `MpqArchiveBuilder.AddFile` gained an overload accepting explicit `MpqFileFlags`; new `SaveTo` overloads accepting `MpqArchiveCreateOptions`.

## v1.2.2
### Changes
- `MpqFile.MpqStream` property is now public.
- Added `leaveOpen` parameter to the `MpqArchive` constructor, `MpqArchive.Create`, and `MpqArchiveBuilder.SaveTo`, to control whether the underlying stream is disposed.

## v1.2.1
### Changes
- Added `MpqArchiveBuilder.RemoveFile(ulong)` overload, alongside the existing `RemoveFile(string)`.

## v1.2.0
### Changes
- Added `MpqArchiveBuilder` class, for incrementally building or modifying an `MpqArchive` (supports adding/removing files and saving to a new archive).
- `MpqArchive.HashTableSize` property is now public.

## v1.1.0
### Changes
- Moved `FileProvider` class into the `War3Net.IO.Mpq` namespace.
- Added `Attributes` class with `VerifyAttributes` extension method, to verify the MPQ "(attributes)" file.
- Added file-existence and open methods to `MpqFile`.
### Bugfixes
- Fixed `MpqFile.OpenRead` to properly close the underlying `MpqStream` after copying its contents.
### Breaking changes
- `FileProvider.OpenNewWrite` has been renamed to `CreateFileAndFolder`.

## v1.0.1
### Bugfixes
- Various MPQ library bugfixes in `MpqArchive`, `MpqHeader`, and `StormBuffer`.

## v1.0.0
### Changes
- Added `MpqOrphanedFile` class to represent entries in the hash table with no corresponding filename.
- Added `TryGetHashString` method.
### Bugfixes
- Fixed "Unable to read beyond the end of the stream" exception in `BlockTable`.
- Fixed "Unable to determine encryption seed" exception.
- Fixed "Invalid enum" exception in `MpqArchive`.
- Fixed `MpqParserException` when parsing Wurst maps.
- Fixed exception when listfile contains garbage data.
- `MpqEntry.Filename` is now correctly null if no `MpqHash` references the entry.
- Orphaned `MpqEntries` no longer use deleted `MpqHashes`.
- Improved `MpqHeader` exception messages.
