namespace War3Net.IO.Mpq
{
    /// <summary>
    /// A Stream based class for reading a file from an <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqStream : Stream
    {
        /// <summary>
        /// Files smaller than this are compressed one sector at a time, because the overhead of parallelizing them outweighs the gain.
        /// </summary>
        private const int ParallelCompressionThreshold = 1024 * 1024;

        private readonly Stream _stream;
        private readonly int _blockSize;
        private readonly bool _canRead;
        private readonly uint[] _blockPositions;
        private readonly bool _isStreamOwner;

        // MpqEntry data
        private readonly uint _filePosition;
        private readonly uint _streamOffset;
        private readonly uint _fileSize;
        private readonly uint _compressedSize;
        private readonly MpqFileFlags _flags;
        private readonly bool _isCompressed;
        private readonly bool _isEncrypted;
        private readonly bool _isSingleUnit;
        private readonly uint _encryptionSeed;
        private readonly uint _baseEncryptionSeed;

        private byte[]? _currentData;
        private long _position;
        private int _currentBlockIndex;

        internal MpqStream(
            Stream baseStream,
            int blockSize,
            uint filePosition,
            uint streamOffset,
            uint fileSize,
            uint compressedSize,
            MpqFileFlags flags,
            uint encryptionSeed,
            uint baseEncryptionSeed,
            uint[] blockPositions,
            bool canRead,
            bool leaveOpen)
        {
            _canRead = canRead;
            _isStreamOwner = !leaveOpen;

            _filePosition = filePosition;
            _streamOffset = streamOffset;
            _fileSize = fileSize;
            _compressedSize = compressedSize;
            _flags = flags;
            _isCompressed = (_flags & MpqFileFlags.Compressed) != 0;
            _isEncrypted = _flags.HasFlag(MpqFileFlags.Encrypted);
            _isSingleUnit = _flags.HasFlag(MpqFileFlags.SingleUnit);

            _encryptionSeed = encryptionSeed;
            _baseEncryptionSeed = baseEncryptionSeed;

            _stream = baseStream;
            _blockSize = blockSize;

            _blockPositions = blockPositions;
            _currentBlockIndex = -1;
        }

        /// <summary>
        /// Re-encodes the stream using the given parameters.
        /// </summary>
        internal Stream Transform(MpqFileFlags targetFlags, MpqCompressionType compressionType, uint targetFilePosition, int targetBlockSize)
        {
            var compressedStream = Compress(targetFlags, compressionType, targetBlockSize, out var fileSize);
            return Encrypt(compressedStream, fileSize, targetFlags, _baseEncryptionSeed, targetFilePosition, targetBlockSize);
        }

        /// <summary>
        /// Performs the part of <see cref="Transform"/> that does not depend on the file's position in the archive.
        /// </summary>
        /// <remarks>
        /// The result must be passed to <see cref="Encrypt"/>, which takes ownership of it.
        /// </remarks>
        /// <param name="maxDegreeOfParallelism">
        /// The maximum number of sectors that are compressed concurrently. Only used for large files that are not <see cref="MpqFileFlags.SingleUnit"/>.
        /// The output does not depend on this value.
        /// </param>
        internal Stream Compress(MpqFileFlags targetFlags, MpqCompressionType compressionType, int targetBlockSize, out uint fileSize, int maxDegreeOfParallelism = 1)
        {
            var memoryStream = new MemoryStream();
            CopyTo(memoryStream);
            memoryStream.Position = 0;
            fileSize = (uint)memoryStream.Length;

            var compressedStream = GetCompressedStream(memoryStream, targetFlags, compressionType, targetBlockSize, maxDegreeOfParallelism);
            if (!ReferenceEquals(compressedStream, memoryStream))
            {
                memoryStream.Dispose();
            }

            compressedStream.Position = 0;
            return compressedStream;
        }

        /// <summary>
        /// Performs the part of <see cref="Transform"/> that depends on the file's position in the archive, which is only needed when the
        /// <paramref name="targetFlags"/> contain <see cref="MpqFileFlags.Encrypted"/>. Takes ownership of <paramref name="compressedStream"/>.
        /// </summary>
        internal static Stream Encrypt(Stream compressedStream, uint fileSize, MpqFileFlags targetFlags, uint baseEncryptionSeed, uint targetFilePosition, int targetBlockSize)
        {
            var blockPosCount = (uint)(((int)fileSize + targetBlockSize - 1) / targetBlockSize) + 1;
            if (!targetFlags.HasFlag(MpqFileFlags.Encrypted) || blockPosCount <= 1)
            {
                compressedStream.Position = 0;
                return compressedStream;
            }

            using var ownedCompressedStream = compressedStream;
            var compressedSize = (uint)compressedStream.Length;

            var blockPositions = new int[blockPosCount];
            var singleUnit = targetFlags.HasFlag(MpqFileFlags.SingleUnit);

            var hasBlockPositions = !singleUnit && ((targetFlags & MpqFileFlags.Compressed) != 0);
            if (hasBlockPositions)
            {
                using (var reader = new BinaryReader(compressedStream, Encoding.UTF8, true))
                {
                    for (var i = 0; i < blockPosCount; i++)
                    {
                        blockPositions[i] = (int)reader.ReadUInt32();
                    }
                }

                compressedStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                if (singleUnit)
                {
                    blockPosCount = 2;
                }

                blockPositions[0] = 0;
                for (var blockIndex = 2; blockIndex < blockPosCount; blockIndex++)
                {
                    blockPositions[blockIndex - 1] = targetBlockSize * (blockIndex - 1);
                }

                blockPositions[blockPosCount - 1] = (int)compressedSize;
            }

            var encryptionSeed = baseEncryptionSeed;
            if (targetFlags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey))
            {
                encryptionSeed = MpqEncryptionUtils.AdjustEncryptionSeed(encryptionSeed, targetFilePosition, fileSize);
            }

            var resultStream = new MemoryStream();
            var currentOffset = 0;
            using (var writer = new BinaryWriter(resultStream, UTF8EncodingProvider.StrictUTF8, true))
            {
                for (var blockIndex = hasBlockPositions ? 0 : 1; blockIndex < blockPosCount; blockIndex++)
                {
                    var toWrite = blockPositions[blockIndex] - currentOffset;

                    var data = StormBuffer.EncryptStream(compressedStream, (uint)(encryptionSeed + blockIndex - 1), currentOffset, toWrite);
                    writer.Write(data);

                    currentOffset += toWrite;
                }
            }

            resultStream.Position = 0;
            return resultStream;
        }

        private Stream GetCompressedStream(MemoryStream baseStream, MpqFileFlags targetFlags, MpqCompressionType compressionType, int targetBlockSize, int maxDegreeOfParallelism)
        {
            if ((targetFlags & MpqFileFlags.Compressed) == 0)
            {
                return baseStream;
            }

            var resultStream = new MemoryStream();
            var singleUnit = targetFlags.HasFlag(MpqFileFlags.SingleUnit);

            void TryCompress(uint bytes)
            {
                var offset = baseStream.Position;
                var compressedStream = compressionType switch
                {
                    MpqCompressionType.ZLib => ZLibCompression.Compress(baseStream, (int)bytes, true),

                    _ => throw new NotSupportedException($"Compression type '{compressionType}' is not supported. Currently only ZLib is supported."),
                };

                // Add one because CompressionType byte not written yet.
                var length = compressedStream.Length + 1;
                if (!singleUnit && length >= bytes)
                {
                    baseStream.CopyTo(resultStream, offset, (int)bytes, StreamExtensions.DefaultBufferSize);
                }
                else
                {
                    resultStream.WriteByte((byte)compressionType);
                    compressedStream.Position = 0;
                    compressedStream.CopyTo(resultStream);
                }

                compressedStream.Dispose();
            }

            var length = (uint)baseStream.Length;

            if (singleUnit)
            {
                TryCompress(length);
            }
            else
            {
                var blockCount = (uint)((length + targetBlockSize - 1) / targetBlockSize) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                resultStream.Position = blockOffsets[0];

                if (maxDegreeOfParallelism > 1 && length >= ParallelCompressionThreshold && baseStream.TryGetBuffer(out var buffer))
                {
                    var compressedBlocks = CompressBlocksParallel(buffer, compressionType, targetBlockSize, (int)blockCount - 1, maxDegreeOfParallelism);
                    for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                    {
                        var compressedBlock = compressedBlocks[blockIndex - 1];
                        if (compressedBlock is null)
                        {
                            // Compression did not reduce the size, so the sector is stored as-is.
                            var blockOffset = (blockIndex - 1) * targetBlockSize;
                            resultStream.Write(buffer.Array!, buffer.Offset + blockOffset, Math.Min(targetBlockSize, buffer.Count - blockOffset));
                        }
                        else
                        {
                            resultStream.WriteByte((byte)compressionType);
                            resultStream.Write(compressedBlock, 0, compressedBlock.Length);
                        }

                        blockOffsets[blockIndex] = (uint)resultStream.Position;
                    }
                }
                else
                {
                    for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                    {
                        var bytesToCompress = blockIndex + 1 == blockCount ? (uint)(baseStream.Length - baseStream.Position) : (uint)targetBlockSize;

                        TryCompress(bytesToCompress);
                        blockOffsets[blockIndex] = (uint)resultStream.Position;
                    }
                }

                resultStream.Position = 0;
                using (var writer = new BinaryWriter(resultStream, UTF8EncodingProvider.StrictUTF8, true))
                {
                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        writer.Write(blockOffsets[blockIndex]);
                    }
                }
            }

            resultStream.Position = 0;
            return resultStream;
        }

        /// <summary>
        /// Compresses each sector of <paramref name="buffer"/> independently and concurrently.
        /// </summary>
        /// <returns>
        /// For each sector, the compressed data (without the compression type byte), or <see langword="null"/> if compression does not reduce its size.
        /// The decision is the same as the one made in <see cref="GetCompressedStream"/>, so the output is identical to sequential compression.
        /// </returns>
        private static byte[]?[] CompressBlocksParallel(ArraySegment<byte> buffer, MpqCompressionType compressionType, int targetBlockSize, int sectorCount, int maxDegreeOfParallelism)
        {
            if (compressionType != MpqCompressionType.ZLib)
            {
                throw new NotSupportedException($"Compression type '{compressionType}' is not supported. Currently only ZLib is supported.");
            }

            var result = new byte[]?[sectorCount];
            var parallelOptions = new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

            System.Threading.Tasks.Parallel.For(0, sectorCount, parallelOptions, sectorIndex =>
            {
                var sectorOffset = sectorIndex * targetBlockSize;
                var bytes = Math.Min(targetBlockSize, buffer.Count - sectorOffset);

                using var sectorStream = new MemoryStream(buffer.Array!, buffer.Offset + sectorOffset, bytes, false);
                using var compressedStream = (MemoryStream)ZLibCompression.Compress(sectorStream, bytes, true);

                // Add one because CompressionType byte not written yet.
                if (compressedStream.Length + 1 < bytes)
                {
                    result[sectorIndex] = compressedStream.ToArray();
                }
            });

            return result;
        }

        public MpqFileFlags Flags => _flags;

        public bool IsCompressed => _isCompressed;

        public bool IsEncrypted => _isEncrypted;

        [Obsolete("Use CanRead instead.")]
        public bool CanBeDecrypted => !_isEncrypted || _fileSize < 4 || _encryptionSeed != 0;

        public uint CompressedSize => _compressedSize;

        public uint FileSize => _fileSize;

        public uint FilePosition => _filePosition;

        public int BlockSize => _blockSize;

        /// <inheritdoc/>
        public override bool CanRead => _canRead;

        /// <inheritdoc/>
        public override bool CanSeek => _canRead;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => _fileSize;

        /// <inheritdoc/>
        public override long Position
        {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        internal uint BaseEncryptionSeed => _baseEncryptionSeed;

        /// <inheritdoc/>
        public override void Flush()
        {
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek)
            {
                throw new NotSupportedException();
            }

            var target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,

                _ => throw new InvalidEnumArgumentException(nameof(origin), (int)origin, typeof(SeekOrigin)),
            };

            if (target < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attempted to Seek before the beginning of the stream");
            }

            if (target > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attempted to Seek beyond the end of the stream");
            }

            return _position = target;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("SetLength is not supported");
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }

            if (_isSingleUnit)
            {
                return ReadInternal(buffer, offset, count);
            }

            var toread = count;
            var readtotal = 0;

            while (toread > 0)
            {
                var read = ReadInternal(buffer, offset, toread);
                if (read == 0)
                {
                    break;
                }

                readtotal += read;
                offset += read;
                toread -= read;
            }

            return readtotal;
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }

            if (_position >= Length)
            {
                return -1;
            }

            BufferData();
            return _currentData[_isSingleUnit ? _position++ : (int)(_position++ & (_blockSize - 1))];
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Write is not supported");
        }

        /// <inheritdoc/>
        public override void Close()
        {
            base.Close();
            if (_isStreamOwner)
            {
                _stream.Close();
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && _isStreamOwner)
            {
                _stream.Dispose();
            }
        }

        /// <summary>
        /// Copy the base stream, so that the contents do not get decompressed nor decrypted.
        /// </summary>
        internal void CopyBaseStreamTo(Stream target)
        {
            lock (_stream)
            {
                _stream.CopyTo(target, _streamOffset, (int)_compressedSize, StreamExtensions.DefaultBufferSize);
            }
        }

        private int ReadInternal(byte[] buffer, int offset, int count)
        {
            // OW: avoid reading past the contents of the file
            if (_position >= Length)
            {
                return 0;
            }

            BufferData();

            var localPosition = _isSingleUnit ? _position : (_position & (_blockSize - 1));
            var canRead = (int)(_currentData.Length - localPosition);
            var bytesToCopy = canRead > count ? count : canRead;
            if (bytesToCopy <= 0)
            {
                return 0;
            }

            Array.Copy(_currentData, localPosition, buffer, offset, bytesToCopy);

            _position += bytesToCopy;
            return bytesToCopy;
        }

        [MemberNotNull(nameof(_currentData))]
        private void BufferData()
        {
            if (!_isSingleUnit)
            {
                var requiredBlock = (int)(_position / _blockSize);
                if (requiredBlock != _currentBlockIndex || _currentData is null)
                {
                    var expectedLength = Math.Min((int)(Length - (requiredBlock * _blockSize)), _blockSize);
                    _currentData = LoadBlock(requiredBlock, expectedLength);
                    _currentBlockIndex = requiredBlock;
                }
            }
            else if (_currentData is null)
            {
                _currentData = LoadSingleUnit();
            }
        }

        private byte[] LoadSingleUnit()
        {
            // Read the entire file into memory
            var fileData = new byte[_compressedSize];
            lock (_stream)
            {
                _stream.Seek(_streamOffset, SeekOrigin.Begin);
                _stream.CopyTo(fileData, 0, fileData.Length);
            }

            if (_isEncrypted && fileData.Length > 3)
            {
                if (_encryptionSeed == 0)
                {
                    throw new MpqParserException("Unable to determine encryption key");
                }

                StormBuffer.DecryptBlock(fileData, _encryptionSeed);
            }

            return _flags.HasFlag(MpqFileFlags.CompressedMulti) && _compressedSize > 0
                ? MpqStreamUtils.DecompressMulti(fileData, _fileSize)
                : fileData;
        }

        private byte[] LoadBlock(int blockIndex, int expectedLength)
        {
            long offset;
            int bufferSize;

            if (_isCompressed)
            {
                offset = _blockPositions[blockIndex];
                bufferSize = (int)(_blockPositions[blockIndex + 1] - offset);
            }
            else
            {
                offset = (uint)(blockIndex * _blockSize);
                bufferSize = expectedLength;
            }

            offset += _streamOffset;

            var buffer = new byte[bufferSize];
            lock (_stream)
            {
                _stream.Seek(offset, SeekOrigin.Begin);
                _stream.CopyTo(buffer, 0, bufferSize);
            }

            if (_isEncrypted && bufferSize > 3)
            {
                if (_encryptionSeed == 0)
                {
                    throw new MpqParserException("Unable to determine encryption key");
                }

                StormBuffer.DecryptBlock(buffer, (uint)(blockIndex + _encryptionSeed));
            }

            if (_isCompressed && (bufferSize != expectedLength))
            {
                buffer = _flags.HasFlag(MpqFileFlags.CompressedPK)
                    ? PKLibCompression.Decompress(buffer, (uint)expectedLength)
                    : MpqStreamUtils.DecompressMulti(buffer, (uint)expectedLength);
            }

            return buffer;
        }
    }
}