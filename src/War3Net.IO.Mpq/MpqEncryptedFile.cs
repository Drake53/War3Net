// ------------------------------------------------------------------------------
// <copyright file="MpqEncryptedFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public sealed class MpqEncryptedFile : MpqUnknownFile
    {
        private readonly long _filePos;
        private readonly uint _fileSize;

        internal MpqEncryptedFile(Stream? originalStream, MpqFileFlags flags, MpqHash mpqHash, uint hashIndex, uint hashCollisions, long filePos, uint fileSize)
            : base(originalStream, flags, mpqHash, hashIndex, hashCollisions)
        {
            _filePos = filePos;
            _fileSize = fileSize;
        }

        public long FilePos => _filePos;

        public uint FileSize => _fileSize;

        internal override bool IsOriginalStream => true;
    }
}