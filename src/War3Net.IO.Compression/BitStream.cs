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
        /// <param name="sourceStream">The stream to be read.</param>
        public BitStream(Stream sourceStream)
        {
            _baseStream = sourceStream;
        }

        /// <summary>
        /// Read up to 16 bits from the underlying stream.
        /// </summary>
        /// <param name="bitCount">The amount of bits to read.</param>
        /// <returns>The bits read, or -1 if at the end of the stream.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="bitCount"/> is greater than 16.</exception>
        public int ReadBits(int bitCount)
        {
            if (bitCount > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount), $"Maximum {nameof(bitCount)} is {16}");
            }

            if (!EnsureBits(bitCount))
            {
                return -1;
            }

            var result = _current & (0xffff >> (16 - bitCount));
            WasteBits(bitCount);
            return result;
        }

        /// <summary>
        /// Returns the next byte but does not consume it.
        /// </summary>
        /// <returns>The next byte to be read, or -1 if at the end of the stream.</returns>
        public int PeekByte()
        {
            return EnsureBits(8)
                ? _current & 0xff
                : -1;
        }

        /// <summary>
        /// Ensures a certain amount of bits are available, by reading from the underlying stream.
        /// </summary>
        /// <param name="bitCount">The amount of bits that must be available to read.</param>
        /// <returns>True if the requested amount of bits are now available, false otherwise.</returns>
        internal bool EnsureBits(int bitCount)
        {
            // The requested amount of bits are already available.
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