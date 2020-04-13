// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public abstract class MpqFile : IDisposable
    {
        private readonly ulong _name;
        private readonly MpqStream _mpqStream;
        private readonly bool _isStreamOwner;

        private MpqFileFlags _flags;
        private MpqLocale _locale;
        private MpqCompressionType _compressionType;

        internal MpqFile(ulong hashedName, MpqStream mpqStream, MpqFileFlags flags, MpqLocale locale, bool leaveOpen)
        {
            _name = hashedName;
            _mpqStream = mpqStream ?? throw new ArgumentNullException(nameof(mpqStream));
            _isStreamOwner = !leaveOpen;

            _flags = flags;
            _locale = locale;
            _compressionType = MpqCompressionType.ZLib;
        }

        public ulong Name => _name;

        internal MpqStream MpqStream => _mpqStream;

        public MpqFileFlags TargetFlags
        {
            get => _flags;
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

                _flags = value;
            }
        }

        public MpqLocale Locale
        {
            get => _locale;
            set
            {
                if (!Enum.IsDefined(typeof(MpqLocale), value))
                {
                    throw new ArgumentException("Invalid enum.", nameof(value));
                }

                _locale = value;
            }
        }

        public MpqCompressionType CompressionType
        {
            get => _compressionType;
            set
            {
                if (!Enum.IsDefined(typeof(MpqCompressionType), value))
                {
                    throw new ArgumentException("Invalid enum.", nameof(value));
                }

                _compressionType = value;
            }
        }

        internal bool IsFilePositionFixed => !_mpqStream.CanBeDecrypted && _mpqStream.Flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey);

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

        public static MpqFile New(Stream? stream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, uint? encryptionSeed = null)
        {
            var mpqStream = stream as MpqStream ?? new MpqStream(stream ?? new MemoryStream(), null);
            return new MpqUnknownFile(mpqStream, mpqStream.Flags, mpqHash, hashIndex, hashCollisions, encryptionSeed);
        }

        public static MpqFile New(Stream? stream, string fileName, bool leaveOpen = false)
        {
            var mpqStream = stream as MpqStream ?? new MpqStream(stream ?? new MemoryStream(), fileName, leaveOpen);
            return new MpqKnownFile(fileName, mpqStream, mpqStream.Flags, MpqLocale.Neutral, leaveOpen);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isStreamOwner)
            {
                _mpqStream.Dispose();
            }
        }

        public bool IsSameAs(MpqFile other)
        {
            return _name == other._name && _locale == other._locale;
        }

        internal void AddToArchive(MpqArchive mpqArchive, uint index, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            var headerOffset = mpqArchive.HeaderOffset;
            var absoluteFileOffset = (uint)mpqArchive.BaseStream.Position;
            var relativeFileOffset = absoluteFileOffset - headerOffset;

            var mustChangePosition = _flags.HasFlag(MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey) && _mpqStream.FilePosition != relativeFileOffset;
            if (_flags == _mpqStream.Flags && mpqArchive.BlockSize == _mpqStream.BlockSize && !mustChangePosition)
            {
                _mpqStream.CopyBaseStreamTo(mpqArchive.BaseStream);
                GetTableEntries(mpqArchive, index, relativeFileOffset, _mpqStream.CompressedSize, _mpqStream.FileSize, out mpqEntry, out mpqHash);
            }
            else
            {
                if (IsFilePositionFixed)
                {
                    throw new Exception();
                }

                using var newStream = _mpqStream.Transform(_flags, _compressionType, relativeFileOffset, mpqArchive.BlockSize);
                newStream.CopyTo(mpqArchive.BaseStream);
                GetTableEntries(mpqArchive, index, relativeFileOffset, (uint)newStream.Length, _mpqStream.FileSize, out mpqEntry, out mpqHash);
            }
        }

        protected abstract void GetTableEntries(MpqArchive mpqArchive, uint index, uint relativeFileOffset, uint compressedSize, uint fileSize, out MpqEntry mpqEntry, out MpqHash mpqHash);
    }
}