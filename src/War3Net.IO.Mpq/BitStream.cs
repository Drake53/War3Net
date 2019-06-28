// ------------------------------------------------------------------------------
// <copyright file="BitStream.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace Foole.Mpq
{
    /// <summary>
    /// A utility class for reading groups of bits from a stream.
    /// </summary>
    internal class BitStream
    {
        private readonly Stream _baseStream;
        private int _current;
        private int _bitCount;

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
            if (!EnsureBits(8))
            {
                return -1;
            }

            return _current & 0xff;
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

        private bool WasteBits(int bitCount)
        {
            _current >>= bitCount;
            _bitCount -= bitCount;
            return true;
        }
    }
}