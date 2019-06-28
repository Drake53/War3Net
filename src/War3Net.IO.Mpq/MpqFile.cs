using System;
using System.IO;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Foole.Mpq
{
    public class MpqFile : IEquatable<MpqFile>
    {
        //private MpqArchive _archive;

        private MpqEntry _entry;
        private MpqHash? _hash;
        private uint _hashIndex; // position in hashtable
        private uint _hashCollisions; // possible amount of collisions this unknown file had in old archive

        private Stream _baseStream;
        private MemoryStream _compressedStream;
        private int _blockSize; // used for compression

        private string _fileName;

        public MpqFile( Stream sourceStream, string fileName, MpqFileFlags flags, ushort blockSize )
        {
            _baseStream = sourceStream;
            _fileName = fileName;

            _blockSize = 0x200 << blockSize;

            var fileSize = (uint)_baseStream.Length;
            var compressedSize = ( ( flags & MpqFileFlags.Compressed ) != 0 ) ? Compress() : fileSize;

            _entry = new MpqEntry( fileName, compressedSize, fileSize, flags );
        }

        // Use this contructor for files that came from another archive, and for which the filename is unknown.
        public MpqFile( Stream sourceStream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, MpqFileFlags flags, ushort blockSize )
            : this( sourceStream, null, flags, blockSize )
        {
            if ( mpqHash.Mask == 0 )
            {
                throw new ArgumentException( "Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash) );
            }

            _hash = mpqHash;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
        }

        public MpqEntry MpqEntry => _entry;

        public MpqHash MpqHash => _hash.Value;

        public uint HashIndex => _hashIndex;

        public uint HashCollisions => _hashCollisions;

        public Stream BaseStream => _baseStream;

        public MemoryStream MemoryStream => _compressedStream;

        public string Name => _fileName;

        //public MpqArchive Archive => _archive;

        /*public void AddToArchive( MpqArchive archive )
        {
            if ( _archive != null )
            {
                throw new InvalidOperationException();
            }

            _archive = archive;
            _entry.SetPos( 0 );
        }*/

        public void AddToArchive( uint headerOffset, uint index, uint filePos, uint locale, uint mask )
        {
            _entry.SetPos( headerOffset, filePos );

            // This file came from another archive, and has an unknown filename.
            if ( _hash.HasValue )
            {
                // Overwrite blockIndex from old archive.
                var hash = _hash.Value;
                _hash = new MpqHash( hash.Name1, hash.Name2, hash.Locale, index, hash.Mask );
            }
            else
            {
                _hash = new MpqHash( _fileName, mask, locale, index );
                _hashIndex = MpqHash.GetIndex( _fileName, mask );
            }
        }

        /*public void WriteToStream( Stream stream )
        {
            WriteToStream( new BinaryWriter( stream ) );
            //WriteToStream( new StreamWriter( stream ) );
        }*/

        public void WriteToStream( BinaryWriter writer )
        {
            var stream = _entry.IsCompressed ? _compressedStream : _baseStream;
            while ( true )
            {
                var i = stream.ReadByte();
                if ( i == -1 )
                {
                    break;
                }
                writer.Write( (byte)i );
            }

            stream.Dispose();
        }

        private uint Compress()
        {
            _compressedStream = new MemoryStream();

            //var blockSize = _archive?.BlockSize ?? _blockSize;
            var blockCount = (uint)( ( (int)_baseStream.Length + _blockSize - 1 ) / _blockSize ) + 1;
            var blockOffsets = new uint[blockCount];

            blockOffsets[0] = 4 * blockCount;

            _compressedStream.Position = blockOffsets[0];

            for ( var blockIndex = 1; blockIndex < blockCount; blockIndex++ )
            {
                using ( var stream = new MemoryStream() )
                {
                    using ( var deflater = new DeflaterOutputStream( stream ) )
                    {
                        for ( var i = 0; i < _blockSize; i++ )
                        {
                            var r = _baseStream.ReadByte();
                            if ( r == -1 )
                            {
                                break;
                            }
                            deflater.WriteByte( (byte)r );
                        }

                        deflater.Finish();
                        deflater.Flush();
                        stream.Position = 0;

                        // First byte in the block indicates the compression algorithm used.
                        // TODO: add enum for compression modes
                        _compressedStream.WriteByte( 2 );

                        while ( true )
                        {
                            var read = stream.ReadByte();
                            if ( read == -1 )
                            {
                                break;
                            }
                            _compressedStream.WriteByte( (byte)read );
                        }
                    }

                    blockOffsets[blockIndex] = (uint)_compressedStream.Position;
                }
            }

            _baseStream.Dispose();

            _compressedStream.Position = 0;

            using ( var writer = new BinaryWriter( _compressedStream, new System.Text.UTF8Encoding( false, true ), true ) )
            {
                for ( var blockIndex = 0; blockIndex < blockCount; blockIndex++ )
                {
                    writer.Write( blockOffsets[blockIndex] );
                }
            }

            _compressedStream.Position = 0;

            return (uint)_compressedStream.Length;
        }

        bool IEquatable<MpqFile>.Equals( MpqFile other )
        {
            return StringComparer.OrdinalIgnoreCase.Compare( _fileName, other._fileName ) == 0;
        }
    }
}