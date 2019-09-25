// ------------------------------------------------------------------------------
// <copyright file="HashTable.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The <see cref="HashTable"/> of an <see cref="MpqArchive"/> contains the list of <see cref="MpqHash"/> objects.
    /// </summary>
    internal sealed class HashTable : MpqTable
    {
        /// <summary>
        /// The key used to encrypt and decrypt the <see cref="HashTable"/>.
        /// </summary>
        internal const string TableKey = "(hash table)";

        private readonly MpqHash[] _hashes;
        private readonly uint _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTable"/> class.
        /// </summary>
        /// <param name="size">The maximum amount of entries that can be contained in this table. This value is automatically rounded up to the nearest power of two.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> argument is larger than <see cref="MpqTable.MaxSize"/>.</exception>
        public HashTable(uint size)
            : base(GenerateMask(size) + 1)
        {
            // The size of the hashtable must always be a power of two.
            _mask = Size - 1;
            _hashes = new MpqHash[Size];
            for (var i = 0; i < Size; i++)
            {
                _hashes[i] = MpqHash.NULL;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTable"/> class.
        /// </summary>
        /// <param name="minimumSize">The minimum capacity that this <see cref="HashTable"/> should have.</param>
        /// <param name="freeSpace">Determines how much space is available for files with known filenames. Use 1 if no files with an unknown filename will be added.</param>
        /// <param name="multiplier">Multiplier for the size of the hashtable. By increasing the size beyond the minimum, the amount of collisions with StringHash will be reduced.</param>
        /// <exception cref="DivideByZeroException">Thrown when <paramref name="freeSpace"/> is zero.</exception>
        public HashTable(uint minimumSize, float freeSpace, float multiplier)
            : this(Math.Min(MaxSize, Math.Max(minimumSize, (uint)(multiplier * minimumSize / freeSpace))))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTable"/> class.
        /// </summary>
        /// <param name="knownFiles">The amount of files with a known filename that will be added to the <see cref="HashTable"/>.</param>
        /// <param name="unknownFiles">The amount of files with an unknown filename that will be added to the <see cref="HashTable"/>.</param>
        /// <param name="oldTableSize">The size of the smallest <see cref="HashTable"/> from which the unknown files came.</param>
        /// <param name="multiplier">Multiplier for the size of the hashtable. By increasing the size beyond the minimum, the amount of collisions with StringHash will be reduced.</param>
        /// <exception cref="DivideByZeroException">Thrown when <paramref name="unknownFiles"/> is equal to <paramref name="oldTableSize"/>.</exception>
        /// <remarks>
        /// If the unknown files are sourced from multiple <see cref="HashTable"/>s with different sizes, it's recommended to use a different constructor.
        /// </remarks>
        public HashTable(uint knownFiles, uint unknownFiles, uint oldTableSize, float multiplier)
            : this(knownFiles, 1 - ((float)unknownFiles / oldTableSize), multiplier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the contents of the <see cref="HashTable"/>.</param>
        /// <param name="size">The amount of <see cref="MpqHash"/> objects to be added to the <see cref="HashTable"/>.</param>
        internal HashTable(BinaryReader reader, uint size)
            : base(size)
        {
            _mask = Size - 1;
            _hashes = new MpqHash[Size];

            var hashdata = reader.ReadBytes((int)(size * MpqHash.Size));
            Decrypt(hashdata);

            using (var stream = new MemoryStream(hashdata))
            {
                using (var streamReader = new BinaryReader(stream))
                {
                    for (var i = 0; i < size; i++)
                    {
                        _hashes[i] = new MpqHash(streamReader, _mask);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the mask for this <see cref="HashTable"/>.
        /// </summary>
        public uint Mask => _mask;

        /// <summary>
        /// Gets the key used to encrypt and decrypt the <see cref="HashTable"/>.
        /// </summary>
        protected override string Key => TableKey;

        /// <summary>
        /// Gets the length (in bytes) of a single <see cref="MpqHash"/> in the <see cref="HashTable"/>.
        /// </summary>
        protected override int EntrySize => (int)MpqHash.Size;

        /// <summary>
        /// Gets or sets the <see cref="MpqHash"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqHash"/> to get.</param>
        /// <returns>The <see cref="MpqHash"/> at index <paramref name="i"/> of the <see cref="HashTable"/>.</returns>
        internal MpqHash this[int i]
        {
            get => _hashes[i];
            set => _hashes[i] = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="MpqHash"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqHash"/> to get.</param>
        /// <returns>The <see cref="MpqHash"/> at index <paramref name="i"/> of the <see cref="HashTable"/>.</returns>
        internal MpqHash this[uint i]
        {
            get => _hashes[i];
            set => _hashes[i] = value;
        }

        /// <summary>
        /// Generates a bit mask for the given <paramref name="size"/>.
        /// </summary>
        /// <param name="size">The size for which to generate a bit mask.</param>
        /// <returns>Returns the bit mask for the given <paramref name="size"/>.</returns>
        public static uint GenerateMask(uint size)
        {
            size--;
            size |= size >> 1;
            size |= size >> 2;
            size |= size >> 4;
            size |= size >> 8;
            size |= size >> 16;
            return size;
        }

        /// <summary>
        /// Adds an <see cref="MpqHash"/> to the <see cref="HashTable"/>.
        /// </summary>
        /// <param name="hash">The <see cref="MpqHash"/> to be added to the <see cref="HashTable"/>.</param>
        /// <param name="hashIndex">The index at which to add the <see cref="MpqHash"/>.</param>
        /// <param name="hashCollisions">The maximum amount of collisions, if the <see cref="MpqFile"/> came from another <see cref="MpqArchive"/> and has an unknown filename.</param>
        /// <returns>
        /// Returns the amount of <see cref="MpqHash"/> objects that have been added.
        /// This is usually 1, but can be more if the <see cref="MpqFile"/> came from another <see cref="MpqArchive"/>, has an unknown filename,
        /// and the <see cref="HashTable"/> of the <see cref="MpqArchive"/> it came from has a smaller size than this one.
        /// </returns>
        public uint Add(MpqHash hash, uint hashIndex, uint hashCollisions)
        {
            var step = hash.Mask + 1;

            var known = step > _mask;
            Console.WriteLine(
                "Adding file #{0} to hashtable, which is {1} file{2}.",
                hash.BlockIndex,
                known ? "a known" : "an unknown",
                known ? string.Empty : $" found at index {hashIndex} with up to {hashCollisions} collisions");

            // If the hash.Mask is smaller than the hashtable's size, this file came from another archive and has an unknown filename.
            // By passing both the mask and the hashIndex corresponding to that mask, can figure out all hashIndices where this file may belong.
            AddEntry(hash, hashIndex, step);

            // For files with unknown filename, it's also possible that the index at which they were found in the HashTable is not their true index.
            // This is because there may have been StringHash collisions in the HashTable.
            // To deal with this, mark the empty entries in this hashtable, where this file's true hashIndex may be located, as deleted.
            while (hashCollisions > 0)
            {
                if (hashIndex == 0)
                {
                    hashIndex = step;
                }

                /** NOTE: replacing AddEntry with AddDeleted is only possible if passing true to the returnOnUnknown argument of method <see cref="MpqArchive.FindCollidingHashEntries"/> */

                AddDeleted(--hashIndex, step);
                // AddEntry( hash, --hashIndex, step );
                hashCollisions--;
            }

            return Size / step;
        }

        /// <summary>
        /// Writes the <see cref="MpqHash"/> at index <paramref name="i"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write the content to.</param>
        /// <param name="i">The index of the <see cref="MpqHash"/> to write.</param>
        protected override void WriteEntry(BinaryWriter writer, int i)
        {
            // TODO: make method in MpqHash for this?
            // _hashes[i].WriteEntry(writer);
            var hash = _hashes[i];

            writer.Write(hash.Name1);
            writer.Write(hash.Name2);
            writer.Write((uint)hash.Locale);
            writer.Write(hash.BlockIndex);
        }

        private void AddDeleted(uint hashIndex, uint step)
        {
            for (var i = hashIndex; i <= _mask; i += step)
            {
                if (_hashes[i].IsEmpty)
                {
                    Console.WriteLine("Marked index {0} as deleted.", i);
                    _hashes[i] = MpqHash.DELETED;
                }
                else if (_hashes[i].IsDeleted)
                {
                    Console.WriteLine("Index {0} was already marked as deleted.", i);
                }
                else if (_hashes[i].Mask == step - 1)
                {
                    Console.WriteLine("Index {0} is reserved for another unknown file.", i);
                }
                else
                {
                    Console.WriteLine("A known file is already located at index {0}.", i);
                }
            }
        }

        private void AddEntry(MpqHash hash, uint hashIndex, uint step)
        {
            // If the old archive had a smaller hashtable, it masked less bits to determine the index for the hash entry, and cannot recover the bits that were masked away.
            // As a result, need to add this hash entry in every index where the bits match with the old archive's mask.
            for (var i = hashIndex; i <= _mask; i += step)
            {
                // Console.WriteLine( "Try to add file #{0}'s hash at index {1}", hash.BlockIndex, i );
                TryAdd(hash, i);
            }
        }

        private void TryAdd(MpqHash hash, uint index)
        {
            while (!_hashes[index].IsEmpty)
            {
                // Deal with collisions
                index = (index + 1) & _mask;

                // or: if (++index)>_mask index=0;
            }

            _hashes[index] = hash;
        }
    }
}