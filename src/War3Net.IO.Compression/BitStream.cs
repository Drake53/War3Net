// ------------------------------------------------------------------------------
// <copyright file="BitStream.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Compression
{
    /// <summary>
    /// A utility class for reading groups of bits from a stream.
    /// </summary>
    internal class BitStream
    {
        private readonly Stream _baseStream;
        private int _current;
        private int _bitCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        public BitStream(Stream sourceStream)
        {
            _baseStream = sourceStream;
        }

        public int ReadBits(int bitCount)
        {
            if (bitCount > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount), "Maximum BitCount is 16");
            }

            if (!EnsureBits(bitCount))
            {
                return -1;
            }

            var result = _current & (0xffff >> (16 - bitCount));
            WasteBits(bitCount);
            return result;
        }

        public int PeekByte()
        {
            return EnsureBits(8)
                ? _current & 0xff
                : -1;
        }

        public bool EnsureBits(int bitCount)
        {
            if (bitCount <= _bitCount)
            {
                return true;
            }

            if (_baseStream.Position >= _baseStream.Length)
            {
                return false;
            }

            var nextvalue = _baseStream.ReadByte();
            _current |= nextvalue << _bitCount;
            _bitCount += 8;
            return true;
        }

        private void WasteBits(int bitCount)
        {
            _current >>= bitCount;
            _bitCount -= bitCount;
        }
    }
}