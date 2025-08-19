// ------------------------------------------------------------------------------
// <copyright file="CascStream.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Index;
using War3Net.IO.Casc.Storage;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Stream implementation for reading CASC files.
    /// </summary>
    public class CascStream : Stream
    {
        private readonly CascStorageContext _context;
        private readonly EKeyEntry _indexEntry;
        private readonly Stream _dataStream;
        private readonly bool _ownsStream;
        private byte[]? _decodedData;
        private MemoryStream? _memoryStream;
        private long _position;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStream"/> class.
        /// </summary>
        /// <param name="context">The storage context.</param>
        /// <param name="indexEntry">The index entry for the file.</param>
        internal CascStream(CascStorageContext context, EKeyEntry indexEntry)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _indexEntry = indexEntry ?? throw new ArgumentNullException(nameof(indexEntry));

            // Open the data file
            var dataFilePath = IndexManager.GetDataFilePath(indexEntry, _context.DataPath!);
            _dataStream = File.OpenRead(dataFilePath);
            _ownsStream = true;

            // Seek to the file offset
            _dataStream.Seek((long)indexEntry.DataFileOffset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStream"/> class with a provided stream.
        /// </summary>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="encodedSize">The encoded size of the data.</param>
        internal CascStream(Stream dataStream, uint encodedSize)
        {
            _dataStream = dataStream ?? throw new ArgumentNullException(nameof(dataStream));
            _ownsStream = false;
            _indexEntry = new EKeyEntry { EncodedSize = encodedSize };
            _context = new CascStorageContext();
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => true;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                EnsureDecoded();
                return _memoryStream?.Length ?? 0;
            }
        }

        /// <inheritdoc/>
        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _position = value;
            }
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            // Read-only stream, nothing to flush
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (buffer.Length - offset < count)
            {
                throw new ArgumentException("Buffer too small");
            }

            EnsureDecoded();

            if (_memoryStream == null)
            {
                return 0;
            }

            // Clamp position to valid range
            _position = Math.Max(0, Math.Min(_position, _memoryStream.Length));

            _memoryStream.Position = _position;
            int bytesRead = _memoryStream.Read(buffer, offset, count);
            _position = _memoryStream.Position; // Keep positions synchronized
            return bytesRead;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            EnsureDecoded();

            long newPosition = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentException("Invalid seek origin", nameof(origin)),
            };

            if (newPosition < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            _position = newPosition;
            return _position;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("Stream is read-only");
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Stream is read-only");
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _memoryStream?.Dispose();
                    if (_ownsStream)
                    {
                        _dataStream?.Dispose();
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private void EnsureDecoded()
        {
            if (_decodedData != null)
            {
                return;
            }

            // Read encoded data
            var encodedData = new byte[_indexEntry.EncodedSize];
            int totalRead = 0;
            while (totalRead < encodedData.Length)
            {
                int read = _dataStream.Read(encodedData, totalRead, encodedData.Length - totalRead);
                if (read == 0)
                {
                    throw new EndOfStreamException("Unexpected end of data stream");
                }

                totalRead += read;
            }

            // Check if data is BLTE-encoded
            if (BlteDecoder.IsBlte(encodedData))
            {
                _decodedData = BlteDecoder.Decode(encodedData);
            }
            else
            {
                _decodedData = encodedData;
            }

            _memoryStream = new MemoryStream(_decodedData, false);
        }
    }
}