using System;
using System.IO;

namespace Foole.Mpq
{
    /// <summary>
    /// The <see cref="HashTable"/> of an <see cref="MpqArchive"/> contains the list of <see cref="MpqHash"/> objects.
    /// </summary>
    internal sealed class HashTable : MpqTable
    {
        internal const string TableKey = "(hash table)";

        private MpqHash[] _hashes;
        private uint _mask;

        // The size of the hashtable must always be a power of two.
        public static uint GenerateMask( uint size )
        {
            size--;
            size |= size >> 1;
            size |= size >> 2;
            size |= size >> 4;
            size |= size >> 8;
            size |= size >> 16;
            return size;
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public HashTable( uint size ) : base( GenerateMask( size ) + 1 )
        {
            _mask = _size - 1;
            _hashes = new MpqHash[_size];
            for ( var i = 0; i < _size; i++ )
            {
                _hashes[i] = MpqHash.NULL;
            }
        }

        /// <param name="freeSpace">Determines how much space is available for files with known filenames. Use 1 if no files with an unknown filename will be added.</param>
        /// <param name="multiplier">Multiplier for the size of the hashtable. By increasing the size beyond the minimum, the amount of collisions with StringHash will be reduced.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="DivideByZeroException"></exception>
        public HashTable( uint minimumSize, float freeSpace, float multiplier )
            : this( Math.Min( MaxSize, Math.Max( minimumSize, (uint)( multiplier * minimumSize / freeSpace ) ) ) ) { }

        public HashTable( uint knownFiles, uint unknownFiles, uint oldHeaderSize, float multiplier )
            : this( knownFiles, 1 - (float)unknownFiles / oldHeaderSize, multiplier ) { }

        internal HashTable( BinaryReader reader, uint size ) : base( size )
        {
            _mask = _size - 1;
            _hashes = new MpqHash[_size];

            var hashdata = reader.ReadBytes( (int)( size * MpqHash.Size ) );
            Decrypt( hashdata, TableKey );

            using ( var stream = new MemoryStream( hashdata ) )
            {
                using ( var streamReader = new BinaryReader( stream ) )
                {
                    for ( var i = 0; i < size; i++ )
                    {
                        _hashes[i] = new MpqHash( streamReader, _mask );
                    }
                }
            }
        }

        public uint Mask => _mask;

        public override string Key => TableKey;

        protected internal override int EntrySize => (int)MpqHash.Size;

        internal MpqHash this[int index]
        {
            get => _hashes[index];
        }

        internal MpqHash this[uint index]
        {
            get => _hashes[index];
        }

        public uint Add( MpqHash hash, uint hashIndex, uint hashCollisions )
        {
            var step = hash.Mask + 1;

            var known = step > _mask;
            Console.WriteLine( "Adding file #{0} to hashtable, which is {1} file{2}.",
                hash.BlockIndex,
                known ? "a known" : "an unknown",
                known
                    ? string.Empty
                    : string.Format( " found at index {0} with up to {1} collisions",
                        hashIndex,
                        hashCollisions ) );

            // If the hash.Mask is smaller than the hashtable's size, this file came from another archive and has an unknown filename.
            // By passing both the mask and the hashIndex corresponding to that mask, can figure out all hashIndices where this file may belong.
            AddEntry( hash, hashIndex, step );

            // For files with unknown filename, it's also possible that the index at which they were found in the HashTable is not their true index.
            // This is because there may have been StringHash collisions in the HashTable.
            // To deal with this, mark the empty entries in this hashtable, where this file's true hashIndex may be located, as deleted.
            while ( hashCollisions > 0 )
            {
                if ( hashIndex == 0 )
                {
                    hashIndex = step;
                }

                /** NOTE: replacing AddEntry with AddDeleted is only possible if passing true to the returnOnUnknown argument of method <see cref="MpqArchive.FindCollidingHashEntries"/> */

                AddDeleted( --hashIndex, step );
                //AddEntry( hash, --hashIndex, step );
                hashCollisions--;
            }

            return _size / step;
        }

        private void AddDeleted( uint hashIndex, uint step )
        {
            for ( var i = hashIndex; i <= _mask; i += step )
            {
                if ( _hashes[i].IsEmpty() )
                {
                    Console.WriteLine( "Marked index {0} as deleted.", i );
                    _hashes[i] = MpqHash.DELETED;
                }
                else if ( _hashes[i].IsDeleted() )
                {
                    Console.WriteLine( "Index {0} was already marked as deleted.", i );
                }
                else if ( _hashes[i].Mask == step - 1 )
                {
                    Console.WriteLine( "Index {0} is reserved for another unknown file.", i );
                }
                else
                {
                    Console.WriteLine( "A known file is already located at index {0}.", i );
                }
            }
        }

        private void AddEntry( MpqHash hash, uint hashIndex, uint step )
        {
            // If the old archive had a smaller hashtable, it masked less bits to determine the index for the hash entry, and cannot recover the bits that were masked away.
            // As a result, need to add this hash entry in every index where the bits match with the old archive's mask.
            for ( var i = hashIndex; i <= _mask; i += step )
            {
                //Console.WriteLine( "Try to add file #{0}'s hash at index {1}", hash.BlockIndex, i );
                TryAdd( hash, i );
            }
        }

        private void TryAdd( MpqHash hash, uint index )
        {
            while ( _hashes[index].IsOccupied() )
            {
                //Console.WriteLine( "Collision" );

                // Deal with collisions
                index = ( index + 1 ) & _mask;

                // or: if (++index)>_mask index=0;
            }
            _hashes[index] = hash;
        }

        protected override void WriteEntry( BinaryWriter writer, int i )
        {
            var hash = _hashes[i];

            // TODO: make method in MpqHash for this?
            writer.Write( hash.Name1 );
            writer.Write( hash.Name2 );
            writer.Write( hash.Locale );
            writer.Write( hash.BlockIndex );
        }
    }
}