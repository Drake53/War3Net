// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;

using War3Net.Common.Extensions;
using War3Net.IO.Mpq.Extensions;

namespace War3Net.IO.Mpq
{
    public abstract class MpqFile : IDisposable, IComparable, IComparable<MpqFile>, IEquatable<MpqFile>
    {
        private readonly ulong? _name;
        private readonly MpqStream _mpqStream;
        private readonly bool _isStreamOwner;

        private MpqFileFlags _targetFlags;
        private MpqLocale _locale;
        private MpqCompressionType _compressionType;

        internal MpqFile(ulong? hashedName, MpqStream mpqStream, MpqFileFlags flags, MpqLocale locale, bool leaveOpen)
        {
            _name = hashedName;
            _mpqStream = mpqStream ?? throw new ArgumentNullException(nameof(mpqStream));
            _isStreamOwner = !leaveOpen;

            _targetFlags = flags;
            _locale = locale;
            _compressionType = MpqCompressionType.ZLib;
        }

        public ulong Name => _name.GetValueOrDefault(default(ulong));

        public MpqStream MpqStream => _mpqStream;

        public MpqFileFlags TargetFlags
        {
            get => _targetFlags;
            set
            {
                if ((value & MpqFileFlags.Garbage) != 0)
                {
                    throw new ArgumentException("Invalid enum.", nameof(value));
                }

                if (value.HasFlag(MpqFileFlags.Encrypted) && EncryptionSeed is null)
                {
                    throw new ArgumentException("Cannot set encrypted flag when there is no encryption seed.", nameof(value));
                }

                _targetFlags = value;
            }
        }

        public MpqLocale Locale
        {
            get => _locale;
            set
            {
                if (!value.IsDefined())
                {
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(MpqLocale));
                }

                _locale = value;
            }
        }

        public MpqCompressionType CompressionType
        {
            get => _compressionType;
            set
            {
                if (!value.IsKnownMpqCompressionType())
                {
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(MpqCompressionType));
                }

                _compressionType = value;
            }
        }

        internal bool IsFilePositionFixed => !_mpqStream.CanRead && _mpqStream.Flags.IsOffsetEncrypted();

        /// <summary>
        /// Position in the <see cref="HashTable"/>.
        /// </summary>
        internal abstract uint HashIndex { get; }

        /// <summary>
        /// Gets a value that, combined with <see cref="HashIndex"/>, represents the range of indices where the file may be placed.
        /// </summary>
        /// <remarks>
        /// This value is always zero for <see cref="MpqKnownFile"/>.
        /// For <see cref="MpqUnknownFile"/>, it depends on the <see cref="MpqHash"/>es preceding this file's hash in the <see cref="MpqArchive"/> from which the file was retrieved.
        /// </remarks>
        internal abstract uint HashCollisions { get; }

        /// <summary>
        /// Gets the base encryption seed used to encrypt this <see cref="MpqFile"/>'s stream.
        /// </summary>
        /// <remarks>
        /// If the <see cref="MpqFile"/> has the <see cref="MpqFileFlags.BlockOffsetAdjustedKey"/> flag, this seed must be adjusted based on the file's position and size.
        /// </remarks>
        protected abstract uint? EncryptionSeed { get; }

        public static MpqFile New(Stream? stream, string fileName, bool leaveOpen = false)
        {
            return New(stream, fileName, MpqLocale.Neutral, leaveOpen);
        }

        public static MpqFile New(Stream? stream, string fileName, MpqLocale locale, bool leaveOpen = false)
        {
            var mpqStream = stream as MpqStream ?? new MpqStream(stream ?? new MemoryStream(), fileName, leaveOpen);
            return new MpqKnownFile(fileName, mpqStream, mpqStream.Flags, locale, leaveOpen);
        }

        public static MpqFile New(Stream? stream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, uint? encryptionSeed = null)
        {
            var mpqStream = stream as MpqStream ?? new MpqStream(stream ?? new MemoryStream(), null);
            return new MpqUnknownFile(mpqStream, mpqStream.Flags, mpqHash, hashIndex, hashCollisions, encryptionSeed);
        }

        public static MpqFile New(Stream? stream)
        {
            var mpqStream = stream as MpqStream ?? new MpqStream(stream ?? new MemoryStream(), null);
            return new MpqOrphanedFile(mpqStream, mpqStream.Flags);
        }

        public static bool Exists(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            // Check if file is contained in an mpq archive.
            var subPath = path;
            var fullPath = new FileInfo(path).FullName;
            while (!File.Exists(subPath))
            {
                subPath = new FileInfo(subPath).DirectoryName;
                if (subPath is null)
                {
                    return false;
                }
            }

            var relativePath = fullPath.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var archive = MpqArchive.Open(subPath);
            return Exists(archive, relativePath);
        }

        public static bool Exists(MpqArchive archive, string path)
        {
            if (archive.FileExists(path))
            {
                return true;
            }

            // Check if file is contained in an mpq archive.
            var subPath = path;
            var ignoreLength = new FileInfo(subPath).FullName.Length - path.Length;
            while (!archive.FileExists(subPath))
            {
                var directoryName = new FileInfo(subPath).DirectoryName ?? string.Empty;
                if (directoryName.Length <= ignoreLength)
                {
                    return false;
                }

                subPath = directoryName.Substring(ignoreLength);
            }

            var relativePath = path.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var subArchiveStream = archive.OpenFile(subPath);
            using var subArchive = MpqArchive.Open(subArchiveStream);
            return Exists(subArchive, relativePath);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream OpenRead(string path)
        {
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            // Assume file is contained in an mpq archive.
            var subPath = path;
            var fullPath = new FileInfo(path).FullName;
            while (!File.Exists(subPath))
            {
                subPath = new FileInfo(subPath).DirectoryName;
                if (subPath is null)
                {
                    throw new FileNotFoundException($"File not found: {path}");
                }
            }

            var relativePath = fullPath.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var archive = MpqArchive.Open(subPath);
            return OpenRead(archive, relativePath);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream OpenRead(MpqArchive archive, string path)
        {
            static Stream GetArchiveFileStream(MpqArchive archive, string filePath)
            {
                using var mpqStream = archive.OpenFile(filePath);
                using var memoryStream = new MemoryStream((int)mpqStream.Length);

                mpqStream.CopyTo(memoryStream);

                return new MemoryStream(memoryStream.ToArray(), false);
            }

            if (archive.FileExists(path))
            {
                return GetArchiveFileStream(archive, path);
            }

            // Assume file is contained in an mpq archive.
            var subPath = path;
            var ignoreLength = new FileInfo(subPath).FullName.Length - path.Length;
            while (!archive.FileExists(subPath))
            {
                var directoryName = new FileInfo(subPath).DirectoryName ?? string.Empty;
                if (directoryName.Length <= ignoreLength)
                {
                    throw new FileNotFoundException($"File not found: {path}");
                }

                subPath = directoryName.Substring(ignoreLength);
            }

            var relativePath = path.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var subArchiveStream = archive.OpenFile(subPath);
            using var subArchive = MpqArchive.Open(subArchiveStream);
            return GetArchiveFileStream(subArchive, relativePath);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isStreamOwner)
            {
                _mpqStream.Dispose();
            }
        }

        public int CompareTo(object? value)
        {
            return MpqFileComparer.Default.Compare(this, value);
        }

        public int CompareTo(MpqFile? mpqFile)
        {
            return MpqFileComparer.Default.Compare(this, mpqFile);
        }

        public bool Equals(MpqFile? other)
        {
            return MpqFileComparer.Default.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _locale);
        }

        internal void AddToArchive(MpqArchive mpqArchive, uint index, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            var headerOffset = mpqArchive.HeaderOffset;
            var absoluteFileOffset = (uint)mpqArchive.BaseStream.Position;
            var relativeFileOffset = absoluteFileOffset - headerOffset;

            var mustChangePosition = _targetFlags.IsOffsetEncrypted() && _mpqStream.FilePosition != relativeFileOffset;
            if (_targetFlags == _mpqStream.Flags && mpqArchive.BlockSize == _mpqStream.BlockSize && !mustChangePosition)
            {
                _mpqStream.CopyBaseStreamTo(mpqArchive.BaseStream);
                GetTableEntries(mpqArchive, index, relativeFileOffset, _mpqStream.CompressedSize, _mpqStream.FileSize, out mpqEntry, out mpqHash);
            }
            else
            {
                if (!_mpqStream.CanRead)
                {
                    throw new InvalidOperationException("Unable to re-encode the mpq file, because its stream cannot be read.");
                }

                using var newStream = _mpqStream.Transform(_targetFlags, _compressionType, relativeFileOffset, mpqArchive.BlockSize);
                newStream.CopyTo(mpqArchive.BaseStream);
                GetTableEntries(mpqArchive, index, relativeFileOffset, (uint)newStream.Length, _mpqStream.FileSize, out mpqEntry, out mpqHash);
            }
        }

        protected abstract void GetTableEntries(MpqArchive mpqArchive, uint index, uint relativeFileOffset, uint compressedSize, uint fileSize, out MpqEntry mpqEntry, out MpqHash mpqHash);
    }
}