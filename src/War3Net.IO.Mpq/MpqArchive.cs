// ------------------------------------------------------------------------------
// <copyright file="MpqArchive.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.IO.Mpq.Extensions;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// Represents a MoPaQ file, that is used to archive files.
    /// </summary>
    public sealed class MpqArchive : IDisposable, IEnumerable<MpqEntry>
    {
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

        private readonly bool _isStreamOwner;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqArchive"/> class.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> from which to load an <see cref="MpqArchive"/>.</param>
        /// <param name="loadListFile">If <see langword="true"/>, automatically add filenames from the <see cref="MpqArchive"/>'s <see cref="ListFile"/> after the archive is initialized.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> is <see langword="null"/>.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public MpqArchive(Stream sourceStream, bool loadListFile = false)
        {
            _isStreamOwner = true;
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

            if (loadListFile)
            {
                if (TryOpenFile(ListFile.FileName, out var listFileStream))
                {
                    using var listFileReader = new StreamReader(listFileStream, leaveOpen: false);
                    AddFileNames(listFileReader.ReadListFile().FileNames);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqArchive"/> class.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> containing pre-archive data. Can be <see langword="null"/>.</param>
        /// <param name="inputFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="createOptions"></param>
        /// <param name="leaveOpen">If <see langword="false"/>, the given <paramref name="sourceStream"/> will be disposed when the <see cref="MpqArchive"/> is disposed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mpqFiles"/> collection is <see langword="null"/>.</exception>
        public MpqArchive(Stream? sourceStream, IEnumerable<MpqFile> inputFiles, MpqArchiveCreateOptions createOptions, bool leaveOpen = false)
        {
            if (inputFiles is null)
            {
                throw new ArgumentNullException(nameof(inputFiles));
            }

            if (createOptions is null)
            {
                throw new ArgumentNullException(nameof(createOptions));
            }

            _isStreamOwner = !leaveOpen;
            _baseStream = AlignStream(sourceStream);

            _headerOffset = _baseStream.Position;
            _blockSize = BlockSizeModifier << createOptions.BlockSize;
            _archiveFollowsHeader = createOptions.WriteArchiveFirst;

            var listFileName = ListFile.FileName.GetStringHash();
            var attributesName = Attributes.FileName.GetStringHash();

            var listFileCreateMode = createOptions.ListFileCreateMode.GetValueOrDefault(MpqFileCreateMode.Overwrite);
            var attributesCreateMode = createOptions.AttributesCreateMode.GetValueOrDefault(MpqFileCreateMode.Overwrite);
            var haveListFile = false;
            var haveAttributes = false;
            var mpqFiles = new HashSet<MpqFile>(MpqFileComparer.Default);
            foreach (var mpqFile in inputFiles)
            {
                if (mpqFile is MpqOrphanedFile)
                {
                    continue;
                }

                if (mpqFile.Name == listFileName)
                {
                    if (listFileCreateMode.HasFlag(MpqFileCreateMode.RemoveFlag))
                    {
                        continue;
                    }
                    else
                    {
                        haveListFile = true;
                    }
                }

                if (mpqFile.Name == attributesName)
                {
                    if (attributesCreateMode.HasFlag(MpqFileCreateMode.RemoveFlag))
                    {
                        continue;
                    }
                    else
                    {
                        haveAttributes = true;
                    }
                }

                if (!mpqFiles.Add(mpqFile))
                {
                    // todo: logging?
                }
            }

            var fileCount = (uint)mpqFiles.Count;

            var wantGenerateListFile = !haveListFile && listFileCreateMode.HasFlag(MpqFileCreateMode.AddFlag);
            var listFile = wantGenerateListFile ? new ListFile() : null;
            if (wantGenerateListFile)
            {
                fileCount++;
            }

            var wantGenerateAttributes = !haveAttributes && attributesCreateMode.HasFlag(MpqFileCreateMode.AddFlag);
            var attributes = wantGenerateAttributes ? new Attributes(createOptions) : null;
            if (wantGenerateAttributes)
            {
                fileCount++;
            }

            _hashTable = new HashTable(Math.Max(createOptions.HashTableSize ?? fileCount * 8, fileCount));
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

                void InsertMpqFile(MpqFile mpqFile, bool updateEndOfStream, bool allowMultiple = true)
                {
                    if (listFile is not null && mpqFile is MpqKnownFile knownFile)
                    {
                        listFile.FileNames.Add(knownFile.FileName);
                    }

                    mpqFile.AddToArchive(this, fileIndex, out var mpqEntry, out var mpqHash);
                    var hashTableEntries = _hashTable.Add(mpqHash, mpqFile.HashIndex, mpqFile.HashCollisions);
                    if (!allowMultiple && hashTableEntries > 1)
                    {
                        throw new Exception();
                    }

                    var crc32 = 0;
                    if (attributes is not null && attributes.Flags.HasFlag(AttributesFlags.Crc32) && allowMultiple)
                    {
                        mpqFile.MpqStream.Position = 0;
                        crc32 = new Ionic.Crc.CRC32().GetCrc32(mpqFile.MpqStream);
                    }

                    for (var i = 0; i < hashTableEntries; i++)
                    {
                        _blockTable.Add(mpqEntry);
                        if (attributes is not null)
                        {
                            if (attributes.Flags.HasFlag(AttributesFlags.Crc32))
                            {
                                attributes.Crc32s.Add(crc32);
                            }

                            if (attributes.Flags.HasFlag(AttributesFlags.DateTime))
                            {
                                attributes.DateTimes.Add(DateTime.Now);
                            }

                            if (attributes.Flags.HasFlag(AttributesFlags.Unk0x04))
                            {
                                attributes.Unk0x04s.Add(new byte[16]);
                            }
                        }
                    }

                    mpqFile.Dispose();

                    fileIndex += hashTableEntries;
                    if (updateEndOfStream)
                    {
                        endOfStream = _baseStream.Position;
                    }
                }

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

                        InsertMpqFile(mpqFixedPositionFile, true);
                    }
                }

                foreach (var mpqFile in mpqFiles.Where(mpqFile => !mpqFile.IsFilePositionFixed))
                {
                    // TODO: insert files into the gaps
                    // need to know compressed size of file first, and if file is also encrypted with blockoffsetadjustedkey, encryption needs to happen after gap selection
                    // therefore, can't use current AddToArchive method, which does both compression and encryption at same time

                    // var availableGaps = gaps.Where(gap => gap.Length >= )
                    var selectedPosition = endOfStream;
                    var selectedGap = false;
                    _baseStream.Position = selectedPosition;

                    InsertMpqFile(mpqFile, !selectedGap);
                }

                if (listFile is not null)
                {
                    _baseStream.Position = endOfStream;

                    using var listFileStream = new MemoryStream();
                    using var listFileWriter = new StreamWriter(listFileStream);
                    listFileWriter.WriteListFile(listFile);
                    listFileWriter.Flush();

                    using var listFileMpqFile = MpqFile.New(listFileStream, ListFile.FileName);
                    listFileMpqFile.TargetFlags = MpqFileFlags.Exists | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey;
                    InsertMpqFile(listFileMpqFile, true);
                }

                if (attributes is not null)
                {
                    _baseStream.Position = endOfStream;

                    if (attributes.Flags.HasFlag(AttributesFlags.Crc32))
                    {
                        attributes.Crc32s.Add(0);
                    }

                    if (attributes.Flags.HasFlag(AttributesFlags.DateTime))
                    {
                        attributes.DateTimes.Add(DateTime.Now);
                    }

                    if (attributes.Flags.HasFlag(AttributesFlags.Unk0x04))
                    {
                        attributes.Unk0x04s.Add(new byte[16]);
                    }

                    using var attributesStream = new MemoryStream();
                    using var attributesWriter = new BinaryWriter(attributesStream);
                    attributesWriter.Write(attributes);
                    attributesWriter.Flush();

                    using var attributesMpqFile = MpqFile.New(attributesStream, Attributes.FileName);
                    attributesMpqFile.TargetFlags = MpqFileFlags.Exists | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey;
                    InsertMpqFile(attributesMpqFile, true, false);
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

                _mpqHeader = new MpqHeader((uint)_headerOffset, (uint)(endOfStream - fileOffset), _hashTable.Size, _blockTable.Size, createOptions.BlockSize, _archiveFollowsHeader);
                _mpqHeader.WriteTo(writer);
            }
        }

        internal Stream BaseStream => _baseStream;

        internal uint HeaderOffset => (uint)_headerOffset;

        /// <summary>
        /// Gets the length (in bytes) of blocks in compressed files.
        /// </summary>
        internal int BlockSize => _blockSize;

        internal MpqHeader Header => _mpqHeader;

        internal HashTable HashTable => _hashTable;

        internal BlockTable BlockTable => _blockTable;

        internal MpqEntry this[int index] => _blockTable[index];

        /// <summary>
        /// Opens an existing <see cref="MpqArchive"/> for reading.
        /// </summary>
        /// <param name="path">The <see cref="MpqArchive"/> to open.</param>
        /// <param name="loadListFile">If <see langword="true"/>, automatically add filenames from the <see cref="MpqArchive"/>'s <see cref="ListFile"/> after the archive is initialized.</param>
        /// <returns>An <see cref="MpqArchive"/> opened from the specified <paramref name="path"/>.</returns>
        /// <exception cref="IOException">Thrown when unable to create a <see cref="FileStream"/> from the given <paramref name="path"/>.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public static MpqArchive Open(string path, bool loadListFile = false)
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

            return Open(fileStream, loadListFile);
        }

        /// <summary>
        /// Opens an existing <see cref="MpqArchive"/> for reading.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> from which to load an <see cref="MpqArchive"/>.</param>
        /// <param name="loadListFile">If <see langword="true"/>, automatically add filenames from the <see cref="MpqArchive"/>'s <see cref="ListFile"/> after the archive is initialized.</param>
        /// <returns>An <see cref="MpqArchive"/> opened from the specified <paramref name="sourceStream"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> is <see langword="null"/>.</exception>
        /// <exception cref="MpqParserException">Thrown when the <see cref="MpqHeader"/> could not be found, or when the MPQ format version is not 0.</exception>
        public static MpqArchive Open(Stream sourceStream, bool loadListFile = false)
        {
            return new MpqArchive(sourceStream, loadListFile);
        }

        /// <summary>
        /// Creates a new <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="path">The path and name of the <see cref="MpqArchive"/> to create.</param>
        /// <param name="mpqFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="createOptions"></param>
        /// <returns>An <see cref="MpqArchive"/> created as a new file at the specified <paramref name="path"/>.</returns>
        /// <exception cref="IOException">Thrown when unable to create a <see cref="FileStream"/> from the given <paramref name="path"/>.</exception>
        public static MpqArchive Create(string path, IEnumerable<MpqFile> mpqFiles, MpqArchiveCreateOptions createOptions)
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

            return Create(fileStream, mpqFiles, createOptions, false);
        }

        /// <summary>
        /// Creates a new <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="sourceStream">The <see cref="Stream"/> containing pre-archive data. Can be <see langword="null"/>.</param>
        /// <param name="mpqFiles">The <see cref="MpqFile"/>s that should be added to the archive.</param>
        /// <param name="createOptions"></param>
        /// <param name="leaveOpen">If <see langword="false"/>, the given <paramref name="sourceStream"/> will be disposed when the <see cref="MpqArchive"/> is disposed.</param>
        /// <returns>An <see cref="MpqArchive"/> that is created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="mpqFiles"/> collection is <see langword="null"/>.</exception>
        public static MpqArchive Create(Stream? sourceStream, IEnumerable<MpqFile> mpqFiles, MpqArchiveCreateOptions createOptions, bool leaveOpen = false)
        {
            return new MpqArchive(sourceStream, mpqFiles, createOptions, leaveOpen);
        }

        /// <summary>
        /// Repairs corrupted values in an <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="path">Path to the archive file.</param>
        /// <returns>A stream containing the repaired archive.</returns>
        [Obsolete]
        public static MemoryStream Restore(string path)
        {
            using var sourceStream = File.OpenRead(path);
            return Restore(sourceStream);
        }

        /// <summary>
        /// Repairs corrupted values in an <see cref="MpqArchive"/>.
        /// </summary>
        /// <param name="sourceStream">The stream containing the archive that needs to be repaired.</param>
        /// <param name="leaveOpen">If <see langword="false"/>, the given <paramref name="sourceStream"/> will be disposed at the end of this method.</param>
        /// <returns>A stream containing the repaired archive.</returns>
        [Obsolete]
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
                new MpqHeader(0, archiveSize, hashTableEntries, blockTableEntries, mpqHeader.BlockSize).WriteTo(writer);
            }

            if (!leaveOpen)
            {
                sourceStream.Dispose();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Searches the <see cref="MpqArchive"/> for files with the given <paramref name="fileName"/>, and sets the <see cref="MpqEntry.FileName"/> if it was not known before.
        /// </summary>
        /// <param name="fileName">The name for which to check if any corresponding files exist.</param>
        /// <returns>The amount of files in the <see cref="BlockTable"/> with the given <paramref name="fileName"/>.</returns>
        public int AddFileName(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var hashes = GetHashEntries(fileName);
            var fileIndicesFound = new HashSet<uint>();
            foreach (var hash in hashes)
            {
                if (fileIndicesFound.Add(hash.BlockIndex))
                {
                    _blockTable[hash.BlockIndex].FileName = fileName;
                }
            }

            return fileIndicesFound.Count;
        }

        /// <summary>
        /// Searches the <see cref="MpqArchive"/> for files with any of the given <paramref name="fileNames"/>, and sets the <see cref="MpqEntry.FileName"/> if it was not known before.
        /// </summary>
        /// <param name="fileNames">The names for which to check if any corresponding files exist.</param>
        /// <returns>The amount of files in the <see cref="BlockTable"/> with any of the given <paramref name="fileNames"/>.</returns>
        public int AddFileNames(params string[] fileNames)
        {
            return fileNames.Sum(fileName => AddFileName(fileName));
        }

        /// <summary>
        /// Searches the <see cref="MpqArchive"/> for files with any of the given <paramref name="fileNames"/>, and sets the <see cref="MpqEntry.FileName"/> if it was not known before.
        /// </summary>
        /// <param name="fileNames">The names for which to check if any corresponding files exist.</param>
        /// <returns>The amount of files in the <see cref="BlockTable"/> with any of the given <paramref name="fileNames"/>.</returns>
        public int AddFileNames(IEnumerable<string> fileNames)
        {
            return fileNames.Sum(fileName => AddFileName(fileName));
        }

        /// <summary>
        /// Tries to find mpq entries corresponding to the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name for which to check if a corresponding <see cref="MpqEntry"/> exists.</param>
        /// <returns><see langword="true"/> if any file with the given <paramref name="fileName"/> exists, <see langword="false"/> otherwise.</returns>
        public bool FileExists(string? fileName)
        {
            return !string.IsNullOrEmpty(fileName) && AddFileName(fileName) > 0;
        }

        /// <summary>
        /// Opens the first matching <see cref="MpqEntry"/>.
        /// </summary>
        /// <param name="fileName">The name of the <see cref="MpqEntry"/> to open.</param>
        /// <param name="locale">
        /// The locale of the <see cref="MpqEntry"/> to open.
        /// If <see langword="null"/>, any file with the given <paramref name="fileName"/> can be opened.
        /// If <see cref="MpqLocale.Neutral"/>, only files with the neutral locale can be opened.
        /// If not <see cref="MpqLocale.Neutral"/>, only files with the specified <paramref name="locale"/> or <see cref="MpqLocale.Neutral"/> can be opened.
        /// </param>
        /// <param name="orderByBlockIndex">If <see langword="true"/>, the file with the lowest position in the <see cref="BlockTable"/> is returned.</param>
        /// <returns>An <see cref="MpqStream"/> that provides access to the matched <see cref="MpqEntry"/>.</returns>
        /// <exception cref="FileNotFoundException">Thrown when no matching <see cref="MpqEntry"/> exists.</exception>
        public MpqStream OpenFile(string fileName, MpqLocale? locale = null, bool orderByBlockIndex = true)
        {
            return TryOpenFile(fileName, locale, orderByBlockIndex, out var stream) ? stream : throw new FileNotFoundException($"File not found: {fileName}");
        }

        /// <summary>
        /// Opens the given <see cref="MpqEntry"/>.
        /// </summary>
        /// <param name="entry">The <see cref="MpqEntry"/> to open.</param>
        /// <returns>An <see cref="MpqStream"/> that provides access to the given <paramref name="entry"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the given <paramref name="entry"/> is <see langword="null"/>.</exception>
        public MpqStream OpenFile(MpqEntry entry)
        {
            return new MpqStream(this, entry ?? throw new ArgumentNullException(nameof(entry)));
        }

        public bool TryOpenFile(string fileName, [NotNullWhen(true)] out MpqStream stream)
        {
            return TryOpenFile(fileName, null, true, out stream);
        }

        public bool TryOpenFile(string fileName, MpqLocale? locale, [NotNullWhen(true)] out MpqStream stream)
        {
            return TryOpenFile(fileName, locale, true, out stream);
        }

        public bool TryOpenFile(string fileName, bool orderByBlockIndex, [NotNullWhen(true)] out MpqStream stream)
        {
            return TryOpenFile(fileName, null, orderByBlockIndex, out stream);
        }

        public bool TryOpenFile(string fileName, MpqLocale? locale, bool orderByBlockIndex, [NotNullWhen(true)] out MpqStream? stream)
        {
            var entry = GetMpqEntries(fileName, locale, orderByBlockIndex).FirstOrDefault();
            if (entry is not null)
            {
                stream = new MpqStream(this, entry);
                return true;
            }

            stream = null;
            return false;
        }

        public IEnumerable<MpqEntry> GetMpqEntries(string fileName, MpqLocale? locale = null, bool orderByBlockIndex = true)
        {
            var hashes = GetHashEntries(fileName);
            foreach (var hash in orderByBlockIndex ? hashes.OrderBy(mpqHash => mpqHash.BlockIndex) : hashes)
            {
                var entry = _blockTable[hash.BlockIndex];
                entry.FileName = fileName;

                if (!locale.HasValue || locale.Value == hash.Locale)
                {
                    yield return entry;
                }
            }

            if (locale.HasValue && locale.Value != MpqLocale.Neutral)
            {
                foreach (var hash in orderByBlockIndex ? hashes.OrderBy(mpqHash => mpqHash.BlockIndex) : hashes)
                {
                    if (hash.Locale == MpqLocale.Neutral)
                    {
                        yield return _blockTable[hash.BlockIndex];
                    }
                }
            }
        }

        // TODO: set hashCollisions values (currently they're always set to 0)
        public IEnumerable<MpqFile> GetMpqFiles()
        {
            // var pairs = new Dictionary<MpqEntry, (uint index, MpqFile file)>();
            var files = new MpqFile[_blockTable.Size]; // array assumes there's no more than one mpqhash for every mpqentry
            var addedEntries = new HashSet<MpqEntry>();

            for (var hashIndex = 0; hashIndex < _hashTable.Size; hashIndex++)
            {
                var mpqHash = _hashTable[hashIndex];
                if (!mpqHash.IsEmpty)
                {
                    var mpqEntry = mpqHash.IsValidBlockIndex ? _blockTable[mpqHash.BlockIndex] : null;
                    if (mpqEntry != null)
                    {
                        // var stream = mpqHash.IsDeleted ? null : OpenFile(mpqEntry);
                        var stream = OpenFile(mpqEntry);
                        var mpqFile = mpqEntry.FileName is null
                            ? MpqFile.New(stream, mpqHash, (uint)hashIndex, 0, mpqEntry.BaseEncryptionSeed)
                            : MpqFile.New(stream, mpqEntry.FileName, mpqHash.Locale);

                        mpqFile.TargetFlags = mpqEntry.Flags & ~MpqFileFlags.Garbage;

                        files[mpqHash.BlockIndex] = mpqFile;
                        addedEntries.Add(mpqEntry); // TODO: use returned bool to check 'duplicate' mpqhashes (which have same blockindex)
                    }
                }
            }

            for (var i = 0; i < (int)_blockTable.Size; i++)
            {
                var mpqEntry = _blockTable[i];
                if (!addedEntries.Contains(mpqEntry))
                {
                    var mpqFile = MpqFile.New(OpenFile(mpqEntry));
                    mpqFile.TargetFlags = 0;
                    files[i] = mpqFile;
                }
            }

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
        IEnumerator IEnumerable.GetEnumerator() => (_blockTable as IEnumerable).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<MpqEntry> IEnumerable<MpqEntry>.GetEnumerator() => (_blockTable as IEnumerable<MpqEntry>).GetEnumerator();

        internal bool TryGetEntryFromHashTable(
            uint hashTableIndex,
            [NotNullWhen(true)] out MpqEntry? mpqEntry)
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

        private static bool TryLocateMpqHeader(
            Stream sourceStream,
            [NotNullWhen(true)] out MpqHeader? mpqHeader,
            out long headerOffset)
        {
            sourceStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(sourceStream, new UTF8Encoding(), true))
            {
                for (headerOffset = 0; headerOffset <= sourceStream.Length - MpqHeader.Size; headerOffset += PreArchiveAlignBytes)
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

        private IEnumerable<MpqHash> GetHashEntries(string fileName)
        {
            if (!StormBuffer.TryGetHashString(fileName, 0, out var index))
            {
                yield break;
            }

            index &= _mpqHeader.HashTableSize - 1;
            var name = fileName.GetStringHash();

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
    }
}