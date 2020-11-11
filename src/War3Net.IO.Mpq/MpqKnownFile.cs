// ------------------------------------------------------------------------------
// <copyright file="MpqKnownFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Mpq
{
    public sealed class MpqKnownFile : MpqFile
    {
        private readonly string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqKnownFile"/> class.
        /// </summary>
        internal MpqKnownFile(string fileName, MpqStream mpqStream, MpqFileFlags flags, MpqLocale locale, bool leaveOpen = false)
            : base(MpqHash.GetHashedFileName(fileName), mpqStream, flags, locale, leaveOpen)
        {
            _fileName = fileName;
        }

        public string FileName => _fileName;

        internal override uint HashIndex => MpqHash.GetIndex(_fileName);

        internal override uint HashCollisions => 0;

        protected override uint? EncryptionSeed => MpqEntry.CalculateEncryptionSeed(_fileName, out var encryptionSeed) ? encryptionSeed : null;

        public override string ToString()
        {
            return _fileName;
        }

        protected override void GetTableEntries(MpqArchive mpqArchive, uint index, uint relativeFileOffset, uint compressedSize, uint fileSize, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            mpqEntry = new MpqEntry(_fileName, mpqArchive.HeaderOffset, relativeFileOffset, compressedSize, fileSize, TargetFlags);
            mpqHash = new MpqHash(_fileName, mpqArchive.HashTableMask, Locale, index);
        }
    }
}