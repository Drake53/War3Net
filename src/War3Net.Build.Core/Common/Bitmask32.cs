// ------------------------------------------------------------------------------
// <copyright file="Bitmask32.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Build.Common
{
    public sealed class Bitmask32
    {
        private int _mask;

        public Bitmask32()
        {
            _mask = -1;
        }

        public Bitmask32(int mask)
        {
            _mask = mask;
        }

        public Bitmask32(params bool[] bits)
        {
            if (bits.Length > 32)
            {
                throw new ArgumentException($"Got {bits.Length} booleans, but can only store 32 bits.", nameof(bits));
            }

            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    _mask |= 1 << i;
                }
            }
        }

        public Bitmask32(params int[] bitIndices)
        {
            for (var b = 0; b < bitIndices.Length; b++)
            {
                var i = bitIndices[b];
                if (i < 0 || i >= 32)
                {
                    throw new ArgumentOutOfRangeException(nameof(bitIndices));
                }

                _mask |= 1 << i;
            }
        }

        internal Bitmask32(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public bool this[int index]
        {
            get => index >= 0 && index < 32 ? (_mask & 1 << index) != 0 : throw new ArgumentOutOfRangeException(nameof(index));
            set => _mask = index >= 0 && index < 32 ? value ? _mask | 1 << index : _mask & ~(1 << index) : throw new ArgumentOutOfRangeException(nameof(index));
        }

        public void Clear() => _mask = 0;

        public override string ToString() => Convert.ToString(_mask, 2).PadLeft(32, '0');

        internal void ReadFrom(BinaryReader reader)
        {
            _mask = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(_mask);
        }
    }
}