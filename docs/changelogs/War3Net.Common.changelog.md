# War3Net.Common Changelog

All notable changes to the `War3Net.Common` package, newest version first.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added .NET 10.0 target framework; the package now targets both .NET 6.0 and .NET 10.0.

## [v6.0.2] - 2026-03-01

### Breaking Changes

- Updated target framework from .NET 5.0 to .NET 6.0.

## [v6.0.1] - 2026-02-01

_No functional changes; package readme and metadata updated for nuget.org presentation._

## [v6.0.0] - 2026-01-25

### Added

- Added `FromByteRaw`, `FromSByteRaw`, `FromInt16Raw`, `FromUInt16Raw`, `FromInt32Raw`, `FromUInt32Raw`, `FromInt64Raw`, and `FromUInt64Raw` methods to `EnumConvert<TEnum>`, which skip the validation performed by their non-`Raw` counterparts.
- Added `JsonElementExtensions.GetByteRaw<TEnum>` and `JsonElementExtensions.GetInt32Raw<TEnum>`, both with and without a `propertyName` parameter.

## [v5.8.0] - 2025-09-06

_No functional changes; version bumped to align with the rest of War3Net's v5.8.0 release._

## v5.6.1 - 2023-01-07

### Added

- Added `EnumerateObject`, `GetBoolean`, and `GetUInt32` extension methods to `JsonElementExtensions`.

## v5.6.0 - 2022-12-20

### Added

- Added `JsonElementExtensions` class, with `GetByte<TEnum>` and `GetInt32<TEnum>` methods that read an enum from either a JSON number or string, and `propertyName` overloads for `EnumerateArray`, `GetArrayLength`, `GetByte`, `GetInt32`, `GetSingle`, and `GetString`.

## v5.5.5 - 2022-11-13

### Breaking Changes

- Removed `EnumConvert<TEnum>.FromChar` and `BinaryReaderExtensions.ReadChar<TEnum>`.

### Added

- Added `EnumConvert<TEnum>.FromInt64` and `EnumConvert<TEnum>.FromUInt64`.

### Changed

- `EnumConvert<TEnum>` now uses `Unsafe.As` instead of boxing, and throws a descriptive `InvalidOperationException` instead of an `InvalidCastException` when the underlying type of `TEnum` does not match the method that was called (e.g. `FromInt32` on an enum with `byte` as its underlying type).

### Fixed

- `EnumExtensions.IsDefined` no longer throws an `InvalidCastException` for enums that don't use `int` as their underlying type.
- Fixed the binary representation of the value in the exception message thrown by `EnumConvert<TEnum>.FromSByte`, `FromUInt16`, and `FromUInt32` for flags enums.

## v5.5.3 - 2022-10-29

### Fixed

- Fixed `EnumConvert<TEnum>.FromChar` throwing an `InvalidCastException`.

## v5.5.2 - 2022-10-25

### Breaking Changes

- `DictionaryExtensions.SetValue` now extends `IDictionary<TKey, TValue>` instead of `Dictionary<TKey, TValue>`.

### Added

- Added `EnumConvert<TEnum>` class, with `FromByte`, `FromSByte`, `FromInt16`, `FromUInt16`, `FromInt32`, `FromUInt32`, and `FromChar` methods that convert a primitive value to an enum, validating that the result is defined for `TEnum`.
- Added `UTF8EncodingProvider` class, which provides `UTF8` and `StrictUTF8` encodings that do not use a byte order mark.
- Added `StreamExtensions.Copy`, which reads up to a maximum amount of bytes into a new `byte[]`, and a `StreamExtensions.CopyTo` overload that copies into an existing `byte[]`.

### Changed

- The generic `BinaryReaderExtensions` methods (`ReadByte<TEnum>`, `ReadChar<TEnum>`, and `ReadInt32<TEnum>`) now use `EnumConvert<TEnum>`, and throw an `ArgumentException` instead of an `InvalidDataException` for undefined values.
- Updated `System.Drawing.Common` from v5.0.0 to v6.0.0.

## v5.5.0 - 2022-08-20

_No functional changes; version bumped to align with the rest of War3Net's v5.5.0 release._

## v5.4.5 - 2022-05-27

### Changed

- Undefined values of flags enums are now displayed as a binary literal (e.g. `0b00000000000000000000000000000101`) in the exception message thrown by the generic `BinaryReaderExtensions` methods.

## v5.4.0 - 2022-02-13

### Added

- Added `BinaryReaderExtensions.ReadColorBgra` and `ColorExtensions.ToBgra`.

## v5.0.2 - 2021-02-14

### Added

- Added `Int32Extensions.ToBool`.

### Changed

- `BinaryReaderExtensions.ReadBool` now throws an `ArgumentException` instead of an `InvalidDataException` when the value read is neither 0 nor 1.
- `DictionaryExtensions.SetValue` now has a `notnull` constraint on `TKey`.

## v5.0.1 - 2020-12-25

### Added

- Added `EnumExtensions` class with an `IsDefined<TEnum>` method, which (unlike `Enum.IsDefined`) also accepts combinations of defined flags for enums with `FlagsAttribute`, with an optional `allowNoFlags` parameter to control whether zero is considered valid.

### Changed

- The generic `BinaryReaderExtensions` methods now use `IsDefined<TEnum>`, and no longer throw a `NotSupportedException` for enums whose name ends with "Version"; an `InvalidDataException` is thrown instead.

## v5.0.0 - 2020-12-14

### Breaking Changes

- Updated target framework from .NET Core 3.1 to .NET 5.0.

### Added

- Added `BinaryReaderExtensions.ReadInt24`, `ReadUInt24`, and `ReadByte<TEnum>`.
- Added `BinaryWriterExtensions.WriteInt24` and `WriteUInt24`.
- Added `ColorExtensions` class with a `ToRgba` method.
- Added `Int32Extensions.ToRgbaColor`.

### Changed

- Updated `System.Drawing.Common` from v4.5.1 to v5.0.0.

## v0.3.2 - 2020-11-11

_No functional changes._

## v0.3.1 - 2020-10-29

### Added

- Added optional `endWithNullChar` parameter to `BinaryWriterExtensions.WriteString`, which should be set to `false` when writing a length-prefixed string.

### Fixed

- `WriteString` no longer throws an `ArgumentException` when the string ends with more than one null character.

## v0.3.0 - 2020-10-27

### Breaking Changes

- Updated target framework from .NET Standard 2.1 to .NET Core 3.1.

### Added

- Added `BinaryReaderExtensions.ReadBool` and `BinaryWriterExtensions.WriteBool`, for reading and writing four-byte booleans.
- Added `BinaryReaderExtensions.ReadInt32<TEnum>` and `ReadChar<TEnum>`, which validate that the value read is defined for `TEnum`. Values that are a combination of defined flags are accepted for enums with `FlagsAttribute`, and a `NotSupportedException` is thrown for enums whose name ends with "Version".

## v0.2.0 - 2020-09-14

### Breaking Changes

- Updated target frameworks from .NET Framework 4.5/.NET Standard 1.3/.NET Standard 2.0 to .NET Standard 2.1.

### Changed

- `BinaryReaderExtensions.ReadColorRgba` and the `System.Drawing.Common` dependency are no longer conditional on the target framework.

## v0.1.3 - 2020-06-21

### Added

- Added `BinaryReaderExtensions.ReadString`, which reads a fixed amount of characters and trims trailing null characters.
- Added `BinaryWriterExtensions.WriteString` overload that writes a string padded with null characters to a fixed length, and throws an `ArgumentOutOfRangeException` if the string is longer than that length.

### Changed

- `ReadChars` now throws an `InvalidDataException` when the end of the stream is reached without encountering a null character, and an `ArgumentNullException` when the reader is `null`.
- `WriteString` now throws an `ArgumentException` when the string contains a null character that is not the last character, or an invalid or incomplete surrogate pair, and an `ArgumentNullException` when the writer is `null`.
- The `s` parameter of `WriteString` is now nullable.

### Fixed

- `BinaryReaderExtensions.ReadChars` now decodes the bytes as UTF-8 instead of reading `char` values one by one, fixing incorrect results for strings containing surrogate characters.
- `BinaryWriterExtensions.WriteString` now writes surrogate pairs correctly.

## v0.1.2 - 2020-04-25

### Added

- Added `DictionaryExtensions` class with a `SetValue` method, which adds the key/value pair, or overwrites the value if the key is already present.

## v0.1.1 - 2020-04-12

### Added

- Added `Int32Extensions` class with a `ToRawcode` method, and `StringExtensions` class with a `FromRawcode` method, for converting between four-character rawcodes and 32-bit integers.

## v0.1.0 - 2019-12-26

### Added

- Initial release, with `BinaryReaderExtensions` (`ReadChars`, `ReadColorRgba`), `BinaryWriterExtensions` (`WriteString`), and `StreamExtensions` (`CopyTo`, `ReadWord`, `ReadWordAsInt`).

[Unreleased]: https://github.com/Drake53/War3Net/compare/v6.0.3...HEAD
[v6.0.2]: https://github.com/Drake53/War3Net/releases/tag/v2026.3.1
[v6.0.1]: https://github.com/Drake53/War3Net/releases/tag/v2026.2.1
[v6.0.0]: https://github.com/Drake53/War3Net/releases/tag/v2026.1.25
[v5.8.0]: https://github.com/Drake53/War3Net/releases/tag/v2025.9.6
