// ------------------------------------------------------------------------------
// <copyright file="MpqTable.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// A table in an <see cref="MpqArchive"/>.
    /// </summary>
    internal abstract class MpqTable
    {
        /// <summary>
        /// The maximum capacity of an <see cref="MpqTable"/>.
        /// </summary>
        public const int MaxSize = 0x1 << 15;

        private readonly uint _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqTable"/> class.
        /// </summary>
        /// <param name="size">The maximum amount of entries that can be contained in this table.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> argument is larger than <see cref="MaxSize"/>.</exception>
        public MpqTable(uint size)
        {
            if (size > MaxSize)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            _size = size;
        }

        /// <summary>
        /// Gets the capacity of the <see cref="MpqTable"/>.
        /// </summary>
        public uint Size => _size;

        /// <summary>
        /// Gets the key used to encrypt and decrypt the <see cref="MpqTable"/>.
        /// </summary>
        protected abstract string Key { get; }

        /// <summary>
        /// Gets the length (in bytes) of a single entry in the <see cref="MpqTable"/>.
        /// </summary>
        protected abstract int EntrySize { get; }

        /// <summary>
        /// Encrypts the contents of the <see cref="MpqTable"/>.
        /// </summary>
        /// <param name="data">The unencrypted entries in the table.</param>
        public void Encrypt(byte[] data)
        {
            StormBuffer.EncryptBlock(data, StormBuffer.HashString(Key, 0x300));
        }

        /// <summary>
        /// Decrypts the contents of the <see cref="MpqTable"/>.
        /// </summary>
        /// <param name="data">The encrypted entries in the table.</param>
        public void Decrypt(byte[] data)
        {
            StormBuffer.DecryptBlock(data, StormBuffer.HashString(Key, 0x300));
        }

        /// <summary>
        /// Write the entire <see cref="MpqTable"/>'s encrypted contents to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write the contents to.</param>
        public void SerializeTo(Stream stream)
        {
            using (var writer = new BinaryWriter(stream, new System.Text.UTF8Encoding(false, true), true))
            {
                WriteTo(writer);
            }
        }

        /// <summary>
        /// Write the entire <see cref="MpqTable"/>'s encrypted contents to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write the contents to.</param>
        public void WriteTo(BinaryWriter writer)
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

        /// <summary>
        /// Writes the entry at index <paramref name="i"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write the content to.</param>
        /// <param name="i">The index of the entry to write.</param>
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
                    Encrypt(data);
                }
            }

            return data;
        }
    }
}