// ------------------------------------------------------------------------------
// <copyright file="Salsa20.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Crypto
{
    /// <summary>
    /// Salsa20 stream cipher implementation for CASC decryption.
    /// </summary>
    public class Salsa20
    {
        private const int StateSize = 16;
        private readonly uint[] _state;
        private readonly byte[] _keyStream;
        private int _keyStreamPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Salsa20"/> class.
        /// </summary>
        /// <param name="key">The encryption key (16 or 32 bytes).</param>
        /// <param name="iv">The initialization vector (8 bytes).</param>
        public Salsa20(byte[] key, byte[] iv)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length != 16 && key.Length != 32)
            {
                throw new ArgumentException("Key must be 16 or 32 bytes", nameof(key));
            }

            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (iv.Length != 8)
            {
                throw new ArgumentException("IV must be 8 bytes", nameof(iv));
            }

            _state = new uint[StateSize];
            _keyStream = new byte[64];
            _keyStreamPosition = 64;

            Initialize(key, iv);
        }

        /// <summary>
        /// Encrypts or decrypts data (XOR operation).
        /// </summary>
        /// <param name="data">The data to process.</param>
        /// <returns>The processed data.</returns>
        public byte[] Process(byte[] data)
        {
            if (data == null)
            {
                return Array.Empty<byte>();
            }

            var result = new byte[data.Length];
            Process(data, 0, data.Length, result, 0);
            return result;
        }

        /// <summary>
        /// Encrypts or decrypts data in place.
        /// </summary>
        /// <param name="data">The data to process.</param>
        public void ProcessInPlace(byte[] data)
        {
            if (data != null)
            {
                Process(data, 0, data.Length, data, 0);
            }
        }

        /// <summary>
        /// Encrypts or decrypts data.
        /// </summary>
        /// <param name="input">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="length">The number of bytes to process.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="outputOffset">The output offset.</param>
        public void Process(byte[] input, int inputOffset, int length, byte[] output, int outputOffset)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            for (int i = 0; i < length; i++)
            {
                if (_keyStreamPosition >= 64)
                {
                    GenerateKeyStream();
                    _keyStreamPosition = 0;
                }

                output[outputOffset + i] = (byte)(input[inputOffset + i] ^ _keyStream[_keyStreamPosition++]);
            }
        }

        private void Initialize(byte[] key, byte[] iv)
        {
            // Constants
            if (key.Length == 32)
            {
                // 32-byte key
                _state[0] = 0x61707865; // "expa"
                _state[5] = 0x3320646e; // "nd 3"
                _state[10] = 0x79622d32; // "2-by"
                _state[15] = 0x6b206574; // "te k"
            }
            else
            {
                // 16-byte key
                _state[0] = 0x61707865; // "expa"
                _state[5] = 0x3120646e; // "nd 1"
                _state[10] = 0x79622d36; // "6-by"
                _state[15] = 0x6b206574; // "te k"
            }

            // Key
            var keyIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                _state[i + 1] = ToUInt32(key, keyIndex);
                keyIndex += 4;
            }

            if (key.Length == 32)
            {
                for (int i = 0; i < 4; i++)
                {
                    _state[i + 11] = ToUInt32(key, keyIndex);
                    keyIndex += 4;
                }
            }
            else
            {
                // For 16-byte key, repeat the key
                keyIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    _state[i + 11] = ToUInt32(key, keyIndex);
                    keyIndex += 4;
                }
            }

            // IV
            _state[6] = ToUInt32(iv, 0);
            _state[7] = ToUInt32(iv, 4);

            // Counter (set to 0)
            _state[8] = 0;
            _state[9] = 0;

            // Unused positions
            _state[14] = 0;
            _state[13] = 0;
        }

        private void GenerateKeyStream()
        {
            var workingState = new uint[StateSize];
            Array.Copy(_state, workingState, StateSize);

            // 20 rounds (10 double-rounds)
            for (int i = 0; i < 10; i++)
            {
                QuarterRound(ref workingState[0], ref workingState[4], ref workingState[8], ref workingState[12]);
                QuarterRound(ref workingState[5], ref workingState[9], ref workingState[13], ref workingState[1]);
                QuarterRound(ref workingState[10], ref workingState[14], ref workingState[2], ref workingState[6]);
                QuarterRound(ref workingState[15], ref workingState[3], ref workingState[7], ref workingState[11]);

                QuarterRound(ref workingState[0], ref workingState[1], ref workingState[2], ref workingState[3]);
                QuarterRound(ref workingState[5], ref workingState[6], ref workingState[7], ref workingState[4]);
                QuarterRound(ref workingState[10], ref workingState[11], ref workingState[8], ref workingState[9]);
                QuarterRound(ref workingState[15], ref workingState[12], ref workingState[13], ref workingState[14]);
            }

            // Add initial state
            for (int i = 0; i < StateSize; i++)
            {
                workingState[i] += _state[i];
            }

            // Convert to byte stream
            var index = 0;
            for (int i = 0; i < StateSize; i++)
            {
                _keyStream[index++] = (byte)workingState[i];
                _keyStream[index++] = (byte)(workingState[i] >> 8);
                _keyStream[index++] = (byte)(workingState[i] >> 16);
                _keyStream[index++] = (byte)(workingState[i] >> 24);
            }

            // Increment counter (positions 8 and 9 per Salsa20 spec)
            _state[8]++;
            if (_state[8] == 0)
            {
                _state[9]++;
            }
        }

        private static void QuarterRound(ref uint a, ref uint b, ref uint c, ref uint d)
        {
            b ^= RotateLeft(a + d, 7);
            c ^= RotateLeft(b + a, 9);
            d ^= RotateLeft(c + b, 13);
            a ^= RotateLeft(d + c, 18);
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        private static uint ToUInt32(byte[] data, int offset)
        {
            return (uint)(data[offset] |
                         (data[offset + 1] << 8) |
                         (data[offset + 2] << 16) |
                         (data[offset + 3] << 24));
        }
    }
}