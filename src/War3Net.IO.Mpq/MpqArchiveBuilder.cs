// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Mpq
{
    public sealed class MpqArchiveBuilder
    {
        private readonly ushort _originalHashTableSize;
        private readonly List<MpqFile> _originalFiles;
        private readonly List<MpqFile> _modifiedFiles;
        private readonly List<ulong> _removedFiles;

        public MpqArchiveBuilder()
        {
            _originalHashTableSize = 0;
            _originalFiles = new List<MpqFile>();
            _modifiedFiles = new List<MpqFile>();
            _removedFiles = new List<ulong>();
        }

        public MpqArchiveBuilder(MpqArchive originalMpqArchive)
        {
            if (originalMpqArchive is null)
            {
                throw new ArgumentNullException(nameof(originalMpqArchive));
            }

            _originalHashTableSize = (ushort)originalMpqArchive.HashTableSize;
            _originalFiles = new List<MpqFile>(originalMpqArchive.GetMpqFiles());
            _modifiedFiles = new List<MpqFile>();
            _removedFiles = new List<ulong>();
        }

        public void AddFile(MpqFile file)
        {
            _modifiedFiles.Add(file);
        }

        public void RemoveFile(string fileName)
        {
            _removedFiles.Add(MpqHash.GetHashedFileName(fileName));
        }

        public void SaveTo(string fileName)
        {
            using (var stream = File.Create(fileName))
            {
                SaveTo(stream);
            }
        }

        private void SaveTo(Stream stream)
        {
            MpqArchive.Create(stream, GetMpqFiles().ToArray(), _originalHashTableSize).Dispose();
        }

        private IEnumerable<MpqFile> GetMpqFiles()
        {
            return _modifiedFiles.Concat(_originalFiles.Where(originalFile =>
                !_removedFiles.Contains(originalFile.Name) &&
                !_modifiedFiles.Where(modifiedFile => modifiedFile.IsSameAs(originalFile)).Any()));
        }
    }
}