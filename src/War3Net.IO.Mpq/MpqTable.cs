// ------------------------------------------------------------------------------
// <copyright file="MpqTable.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace Foole.Mpq
{
    internal abstract class MpqTable
    {
        public const int MaxSize = 0x1 << 15;

        protected uint _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqTable"/> class.
        /// </summary>
        /// <param name="size"></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> argument is larger than <see cref="MaxSize"/>.</exception>
        public MpqTable(uint size)
        {
            if (size > MaxSize)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            _size = size;
        }

        public uint Size => _size;

        public abstract string Key { get; }

        protected internal abstract int EntrySize { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public static void Encrypt(byte[] data, string key)
        {
            StormBuffer.EncryptBlock(data, StormBuffer.HashString(key, 0x300));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public static void Decrypt(byte[] data, string key)
        {
            StormBuffer.DecryptBlock(data, StormBuffer.HashString(key, 0x300));
        }

        /*public void WriteToStream( Stream stream )
        {
            WriteToStream( new BinaryWriter( stream ) );
            //WriteToStream( new StreamWriter( stream ) );
        }*/

        public void WriteToStream(BinaryWriter writer)
        {
            using (var memoryStream = new MemoryStream(GetEncryptedData()))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    var end = memoryStream.Length;
                    for (var i = 0; i < end; i++)
                    {
                        writer.Write(reader.ReadByte());
                    }
                }
            }
        }

        protected abstract void WriteEntry(BinaryWriter writer, int i);

        private byte[] GetEncryptedData()
        {
            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memoryStream, new System.Text.UTF8Encoding(false, true), true))
                {
                    for (var i = 0; i < _size; i++)
                    {
                        WriteEntry(writer, i);
                    }
                }

                memoryStream.Position = 0;

                using (var reader = new BinaryReader(memoryStream))
                {
                    data = reader.ReadBytes((int)_size * EntrySize);
                    Encrypt(data, Key);
                }
            }

            return data;
        }
    }
}