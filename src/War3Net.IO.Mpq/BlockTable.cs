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

namespace Foole.Mpq
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
        /// <param name="size"></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> argument is larger than <see cref="MpqTable.MaxSize"/>.</exception>
        internal BlockTable(uint size)
            : base(size)
        {
            _entries = new List<MpqEntry>((int)size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="size"></param>
        /// <param name="headerOffset"></param>
        internal BlockTable(BinaryReader reader, uint size, uint headerOffset)
            : base(size)
        {
            _entries = new List<MpqEntry>((int)size);

            var entrydata = reader.ReadBytes((int)(size * MpqEntry.Size));
            Decrypt(entrydata, TableKey);

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
        public override string Key => TableKey;

        /// <inheritdoc/>
        protected internal override int EntrySize => (int)MpqEntry.Size;

        public MpqEntry this[int index] => _entries[index];

        public MpqEntry this[uint index] => _entries[(int)index];

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <inheritdoc/>
        protected override void WriteEntry(BinaryWriter writer, int i)
        {
            _entries[i].WriteEntry(writer);
        }
    }
}