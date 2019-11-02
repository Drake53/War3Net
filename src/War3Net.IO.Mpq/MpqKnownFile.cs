// ------------------------------------------------------------------------------
// <copyright file="MpqKnownFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public sealed class MpqKnownFile : MpqFile
    {
        private readonly bool _isOriginalStream;
        private readonly string _fileName;
        private readonly long? _filePos;
        private readonly uint? _fileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqKnownFile"/> class.
        /// </summary>
        public MpqKnownFile(string fileName, Stream? sourceStream, MpqFileFlags flags, MpqLocale locale, bool leaveOpen = false)
            : base(MpqHash.GetHashedFileName(fileName), sourceStream, flags, locale, leaveOpen)
        {
            _isOriginalStream = false;
            _fileName = fileName;
            _filePos = null;
            _fileSize = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqKnownFile"/> class.
        /// </summary>
        public MpqKnownFile(string fileName, Stream? sourceStream, MpqFileFlags flags, MpqLocale locale, long? filePos, uint? fileSize, bool leaveOpen = false)
            : base(MpqHash.GetHashedFileName(fileName), sourceStream, flags, locale, leaveOpen)
        {
            _isOriginalStream = true;
            _fileName = fileName;
            _filePos = filePos;
            _fileSize = fileSize;

            if (flags.HasFlag(MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey) && (filePos is null || fileSize is null))
            {
                throw new ArgumentNullException($"Cannot determine the encryption seed used, because {nameof(filePos)} and/or {nameof(fileSize)} are null.");
            }
        }

        public string FileName => _fileName;

        internal override bool IsOriginalStream => _isOriginalStream;

        internal override uint HashIndex => MpqHash.GetIndex(_fileName);

        internal override uint HashCollisions => 0;

        internal override long? FilePos => _filePos;

        internal override uint? FileSize => _fileSize;

        protected override uint? EncryptionSeed => MpqEntry.CalculateEncryptionSeed(_fileName);
    }
}