// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Foole.Mpq
{
    public class MpqFile : IEquatable<MpqFile>
    {
        private readonly Stream _baseStream;
        private readonly MpqEntry _entry;
        private readonly uint _hashCollisions; // possible amount of collisions this unknown file had in old archive
        private readonly int _blockSize; // used for compression
        private readonly string _fileName;

        private MemoryStream _compressedStream;
        private MpqHash? _hash;
        private uint _hashIndex; // position in hashtable

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="fileName"></param>
        /// <param name="flags"></param>
        /// <param name="blockSize"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> argument is null.</exception>
        public MpqFile(Stream sourceStream, string fileName, MpqFileFlags flags, ushort blockSize)
        {
            _baseStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            _fileName = fileName;

            _blockSize = 0x200 << blockSize;

            var fileSize = (uint)_baseStream.Length;
            var compressedSize = ( ( flags & MpqFileFlags.Compressed ) != 0 ) ? Compress() : fileSize;

            _entry = new MpqEntry(fileName, compressedSize, fileSize, flags);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class, for which the filename is unknown.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="mpqHash"></param>
        /// <param name="hashIndex"></param>
        /// <param name="hashCollisions"></param>
        /// <param name="flags"></param>
        /// <param name="blockSize"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> argument is null.</exception>
        public MpqFile(Stream sourceStream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, MpqFileFlags flags, ushort blockSize)
            : this(sourceStream, null, flags, blockSize)
        {
            if (mpqHash.Mask == 0)
            {
                throw new ArgumentException("Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash));
            }

            _hash = mpqHash;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
        }

        /// <summary>
        /// 
        /// </summary>
        public MpqEntry MpqEntry => _entry;

        /// <summary>
        /// 
        /// </summary>
        public MpqHash MpqHash => _hash.Value;

        /// <summary>
        /// 
        /// </summary>
        public uint HashIndex => _hashIndex;

        /// <summary>
        /// 
        /// </summary>
        public uint HashCollisions => _hashCollisions;

        /// <summary>
        /// 
        /// </summary>
        public Stream BaseStream => _baseStream;

        /// <summary>
        /// 
        /// </summary>
        public MemoryStream MemoryStream => _compressedStream;

        /// <summary>
        /// Gets the filename of this <see cref="MpqFile"/>.
        /// </summary>
        public string Name => _fileName;

        // public MpqArchive Archive => _archive;

        /*public void AddToArchive( MpqArchive archive )
        {
            if ( _archive != null )
            {
                throw new InvalidOperationException();
            }

            _archive = archive;
            _entry.SetPos( 0 );
        }*/

        public void AddToArchive(uint headerOffset, uint index, uint filePos, MpqLocale locale, uint mask)
        {
            _entry.SetPos(headerOffset, filePos);

            // This file came from another archive, and has an unknown filename.
            if (_hash.HasValue)
            {
                // Overwrite blockIndex from old archive.
                var hash = _hash.Value;
                _hash = new MpqHash(hash.Name1, hash.Name2, hash.Locale, index, hash.Mask);
            }
            else
            {
                _hash = new MpqHash(_fileName, mask, locale, index);
                _hashIndex = MpqHash.GetIndex(_fileName, mask);
            }
        }

        /*public void WriteToStream( Stream stream )
        {
            WriteToStream( new BinaryWriter( stream ) );
            //WriteToStream( new StreamWriter( stream ) );
        }*/

        public void WriteToStream(BinaryWriter writer)
        {
            var stream = _entry.IsCompressed ? _compressedStream : _baseStream;
            while (true)
            {
                var i = stream.ReadByte();
                if (i == -1)
                {
                    break;
                }
                writer.Write((byte)i);
            }

            stream.Dispose();
        }

        bool IEquatable<MpqFile>.Equals(MpqFile other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_fileName, other._fileName) == 0;
        }

        private uint Compress()
        {
            _compressedStream = new MemoryStream();

            //var blockSize = _archive?.BlockSize ?? _blockSize;
            var blockCount = (uint)( ( (int)_baseStream.Length + _blockSize - 1 ) / _blockSize ) + 1;
            var blockOffsets = new uint[blockCount];

            blockOffsets[0] = 4 * blockCount;

            _compressedStream.Position = blockOffsets[0];

            for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
            {
                using (var stream = new MemoryStream())
                {
                    using (var deflater = new DeflaterOutputStream(stream))
                    {
                        for (var i = 0; i < _blockSize; i++)
                        {
                            var r = _baseStream.ReadByte();
                            if (r == -1)
                            {
                                break;
                            }
                            deflater.WriteByte((byte)r);
                        }

                        deflater.Finish();
                        deflater.Flush();
                        stream.Position = 0;

                        // First byte in the block indicates the compression algorithm used.
                        // TODO: add enum for compression modes
                        _compressedStream.WriteByte(2);

                        while (true)
                        {
                            var read = stream.ReadByte();
                            if (read == -1)
                            {
                                break;
                            }
                            _compressedStream.WriteByte((byte)read);
                        }
                    }

                    blockOffsets[blockIndex] = (uint)_compressedStream.Position;
                }
            }

            _baseStream.Dispose();

            _compressedStream.Position = 0;

            using (var writer = new BinaryWriter(_compressedStream, new System.Text.UTF8Encoding(false, true), true))
            {
                for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                {
                    writer.Write(blockOffsets[blockIndex]);
                }
            }

            _compressedStream.Position = 0;

            return (uint)_compressedStream.Length;
        }
    }
}