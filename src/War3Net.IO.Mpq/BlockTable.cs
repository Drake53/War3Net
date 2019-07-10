// ------------------------------------------------------------------------------
// <copyright file="BlockTable.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The <see cref="BlockTable"/> of an <see cref="MpqArchive"/> contains the list of <see cref="MpqEntry"/> objects.
    /// </summary>
    internal sealed class BlockTable : MpqTable, IEnumerable<MpqEntry>
    {
        /// <summary>
        /// The key used to encrypt and decrypt the <see cref="BlockTable"/>.
        /// </summary>
        internal const string TableKey = "(block table)";

        private readonly List<MpqEntry> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="size">The maximum amount of entries that can be contained in this table.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> argument is larger than <see cref="MpqTable.MaxSize"/>.</exception>
        internal BlockTable(uint size)
            : base(size)
        {
            _entries = new List<MpqEntry>((int)size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the contents of the <see cref="BlockTable"/>.</param>
        /// <param name="size">The amount of <see cref="MpqEntry"/> objects to be added to the <see cref="BlockTable"/>.</param>
        /// <param name="headerOffset">The length (in bytes) of data before the <see cref="MpqHeader"/>.</param>
        internal BlockTable(BinaryReader reader, uint size, uint headerOffset)
            : base(size)
        {
            _entries = new List<MpqEntry>((int)size);

            var entrydata = reader.ReadBytes((int)(size * MpqEntry.Size));
            Decrypt(entrydata);

            using (var stream = new MemoryStream(entrydata))
            {
                using (var streamReader = new BinaryReader(stream))
                {
                    for (var i = 0; i < size; i++)
                    {
                        _entries.Add(new MpqEntry(streamReader, headerOffset));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the key used to encrypt and decrypt the <see cref="BlockTable"/>.
        /// </summary>
        protected override string Key => TableKey;

        /// <summary>
        /// Gets the length (in bytes) of a single <see cref="MpqEntry"/> in the <see cref="BlockTable"/>.
        /// </summary>
        protected override int EntrySize => (int)MpqEntry.Size;

        /// <summary>
        /// Gets the <see cref="MpqEntry"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqEntry"/> to get.</param>
        /// <returns>The <see cref="MpqEntry"/> at index <paramref name="i"/> of the <see cref="BlockTable"/>.</returns>
        public MpqEntry this[int i] => _entries[i];

        /// <summary>
        /// Gets the <see cref="MpqEntry"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqEntry"/> to get.</param>
        /// <returns>The <see cref="MpqEntry"/> at index <paramref name="i"/> of the <see cref="BlockTable"/>.</returns>
        public MpqEntry this[uint i] => _entries[(int)i];

        /// <summary>
        /// Adds an <see cref="MpqEntry"/> to the <see cref="BlockTable"/>.
        /// </summary>
        /// <param name="entry">The <see cref="MpqEntry"/> to be added to the <see cref="BlockTable"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entry"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="MpqEntry.FilePos"/> property has not been set yet.</exception>
        public void Add(MpqEntry entry)
        {
            if (!(entry?.IsAdded ?? throw new ArgumentNullException(nameof(entry))))
            {
                throw new InvalidOperationException("Cannot add an MpqEntry to the BlockTable before its FilePos is known.");
            }

            _entries.Add(entry);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<MpqEntry> IEnumerable<MpqEntry>.GetEnumerator()
        {
            foreach (var entry in _entries)
            {
                yield return entry;
            }
        }

        /// <summary>
        /// Writes the <see cref="MpqEntry"/> at index <paramref name="i"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write the content to.</param>
        /// <param name="i">The index of the <see cref="MpqEntry"/> to write.</param>
        protected override void WriteEntry(BinaryWriter writer, int i)
        {
            _entries[i].WriteEntry(writer);
        }
    }
}