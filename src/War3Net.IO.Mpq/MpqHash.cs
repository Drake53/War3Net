using System.IO;

namespace Foole.Mpq
{
    public struct MpqHash
    {
        public uint Name1 { get; private set; }
        public uint Name2 { get; private set; }
        public uint Locale { get; private set; } // Normally 0 or UInt32.MaxValue (0xffffffff)
        public uint BlockIndex { get; private set; }
        public uint Mask { get; private set; }

        public static MpqHash DELETED => new MpqHash( 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFE );
        public static MpqHash NULL => new MpqHash( 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF ); // rename EMPTY

        public static readonly uint Size = 16;

        private MpqHash( uint name1, uint name2, uint locale, uint blockIndex )
            : this()
        {
            Name1 = name1;
            Name2 = name2;
            Locale = locale;
            BlockIndex = blockIndex;
        }

        public MpqHash( uint name1, uint name2, uint locale, uint blockIndex, uint mask )
            : this( name1, name2, locale, blockIndex )
        {
            Mask = mask;
        }

        public MpqHash( BinaryReader br, uint mask )
            : this( br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32() )
        {
            Mask = mask;
        }

        public MpqHash( string fileName, uint mask, uint locale, uint blockIndex )
            : this( StormBuffer.HashString( fileName, 0x100 ), StormBuffer.HashString( fileName, 0x200 ), locale, blockIndex, mask )
        { }

        public static uint GetIndex( string path, uint mask )
        {
            return StormBuffer.HashString( path, 0 ) & mask;
        }

        public bool IsEmpty()
        {
            return BlockIndex == 0xFFFFFFFF;
        }

        public bool IsDeleted()
        {
            return BlockIndex == 0xFFFFFFFE;
        }

        public bool IsOccupied()
        {
            return BlockIndex < 0xFFFFFFFE;
        }

        public override string ToString()
        {
            if ( IsEmpty() )
            {
                return "EMPTY";
            }

            if ( IsDeleted() )
            {
                return "DELETED";
            }

            return string.Format( "Entry #{0}", this.BlockIndex );
        }
    }
}