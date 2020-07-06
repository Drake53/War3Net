// ------------------------------------------------------------------------------
// <copyright file="BlockTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
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
        internal BlockTable()
        {
            _entries = new List<MpqEntry>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the contents of the <see cref="BlockTable"/>.</param>
        /// <param name="size">The amount of <see cref="MpqEntry"/> objects to be added to the <see cref="BlockTable"/>.</param>
        /// <param name="headerOffset">The length (in bytes) of data before the <see cref="MpqHeader"/>.</param>
        internal BlockTable(BinaryReader reader, uint size, uint headerOffset)
        {
            var bytesRemaining = reader.BaseStream.Length - reader.BaseStream.Position;
            if (bytesRemaining < size * MpqEntry.Size)
            {
                if (bytesRemaining % MpqEntry.Size != 0)
                {
                    throw new MpqParserException($"Remaining amount of bytes ({bytesRemaining}) is not enough for {size} MPQ entries, and is also not a multiple of {MpqEntry.Size}.");
                }

                size = (uint)bytesRemaining / MpqEntry.Size;
            }

            _entries = new List<MpqEntry>((int)size);

            var entrydata = reader.ReadBytes((int)(size * MpqEntry.Size));
            Decrypt(entrydata);

            using (var stream = new MemoryStream(entrydata))
            {
                using (var streamReader = new BinaryReader(stream))
                {
                    for (var i = 0; i < size; i++)
                    {
                        _entries.Add(MpqEntry.FromReader(streamReader, headerOffset));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the capacity of the <see cref="BlockTable"/>.
        /// </summary>
        public override uint Size => (uint)_entries.Count;

        /// <summary>
        /// Gets the key used to encrypt and decrypt the <see cref="BlockTable"/>.
        /// </summary>
        protected override string Key => TableKey;

        /// <summary>
        /// Gets the length (in bytes) of a single <see cref="MpqEntry"/> in the <see cref="BlockTable"/>.
        /// </summary>
        protected override int EntrySize => (int)MpqEntry.Size;

        /// <summary>
        /// Gets or sets the <see cref="MpqEntry"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqEntry"/> to get.</param>
        /// <returns>The <see cref="MpqEntry"/> at index <paramref name="i"/> of the <see cref="BlockTable"/>.</returns>
        public MpqEntry this[int i]
        {
            get => _entries[i];
            set => _entries[i] = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="MpqEntry"/> at specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the <see cref="MpqEntry"/> to get.</param>
        /// <returns>The <see cref="MpqEntry"/> at index <paramref name="i"/> of the <see cref="BlockTable"/>.</returns>
        public MpqEntry this[uint i]
        {
            get => _entries[(int)i];
            set => _entries[(int)i] = value;
        }

        /// <summary>
        /// Adds an <see cref="MpqEntry"/> to the <see cref="BlockTable"/>.
        /// </summary>
        /// <param name="entry">The <see cref="MpqEntry"/> to be added to the <see cref="BlockTable"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entry"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="MpqEntry.FilePosition"/> property has not been set yet.</exception>
        public void Add(MpqEntry entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
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
            _entries[i].WriteTo(writer);
        }
    }
}