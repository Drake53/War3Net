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

        private Stream? _preparedStream;
        private uint _preparedFileSize;
        private uint? _preparedCrc32;

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
        /// Gets the position in the <see cref="HashTable"/>.
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
            var mpqStream = stream as MpqStream ?? MpqStreamFactory.FromStream(stream ?? new MemoryStream(), fileName, leaveOpen);
            return new MpqKnownFile(fileName, mpqStream, mpqStream.Flags, locale, leaveOpen);
        }

        public static MpqFile New(Stream? stream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, uint? encryptionSeed = null)
        {
            var mpqStream = stream as MpqStream ?? MpqStreamFactory.FromStream(stream ?? new MemoryStream(), null);
            return new MpqUnknownFile(mpqStream, mpqStream.Flags, mpqHash, hashIndex, hashCollisions, encryptionSeed);
        }

        public static MpqFile New(Stream? stream)
        {
            var mpqStream = stream as MpqStream ?? MpqStreamFactory.FromStream(stream ?? new MemoryStream(), null);
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
            _preparedStream?.Dispose();
            _preparedStream = null;

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

        /// <summary>
        /// Does the work for <see cref="AddToArchive"/> that does not depend on where the file ends up in the archive (compression and the CRC32 checksum).
        /// </summary>
        /// <remarks>
        /// This method may run concurrently for different <see cref="MpqFile"/>s; <see cref="MpqStream"/> locks its base stream when reading from it.
        /// </remarks>
        internal void PrepareForArchive(int blockSize, bool computeCrc32, int maxDegreeOfParallelism)
        {
            if (RequiresReencode(blockSize) && _mpqStream.CanRead)
            {
                _preparedStream = _mpqStream.Compress(_targetFlags, _compressionType, blockSize, out _preparedFileSize, maxDegreeOfParallelism);
            }

            if (computeCrc32)
            {
                _preparedCrc32 = ComputeCrc32();
            }
        }

        internal uint GetCrc32()
        {
            return _preparedCrc32 ?? ComputeCrc32();
        }

        internal void AddToArchive(MpqArchive mpqArchive, uint index, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            var headerOffset = mpqArchive.HeaderOffset;
            var absoluteFileOffset = (uint)mpqArchive.BaseStream.Position;
            var relativeFileOffset = absoluteFileOffset - headerOffset;

            var mustChangePosition = _targetFlags.IsOffsetEncrypted() && _mpqStream.FilePosition != absoluteFileOffset;
            if (!RequiresReencode(mpqArchive.BlockSize) && !mustChangePosition)
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

                var compressedStream = _preparedStream;
                var fileSize = _preparedFileSize;
                _preparedStream = null;
                compressedStream ??= _mpqStream.Compress(_targetFlags, _compressionType, mpqArchive.BlockSize, out fileSize);

                using var newStream = MpqStream.Encrypt(compressedStream, fileSize, _targetFlags, _mpqStream.BaseEncryptionSeed, relativeFileOffset, mpqArchive.BlockSize);
                newStream.CopyTo(mpqArchive.BaseStream);
                GetTableEntries(mpqArchive, index, relativeFileOffset, (uint)newStream.Length, _mpqStream.FileSize, out mpqEntry, out mpqHash);
            }
        }

        private bool RequiresReencode(int blockSize)
        {
            return _targetFlags != _mpqStream.Flags || blockSize != _mpqStream.BlockSize;
        }

        private uint ComputeCrc32()
        {
            if (!_mpqStream.CanSeek || !_mpqStream.CanRead)
            {
                return 0;
            }

            // Restore the position, in case the stream still needs to be compressed afterwards.
            var position = _mpqStream.Position;
            _mpqStream.Position = 0;
            var crc32 = Crc32Checksum.Compute(_mpqStream);
            _mpqStream.Position = position;
            return crc32;
        }

        protected abstract void GetTableEntries(MpqArchive mpqArchive, uint index, uint relativeFileOffset, uint compressedSize, uint fileSize, out MpqEntry mpqEntry, out MpqHash mpqHash);
    }
}