namespace War3Net.IO.Mpq
{
    public sealed class MpqArchiveCreateOptions
    {
        /// <summary>
        /// Default value for an <see cref="MpqArchive"/>'s blocksize.
        /// </summary>
        public const ushort DefaultBlockSize = 3;

        public MpqArchiveCreateOptions()
        {
            BlockSize = DefaultBlockSize;
            HashTableSize = null;
            WriteArchiveFirst = true;
            SignatureCreateMode = null;
            SignaturePrivateKey = null;
            ListFileCreateMode = null;
            AttributesCreateMode = null;
            AttributesFlags = AttributesFlags.Crc32 | AttributesFlags.DateTime;
        }

        /// <summary>
        /// The size of blocks in compressed files, which is used to enable seeking.
        /// </summary>
        public ushort BlockSize { get; set; }

        /// <summary>
        /// The desired size of the <see cref="BlockTable"/>. Larger size decreases the likelihood of hash collisions.
        /// </summary>
        public ushort? HashTableSize { get; set; }

        /// <summary>
        /// If <see langword="true"/>, the archive files will be positioned directly after the header. Otherwise, the hashtable and blocktable will come first.
        /// </summary>
        public bool WriteArchiveFirst { get; set; }

        public MpqFileCreateMode? SignatureCreateMode { get; set; }

        public string? SignaturePrivateKey { get; set; }

        public MpqFileCreateMode? ListFileCreateMode { get; set; }

        public MpqFileCreateMode? AttributesCreateMode { get; set; }

        public AttributesFlags AttributesFlags { get; set; }

        /// <summary>
        /// The maximum number of files (and sectors within a large file) that are compressed concurrently while the archive is created.
        /// Files are always written to the archive in the same order, so the output does not depend on this value.
        /// Set to 1 to do all work on the calling thread. If <see langword="null"/>, <see cref="Environment.ProcessorCount"/> is used.
        /// </summary>
        public int? MaxDegreeOfParallelism { get; set; }
    }
}