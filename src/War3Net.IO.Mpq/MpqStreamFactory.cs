// ------------------------------------------------------------------------------
// <copyright file="MpqStreamFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.IO.MemoryMappedFiles;

namespace War3Net.IO.Mpq
{
    public static class MpqStreamFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="archive">The archive from which to load a file.</param>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        internal static MpqStream FromArchive(MpqArchive archive, MpqEntry entry)
        {
            return archive.MemoryMappedFile is not null
                ? FromStream(archive.MemoryMappedFile.CreateViewStream(entry.FilePosition, entry.CompressedSize, MemoryMappedFileAccess.Read), entry, archive.BlockSize, leaveOpen: false)
                : FromStream(archive.BaseStream, entry, archive.BlockSize, leaveOpen: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="baseStream">The <see cref="MpqArchive"/>'s stream.</param>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        /// <param name="blockSize">The <see cref="MpqArchive.BlockSize"/>.</param>
        /// <param name="leaveOpen">If <see langword="false"/>, the <paramref name="baseStream"/> will be closed when the returned <see cref="MpqStream"/> is closed.</param>
        internal static MpqStream FromStream(Stream baseStream, MpqEntry entry, int blockSize, bool leaveOpen = false)
        {
            return new MpqStream(entry, baseStream, blockSize, leaveOpen);
        }

        internal static MpqStream FromStream(Stream baseStream, string? fileName, bool leaveOpen = false)
        {
            var entry = new MpqEntry(fileName, 0, 0, (uint)baseStream.Length, (uint)baseStream.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit);

            return FromStream(baseStream, entry, 0, leaveOpen);
        }
    }
}