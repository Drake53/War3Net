using System;
using System.IO;

namespace Foole.Mpq
{
    [Flags]
    public enum MpqFileFlags : uint
    {
        CompressedPK = 0x100, // AKA Imploded
        CompressedMulti = 0x200,
        Compressed = 0xff00,
        Encrypted = 0x10000,
        BlockOffsetAdjustedKey = 0x020000, // AKA FixSeed
        SingleUnit = 0x1000000,
        FileHasMetadata = 0x04000000, // Appears in WoW 1.10 or newer.  Indicates the file has associated metadata.
        Exists = 0x80000000,
    }

    public class MpqEntry
    {
        public uint CompressedSize { get; private set; }
        public uint FileSize { get; private set; }
        public MpqFileFlags Flags { get; internal set; }
        public uint EncryptionSeed { get; internal set; }

        private uint _fileOffset; // Relative to the header offset
        internal uint FilePos { get; private set; } // Absolute position in the file
        internal bool IsAdded { get; private set; }
        private string _filename;

        public static readonly uint Size = 16;

        public MpqEntry(BinaryReader br, uint headerOffset)
        {
            _fileOffset = br.ReadUInt32();
            FilePos = headerOffset + _fileOffset;
            CompressedSize = br.ReadUInt32();
            FileSize = br.ReadUInt32();
            Flags = (MpqFileFlags)br.ReadUInt32();

            IsAdded = true;
        }

        internal MpqEntry( uint filePos, uint headerOffset, uint compressedSize, uint fileSize, MpqFileFlags flags )
        {
            _fileOffset = filePos - headerOffset;
            FilePos = filePos;
            CompressedSize = compressedSize;
            FileSize = fileSize;
            Flags = flags;
            EncryptionSeed = 0;

            IsAdded = true;
        }

        internal MpqEntry( uint filePos, uint compressedSize, uint fileSize, MpqFileFlags flags )
        {
            FilePos = filePos;
            CompressedSize = compressedSize;
            FileSize = fileSize;
            Flags = flags;

            IsAdded = true;
        }

        internal MpqEntry( string filename, uint compressedSize, uint fileSize, MpqFileFlags flags )
        {
            CompressedSize = compressedSize;
            FileSize = fileSize;
            Flags = flags;

            _filename = filename;
        }

        public static MpqEntry Dummy => new MpqEntry( null, 0, 0, MpqFileFlags.Exists ); //remove exists flag??

        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                EncryptionSeed = CalculateEncryptionSeed();
            }
        }

        public bool IsEncrypted => ( Flags & MpqFileFlags.Encrypted ) != 0;

        public bool IsCompressed => ( Flags & MpqFileFlags.Compressed ) != 0;

        public bool Exists => Flags != 0;

        public bool IsSingleUnit => ( Flags & MpqFileFlags.SingleUnit ) != 0;

        // For debugging
        public int FlagsAsInt => (int)Flags;

        public void SetPos( uint headerOffset, uint fileOffset )
        {
            if ( IsAdded )
            {
                throw new InvalidOperationException( "Cannot change the FilePos for an MpqEntry after it's been set." );
            }

            _fileOffset = fileOffset;
            FilePos = headerOffset + fileOffset;

            IsAdded = true;
        }

        public void WriteEntry( BinaryWriter writer )
        {
            writer.Write( _fileOffset );
            writer.Write( CompressedSize );
            writer.Write( FileSize );
            writer.Write( (uint)Flags );
        }

        public override string ToString()
        {
            if (Filename == null)
            {
                if (!Exists)
                    return "(Deleted file)";
                return string.Format("Unknown file @ {0}", FilePos);
            }
            return Filename;
        }

        private uint CalculateEncryptionSeed()
        {
            if ( Filename == null )
                return 0;

            uint seed = StormBuffer.HashString( Path.GetFileName( Filename ), 0x300 );
            if ( ( Flags & MpqFileFlags.BlockOffsetAdjustedKey ) == MpqFileFlags.BlockOffsetAdjustedKey )
                seed = ( seed + _fileOffset ) ^ FileSize;
            return seed;
        }
    }
}