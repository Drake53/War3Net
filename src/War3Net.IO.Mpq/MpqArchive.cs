// ------------------------------------------------------------------------------
// <copyright file="MpqArchive.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1710 // Identifiers should have correct suffix

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if NETCOREAPP3_0
using System.Diagnostics.CodeAnalysis;
#endif

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// Represents a MoPaQ file, that is used to archive files.
    /// </summary>
    public sealed class MpqArchive : IDisposable, IEnumerable<MpqEntry>
    {
        /// <summary>
        /// Default value for an <see cref="MpqArchive"/>'s blocksize.
        /// </summary>
        public const ushort DefaultBlockSize = 3;

        // The MPQ header will always start at an offset aligned to 512 bytes.
        private const int PreArchiveAlignBytes = 0x200;
        private const int BlockSizeModifier = 0x200;

        private readonly Stream _baseStream;
        private readonly long _headerOffset;
        private readonly int _blockSize;
        private readonly bool _archiveFollowsHeader;

        private readonly MpqHeader _mpqHeader;
        private readonly HashTable _hashTable;
        private readonly BlockTable _blockTable;

        private bool _isStreamOwner = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqArchive"/> class.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> from which to load an <see cref="MpqArchive"/>.</param>
        /// <param name="loadListfile">If true, automatically execute <see cref="AddListfileFilenames()"/> after the <see cref="MpqArchive"/> is initialized.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> is null.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public MpqArchive(Stream sourceStream, bool loadListfile = false)
        {
            _baseStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));

            if (!TryLocateMpqHeader(_baseStream, out var mpqHeader, out _headerOffset))
            {
                throw new MpqParserException("Unable to locate MPQ header.");
            }

            _mpqHeader = mpqHeader;
            _blockSize = BlockSizeModifier << _mpqHeader.BlockSize;
            _archiveFollowsHeader = _mpqHeader.IsArchiveAfterHeader();

            using (var reader = new BinaryReader(_baseStream, new UTF8Encoding(), true))
            {
                // Load hash table
                _baseStream.Seek(_mpqHeader.HashTablePosition, SeekOrigin.Begin);
                _hashTable = new HashTable(reader, _mpqHeader.HashTableSize);

                // Load entry table
                _baseStream.Seek(_mpqHeader.BlockTablePosition, SeekOrigin.Begin);
                _blockTable = new BlockTable(reader, _mpqHeader.BlockTableSize, (uint)_headerOffset);
            }

            if (loadListfile)
            {
                AddListfileFilenames();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqArchive"/> class.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> containing pre-archive data. Can be null.</param>
        /// <param name="inputFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="hashTableSize">The desired size of the <see cref="BlockTable"/>. Larger size decreases the likelihood of hash collisions.</param>
        /// <param name="blockSize">The size of blocks in compressed files, which is used to enable seeking.</param>
        /// <param name="writeArchiveFirst">If true, the archive files will be positioned directly after the header. Otherwise, the hashtable and blocktable will come first.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mpqFiles"/> collection is null.</exception>
        public MpqArchive(Stream? sourceStream, IEnumerable<MpqFile> inputFiles, ushort? hashTableSize = null, ushort blockSize = DefaultBlockSize, bool writeArchiveFirst = true)
        {
            _baseStream = AlignStream(sourceStream);

            _headerOffset = _baseStream.Position;
            _blockSize = BlockSizeModifier << blockSize;
            _archiveFollowsHeader = writeArchiveFirst;

            var mpqFiles = inputFiles?.ToList() ?? throw new ArgumentNullException(nameof(inputFiles));
            var fileCount = (uint)mpqFiles.Count;

            _hashTable = new HashTable(Math.Max(hashTableSize ?? fileCount * 8, fileCount));
            _blockTable = new BlockTable();

            using (var writer = new BinaryWriter(_baseStream, new UTF8Encoding(false, true), true))
            {
                // Skip the MPQ header, since its contents will be calculated afterwards.
                writer.Seek((int)MpqHeader.Size, SeekOrigin.Current);

                // Write Archive
                var fileIndex = 0U;
                var fileOffset = _archiveFollowsHeader ? MpqHeader.Size : throw new NotImplementedException();

                // var gaps = new List<(long Start, long Length)>();
                var endOfStream = _baseStream.Position;

                // Find files that cannot be decrypted, and need to have a specific position in the archive, because that position is used to calculate the encryption seed.
                var mpqFixedPositionFiles = mpqFiles.Where(mpqFile => mpqFile.IsFilePositionFixed).OrderBy(mpqFile => mpqFile.MpqStream.FilePosition).ToArray();
                if (mpqFixedPositionFiles.Length > 0)
                {
                    if (mpqFixedPositionFiles.First()!.MpqStream.FilePosition < 0)
                    {
                        throw new NotSupportedException($"Cannot place files in front of the header.");
                    }

                    foreach (var mpqFixedPositionFile in mpqFixedPositionFiles)
                    {
                        var position = mpqFixedPositionFile.MpqStream.FilePosition;
                        if (position < endOfStream)
                        {
                            throw new ArgumentException($"Fixed position files overlap with each other and/or the header. Archive cannot be created.", nameof(inputFiles));
                        }

                        if (position > endOfStream)
                        {
                            var gapSize = position - endOfStream;
                            // gaps.Add((endOfStream, gapSize));
                            writer.Seek((int)gapSize, SeekOrigin.Current);
                        }

                        mpqFixedPositionFile.AddToArchive(this, fileIndex, out var mpqEntry, out var mpqHash);
                        var hashTableEntries = _hashTable.Add(mpqHash, mpqFixedPositionFile.HashIndex, mpqFixedPositionFile.HashCollisions);
                        for (var i = 0; i < hashTableEntries; i++)
                        {
                            _blockTable.Add(mpqEntry);
                        }

                        mpqFixedPositionFile.Dispose();

                        fileIndex += hashTableEntries;
                        endOfStream = _baseStream.Position;
                    }
                }

                mpqFiles.RemoveAll(mpqFile => mpqFile.IsFilePositionFixed);
                foreach (var mpqFile in mpqFiles)
                {
                    // TODO: insert files into the gaps
                    // need to know compressed size of file first, and if file is also encrypted with blockoffsetadjustedkey, encryption needs to happen after gap selection
                    // therefore, can't use current AddToArchive method, which does both compression and encryption at same time

                    // var availableGaps = gaps.Where(gap => gap.Length >= )
                    var selectedPosition = endOfStream;
                    var selectedGap = false;
                    _baseStream.Position = selectedPosition;

                    mpqFile.AddToArchive(this, fileIndex, out var mpqEntry, out var mpqHash);
                    var hashTableEntries = _hashTable.Add(mpqHash, mpqFile.HashIndex, mpqFile.HashCollisions);
                    for (var i = 0; i < hashTableEntries; i++)
                    {
                        _blockTable.Add(mpqEntry);
                    }

                    mpqFile.Dispose();

                    fileIndex += hashTableEntries;
                    if (!selectedGap)
                    {
                        endOfStream = _baseStream.Position;
                    }
                }

                _baseStream.Position = endOfStream;
                _hashTable.WriteTo(writer);
                _blockTable.WriteTo(writer);

                /*if (!_archiveFollowsHeader)
                {
                    foreach (var mpqFile in mpqFiles)
                    {
                        mpqFile.WriteTo(writer, true);
                    }
                }*/

                writer.Seek((int)_headerOffset, SeekOrigin.Begin);

                _mpqHeader = new MpqHeader((uint)(endOfStream - fileOffset), _hashTable.Size, _blockTable.Size, blockSize, _archiveFollowsHeader);
                _mpqHeader.WriteTo(writer);
            }
        }

        /// <summary>
        /// Gets the <see cref="MpqHeader"/> of this <see cref="MpqArchive"/>.
        /// </summary>
        public MpqHeader Header => _mpqHeader;

        /// <summary>
        /// Gets the size of the <see cref="BlockTable"/>.
        /// </summary>
        public int Count => (int)_blockTable.Size;

        /// <summary>
        /// Gets the stream that represents this <see cref="MpqArchive"/>.
        /// </summary>
        internal Stream BaseStream => _baseStream;

        /// <summary>
        /// Gets the length (in bytes) of blocks in compressed files.
        /// </summary>
        internal int BlockSize => _blockSize;

        internal uint HashTableSize => _hashTable.Size;

        internal uint HashTableMask => _hashTable.Mask;

        internal uint HeaderOffset => (uint)_headerOffset;

        /// <summary>
        /// Retrieves the <see cref="MpqEntry"/> at the given <paramref name="index"/> of the archive's <see cref="BlockTable"/>.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="FileExists(string, out int)"/> method to get the index of a certain <see cref="MpqEntry"/>.
        /// </remarks>
        /// <param name="index">The <paramref name="index"/> of the <see cref="MpqEntry"/> in the <see cref="BlockTable"/>.</param>
        /// <returns>The <see cref="MpqEntry"/> at the given <paramref name="index"/> of the <see cref="BlockTable"/>.</returns>
        public MpqEntry this[int index] => _blockTable[index];

        /// <summary>
        /// Opens an existing <see cref="MpqArchive"/> for reading.
        /// </summary>
        /// <param name="path">The <see cref="MpqArchive"/> to open.</param>
        /// <param name="loadListfile">If true, automatically execute <see cref="AddListfileFilenames()"/> after the <see cref="MpqArchive"/> is initialized.</param>
        /// <returns>An <see cref="MpqArchive"/> opened from the specified <paramref name="path"/>.</returns>
        /// <exception cref="IOException">Thrown when unable to create a <see cref="FileStream"/> from the given <paramref name="path"/>.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public static MpqArchive Open(string path, bool loadListfile = false)
        {
            FileStream fileStream;

            try
            {
                fileStream = File.OpenRead(path);
            }
            catch (Exception exception)
            {
                throw new IOException($"Failed to open the {nameof(MpqArchive)} at {path}", exception);
            }

            return Open(fileStream, loadListfile);
        }

        /// <summary>
        /// Opens an existing <see cref="MpqArchive"/> for reading.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> from which to load an <see cref="MpqArchive"/>.</param>
        /// <param name="loadListfile">If true, automatically execute <see cref="AddListfileFilenames()"/> after the <see cref="MpqArchive"/> is initialized.</param>
        /// <returns>An <see cref="MpqArchive"/> opened from the specified <paramref name="sourceStream"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> is null.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public static MpqArchive Open(Stream sourceStream, bool loadListfile = false)
        {
            return new MpqArchive(sourceStream, loadListfile);
        }

        /// <summary>
        /// Creates a new <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="path">The path and name of the <see cref="MpqArchive"/> to create.</param>
        /// <param name="mpqFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="hashTableSize">The desired size of the <see cref="BlockTable"/>. Larger size decreases the likelihood of hash collisions.</param>
        /// <param name="blockSize">The size of blocks in compressed files, which is used to enable seeking.</param>
        /// <returns>An <see cref="MpqArchive"/> created as a new file at the specified <paramref name="path"/>.</returns>
        /// <exception cref="IOException">Thrown when unable to create a <see cref="FileStream"/> from the given <paramref name="path"/>.</exception>
        public static MpqArchive Create(string path, ICollection<MpqFile> mpqFiles, ushort? hashTableSize = null, ushort blockSize = DefaultBlockSize)
        {
            FileStream fileStream;

            try
            {
                fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            catch (Exception exception)
            {
                throw new IOException($"Failed to create a {nameof(FileStream)} at {path}", exception);
            }

            return Create(fileStream, mpqFiles, hashTableSize, blockSize);
        }

        /// <summary>
        /// Creates a new <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> containing pre-archive data. Can be null.</param>
        /// <param name="mpqFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="hashTableSize">The desired size of the <see cref="BlockTable"/>. Larger size decreases the likelihood of hash collisions.</param>
        /// <param name="blockSize">The size of blocks in compressed files, which is used to enable seeking.</param>
        /// <returns>An <see cref="MpqArchive"/> that is created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mpqFiles"/> collection is null.</exception>
        public static MpqArchive Create(Stream? sourceStream, ICollection<MpqFile> mpqFiles, ushort? hashTableSize = null, ushort blockSize = DefaultBlockSize)
        {
            return new MpqArchive(sourceStream, mpqFiles, hashTableSize, blockSize);
        }

        /// <summary>
        /// Repairs corrupted values in an <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="path">Path to the archive file.</param>
        /// <returns>A stream containing the repaired archive.</returns>
        public static MemoryStream Restore(string path)
        {
            using var sourceStream = File.OpenRead(path);
            return Restore(sourceStream);
        }

        /// <summary>
        /// Repairs corrupted values in an <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="sourceStream">The stream containing the archive that needs to be repaired.</param>
        /// <param name="leaveOpen">If false, the given <paramref name="sourceStream"/> will be disposed at the end of this method.</param>
        /// <returns>A stream containing the repaired archive.</returns>
        public static MemoryStream Restore(Stream sourceStream, bool leaveOpen = false)
        {
            if (sourceStream is null)
            {
                throw new ArgumentNullException(nameof(sourceStream));
            }

            if (!TryLocateMpqHeader(sourceStream, out var mpqHeader, out var headerOffset))
            {
                throw new MpqParserException($"Unable to locate MPQ header.");
            }

            if (mpqHeader.MpqVersion != 0)
            {
                throw new MpqParserException($"MPQ format version {mpqHeader.MpqVersion} is not supported");
            }

            var memoryStream = new MemoryStream();
            using (var writer = new BinaryWriter(memoryStream, new UTF8Encoding(false, true), true))
            {
                // Skip the MPQ header, since its contents will be calculated afterwards.
                writer.Seek((int)MpqHeader.Size, SeekOrigin.Current);

                var archiveSize = 0U;
                var hashTableEntries = mpqHeader.HashTableSize;
                var blockTableEntries = mpqHeader.BlockTableSize > MpqTable.MaxSize
                    ? mpqHeader.IsArchiveAfterHeader()
                        ? mpqHeader.BlockTablePosition < mpqHeader.HeaderOffset
                            ? (mpqHeader.HeaderOffset - mpqHeader.BlockTablePosition) / MpqEntry.Size
                            : (uint)(sourceStream.Length - sourceStream.Position) / MpqEntry.Size
                        : throw new MpqParserException($"Unable to determine true BlockTable size.")
                    : mpqHeader.BlockTableSize;

                var hashTable = (HashTable?)null;
                var blockTable = (BlockTable?)null;

                using (var reader = new BinaryReader(sourceStream, new UTF8Encoding(), true))
                {
                    // Load hash table
                    sourceStream.Seek(mpqHeader.HashTablePosition, SeekOrigin.Begin);
                    hashTable = new HashTable(reader, hashTableEntries);

                    // Load entry table
                    sourceStream.Seek(mpqHeader.BlockTablePosition, SeekOrigin.Begin);
                    blockTable = new BlockTable(reader, blockTableEntries, (uint)headerOffset);

                    // Load archive files
                    for (var i = 0; i < blockTable.Size; i++)
                    {
                        var entry = blockTable[i];
                        if ((entry.Flags & MpqFileFlags.Garbage) == 0)
                        {
                            var size = entry.CompressedSize;
                            var flags = entry.Flags;

                            if (entry.IsEncrypted && entry.Flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey))
                            {
                                // To prevent encryption seed becoming incorrect, save file uncompressed and unencrypted.
                                var pos = sourceStream.Position;
                                using (var mpqStream = new MpqStream(entry, sourceStream, BlockSizeModifier << mpqHeader.BlockSize))
                                {
                                    mpqStream.CopyTo(memoryStream);
                                }

                                sourceStream.Position = pos + size;

                                size = entry.FileSize;
                                flags = entry.Flags & ~(MpqFileFlags.Compressed | MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey);
                            }
                            else
                            {
                                sourceStream.Position = entry.FilePosition;
                                writer.Write(reader.ReadBytes((int)size));
                            }

                            blockTable[i] = new MpqEntry(null, 0, MpqHeader.Size + archiveSize, size, entry.FileSize, flags);
                            archiveSize += size;
                        }
                        else
                        {
                            blockTable[i] = new MpqEntry(null, 0, MpqHeader.Size + archiveSize, 0, 0, 0);
                        }
                    }
                }

                // Fix invalid block indices and locales.
                for (var i = 0; i < hashTable.Size; i++)
                {
                    var hash = hashTable[i];
                    if (!hash.IsEmpty && !hash.IsDeleted && hash.BlockIndex > BlockTable.MaxSize)
                    {
                        // TODO: don't force neutral locale if another MpqHash exists with the same Name1 and Name2, and that has the neutral locale
                        hashTable[i] = new MpqHash(hash.Name, MpqLocale.Neutral /*hash.Locale & (MpqLocale)0x00000FFF*/, hash.BlockIndex & (BlockTable.MaxSize - 1), hash.Mask);
                    }
                }

                hashTable.SerializeTo(memoryStream);
                blockTable.SerializeTo(memoryStream);

                writer.Seek(0, SeekOrigin.Begin);
                new MpqHeader(archiveSize, hashTableEntries, blockTableEntries, mpqHeader.BlockSize).WriteTo(writer);
            }

            if (!leaveOpen)
            {
                sourceStream.Dispose();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public MpqEntry GetEntryFromHashTable(uint hashTableIndex)
        {
            if (hashTableIndex >= _hashTable.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(hashTableIndex));
            }

            var mpqHash = _hashTable[hashTableIndex];
            if (mpqHash.IsEmpty || mpqHash.IsDeleted)
            {
                throw new ArgumentException($"The {nameof(MpqHash)} at the given index is {(mpqHash.IsDeleted ? "deleted" : "empty")}.", nameof(hashTableIndex));
            }

            return _blockTable[mpqHash.BlockIndex];
        }

        public bool TryGetEntryFromHashTable(
            uint hashTableIndex,
#if NETCOREAPP3_0
            [NotNullWhen(true)]
#endif
            out MpqEntry? mpqEntry)
        {
            if (hashTableIndex >= _hashTable.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(hashTableIndex));
            }

            var mpqHash = _hashTable[hashTableIndex];
            if (mpqHash.IsEmpty)
            {
                throw new ArgumentException($"The {nameof(MpqHash)} at the given index is empty.", nameof(hashTableIndex));
            }

            if (mpqHash.IsDeleted)
            {
                mpqEntry = null;
                return false;
            }

            mpqEntry = _blockTable[mpqHash.BlockIndex];
            return true;
        }

        /// <summary>
        /// Opens an <see cref="MpqEntry"/> in the <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="filename">The name of the <see cref="MpqEntry"/> to open.</param>
        /// <returns>An <see cref="MpqStream"/> that provides access to the <see cref="MpqEntry"/> corresponding to the given <paramref name="filename"/>.</returns>
        /// <exception cref="FileNotFoundException">Thrown when no <see cref="MpqEntry"/> corresponding to the given <paramref name="filename"/> exists.</exception>
        public MpqStream OpenFile(string filename)
        {
            var entry = FileExists(filename, out var index)
                ? this[index]
                : throw new FileNotFoundException($"File not found: {filename}");

            entry.Filename = filename;

            return new MpqStream(this, entry);
        }

        /// <summary>
        /// Opens an <see cref="MpqEntry"/> in the <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="entry">The <see cref="MpqEntry"/> to open.</param>
        /// <returns>An <see cref="MpqStream"/> that provides access to the <see cref="MpqEntry"/> <paramref name="entry"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the given <paramref name="entry"/> is null.</exception>
        public MpqStream OpenFile(MpqEntry entry)
        {
            return new MpqStream(this, entry ?? throw new ArgumentNullException(nameof(entry)));
        }

        /// <summary>
        /// Executes <see cref="AddFilenames(Stream, bool)"/> using the <see cref="ListFile"/> in this <see cref="MpqArchive"/>, if it exists.
        /// </summary>
        /// <returns>True if a <see cref="ListFile"/> exists, false otherwise.</returns>
        public bool AddListfileFilenames()
        {
            if (!TryAddFilename(ListFile.Key))
            {
                return false;
            }

            using (Stream s = OpenFile(ListFile.Key))
            {
                AddFilenames(s);
            }

            return true;
        }

        /// <summary>
        /// Executes <see cref="AddFilename(string)"/> for every string in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to read filenames.</param>
        /// <param name="leaveOpen">True to leave the <paramref name="stream"/> open after executing this method, false otherwise.</param>
        /// <returns>The amount of strings (including duplicates) from the given <paramref name="stream"/> for which an <see cref="MpqEntry"/> exists in the archive.</returns>
        public int AddFilenames(Stream stream, bool leaveOpen = false)
        {
            var filesFound = 0;
            using (var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, leaveOpen))
            {
                while (!sr.EndOfStream)
                {
                    filesFound += AddFilename(sr.ReadLine() ?? string.Empty);
                }
            }

            return filesFound;
        }

        /// <summary>
        /// Tries to find the <see cref="MpqEntry"/> corresponding to the given <paramref name="filename"/>, and update its <see cref="MpqEntry.Filename"/> if it exists.
        /// </summary>
        /// <param name="filename">The name for which the corresponding <see cref="MpqEntry"/>'s <see cref="MpqEntry.Filename"/> must be updated.</param>
        /// <returns>True if an <see cref="MpqEntry"/> with the given <paramref name="filename"/> exists in this <see cref="MpqArchive"/>, false otherwise.</returns>
        public bool TryAddFilename(string filename)
        {
            var hashes = GetHashEntries(filename);
            var anyHash = false;
            foreach (var hash in hashes)
            {
                anyHash = true;
                _blockTable[hash.BlockIndex].Filename = filename;
            }

            return anyHash;
        }

        public int AddFilename(string filename)
        {
            var hashes = GetHashEntries(filename);
            var hashCount = 0;
            foreach (var hash in hashes)
            {
                hashCount++;
                _blockTable[hash.BlockIndex].Filename = filename;
            }

            return hashCount;
        }

        /// <summary>
        /// Tries to find the <see cref="MpqEntry"/> corresponding to the given <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">The name for which to check if a corresponding <see cref="MpqEntry"/> exists.</param>
        /// <returns>True if an <see cref="MpqEntry"/> with the given <paramref name="filename"/> exists in this <see cref="MpqArchive"/>, false otherwise.</returns>
        public bool FileExists(string? filename)
        {
            return string.IsNullOrEmpty(filename) ? false : TryGetHashEntry(filename, out _);
        }

        /// <summary>
        /// Tries to find the <see cref="MpqEntry"/> corresponding to the given <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">The name for which to check if a corresponding <see cref="MpqEntry"/> exists.</param>
        /// <param name="entryIndex">The index of the found <see cref="MpqEntry"/>, or -1 if there is no entry corresponding to the given <paramref name="filename"/>.</param>
        /// <returns>True if an <see cref="MpqEntry"/> with the given <paramref name="filename"/> exists in this <see cref="MpqArchive"/>, false otherwise.</returns>
        public bool FileExists(string filename, out int entryIndex)
        {
            var exists = TryGetHashEntry(filename, out var hash);

            entryIndex = exists
                ? (int)hash.BlockIndex
                : -1;

            return exists;
        }

        // TODO: set hashCollisions values (currently they're always set to 0)
        public IEnumerable<MpqFile> GetMpqFiles()
        {
            // var pairs = new Dictionary<MpqEntry, (uint index, MpqFile file)>();
            var files = new MpqFile[_blockTable.Size]; // array assumes there's no more than one mpqhash for every mpqentry
            var addedEntries = new HashSet<MpqEntry>();
            var deletedIndices = new Queue<int>();

            for (var hashIndex = 0; hashIndex < _hashTable.Size; hashIndex++)
            {
                var mpqHash = _hashTable[hashIndex];
                if (!mpqHash.IsEmpty)
                {
                    var mpqEntry = mpqHash.IsValidBlockIndex ? _blockTable[mpqHash.BlockIndex] : null;
                    if (mpqEntry != null)
                    {
                        var stream = mpqHash.IsDeleted ? null : OpenFile(mpqEntry);
                        var mpqFile = mpqEntry.Filename is null
                            ? MpqFile.New(stream, mpqHash, (uint)hashIndex, 0, mpqEntry.BaseEncryptionSeed)
                            : MpqFile.New(stream, mpqEntry.Filename);

                        mpqFile.TargetFlags = mpqEntry.Flags & ~MpqFileFlags.Garbage;
                        if (mpqEntry.Filename != null && Enum.IsDefined(typeof(MpqLocale), mpqHash.Locale))
                        {
                            mpqFile.Locale = mpqHash.Locale;
                        }

                        // pairs.Add(mpqEntry, (mpqHash.BlockIndex, mpqFile));
                        // files.Add(mpqHash.BlockIndex, mpqFile);
                        files[mpqHash.BlockIndex] = mpqFile;
                        addedEntries.Add(mpqEntry); // TODO: use returned bool to check 'duplicate' mpqhashes (which have same blockindex)
                    }
                    else
                    {
                        deletedIndices.Enqueue(hashIndex);
                    }
                }
            }

            var blockIndex = 0U;
            foreach (var mpqEntry in this)
            {
                //if (!pairs.ContainsKey(mpqEntry))
                if (!addedEntries.Contains(mpqEntry))
                {
                    var hashIndex = deletedIndices.Dequeue();
                    var mpqHash = _hashTable[hashIndex];
                    var mpqFile = mpqEntry.Filename is null
                        ? MpqFile.New(null, mpqHash, (uint)hashIndex, 0, mpqEntry.BaseEncryptionSeed)
                        : MpqFile.New(null, mpqEntry.Filename);

                    mpqFile.TargetFlags = 0;
                    if (mpqEntry.Filename != null && Enum.IsDefined(typeof(MpqLocale), mpqHash.Locale))
                    {
                        mpqFile.Locale = mpqHash.Locale;
                    }

                    // pairs.Add(mpqEntry, (blockIndex, mpqFile));
                    // files.Add(blockIndex, mpqFile);
                    files[blockIndex] = mpqFile;
                }

                blockIndex++;
            }

            // return pairs.OrderBy(pair => pair.Value.index).Select(pair => pair.Value.file);
            // return files.Values;
            return files;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isStreamOwner)
            {
                _baseStream?.Close();
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_blockTable as IEnumerable).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<MpqEntry> IEnumerable<MpqEntry>.GetEnumerator()
        {
            foreach (var entry in _blockTable)
            {
                yield return entry;
            }
        }

        private static bool TryLocateMpqHeader(
            Stream sourceStream,
#if NETCOREAPP3_0
            [NotNullWhen(true)]
#endif
            out MpqHeader? mpqHeader,
            out long headerOffset)
        {
            sourceStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(sourceStream, new UTF8Encoding(), true))
            {
                for (headerOffset = 0; headerOffset < sourceStream.Length - MpqHeader.Size; headerOffset += PreArchiveAlignBytes)
                {
                    if (reader.ReadUInt32() == MpqHeader.MpqId)
                    {
                        sourceStream.Seek(-4, SeekOrigin.Current);
                        mpqHeader = MpqHeader.FromReader(reader);
                        mpqHeader.HeaderOffset = (uint)headerOffset;
                        return true;
                    }

                    sourceStream.Seek(PreArchiveAlignBytes - 4, SeekOrigin.Current);
                }
            }

            mpqHeader = null;
            headerOffset = -1;
            return false;
        }

        private static Stream AlignStream(Stream? stream, bool leaveOpen = false)
        {
            if (stream is null)
            {
                return new MemoryStream();
            }

            if (!stream.CanWrite)
            {
                var memoryStream = new MemoryStream();

                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(memoryStream);
                if (!leaveOpen)
                {
                    stream.Dispose();
                }

                return memoryStream;
            }

            stream.Seek(0, SeekOrigin.End);
            var i = (uint)stream.Position & (PreArchiveAlignBytes - 1);
            if (i > 0)
            {
                for (; i < PreArchiveAlignBytes; i++)
                {
                    stream.WriteByte(0);
                }
            }

            return stream;
        }

        private bool TryGetHashEntry(string filename, out MpqHash hash)
        {
            var index = StormBuffer.HashString(filename, 0);
            index &= _mpqHeader.HashTableSize - 1;
            var name = MpqHash.GetHashedFileName(filename);

            for (var i = index; i < _hashTable.Size; ++i)
            {
                hash = _hashTable[i];
                if (hash.Name == name)
                {
                    return true;
                }
            }

            for (uint i = 0; i < index; ++i)
            {
                hash = _hashTable[i];
                if (hash.Name == name)
                {
                    return true;
                }
            }

            hash = default;
            return false;
        }

        private IEnumerable<MpqHash> GetHashEntries(string filename)
        {
            var index = StormBuffer.HashString(filename, 0);
            index &= _mpqHeader.HashTableSize - 1;
            var name = MpqHash.GetHashedFileName(filename);

            var foundAnyHash = false;

            for (var i = index; i < _hashTable.Size; ++i)
            {
                var hash = _hashTable[i];
                if (hash.Name == name)
                {
                    yield return hash;
                    foundAnyHash = true;
                }
                else if (hash.IsEmpty && foundAnyHash)
                {
                    yield break;
                }
            }

            for (uint i = 0; i < index; ++i)
            {
                var hash = _hashTable[i];
                if (hash.Name == name)
                {
                    yield return hash;
                    foundAnyHash = true;
                }
                else if (hash.IsEmpty && foundAnyHash)
                {
                    yield break;
                }
            }
        }

        /*private int TryGetHashEntry(int entryIndex, out MpqHash hash)
        {
            for (var i = 0; i < _hashTable.Size; i++)
            {
                if (_hashTable[i].BlockIndex == entryIndex)
                {
                    hash = _hashTable[i];
                    return i;
                }
            }

            hash = MpqHash.NULL;
            return -1;
        }*/

        /*private bool VerifyHeader()
        {
            return _mpqHeader.HashTableSize == _hashTable.Size
                && _mpqHeader.BlockTableSize == _blockTable.Size
                && _mpqHeader.BlockSize == _blockSize >> BlockSizeModifier;
        }*/

        /*private uint FindCollidingHashEntries( uint hashIndex, bool returnOnUnknown )
        {
            var count = (uint)0;
            var initial = hashIndex;
            for ( ; hashIndex >= 0; count++ )
            {
                if ( _hashtable[--hashIndex].IsEmpty() )
                {
                    return count;
                }
                else if ( returnOnUnknown && _blocktable[_hashtable[hashIndex].BlockIndex].Filename == null )
                {
                    return count;
                }
            }
            hashIndex = HashEntryMask;
            for ( ; hashIndex > initial; count++ )
            {
                if ( _hashtable[--hashIndex].IsEmpty() )
                {
                    return count;
                }
                else if ( returnOnUnknown && _blocktable[_hashtable[hashIndex].BlockIndex].Filename == null )
                {
                    return count;
                }
            }
            return count;
        }*/
    }
}