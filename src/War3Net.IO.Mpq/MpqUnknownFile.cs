// ------------------------------------------------------------------------------
// <copyright file="MpqUnknownFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Mpq
{
    public class MpqUnknownFile : MpqFile
    {
        private readonly uint _hashMask;
        private readonly uint _hashIndex;
        private readonly uint _hashCollisions;
        private readonly uint? _encryptionSeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqUnknownFile"/> class.
        /// </summary>
        internal MpqUnknownFile(MpqStream mpqStream, MpqFileFlags flags, MpqHash mpqHash, uint hashIndex, uint hashCollisions, uint? encryptionSeed = null)
            : base(mpqHash.Name, mpqStream, flags, mpqHash.Locale, false)
        {
            if (mpqHash.Mask == 0)
            {
                throw new ArgumentException("Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash));
            }

            if (flags.HasFlag(MpqFileFlags.Encrypted) && encryptionSeed is null)
            {
                throw new ArgumentException($"Cannot encrypt an {nameof(MpqUnknownFile)} without an encryption seed.", nameof(flags));
            }

            _hashMask = mpqHash.Mask;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
            _encryptionSeed = encryptionSeed;
        }

        public uint Mask => _hashMask;

        internal override uint HashIndex => _hashIndex;

        internal override uint HashCollisions => _hashCollisions;

        protected override uint? EncryptionSeed => _encryptionSeed;

        public MpqKnownFile TryAsKnownFile(string fileName)
        {
            // TODO: if filename matches Name, return MpqKnownFile, otherwise return null
            throw new NotImplementedException();
        }

        protected override void GetTableEntries(MpqArchive mpqArchive, uint index, uint relativeFileOffset, uint compressedSize, uint fileSize, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            mpqEntry = new MpqEntry(null, mpqArchive.HeaderOffset, relativeFileOffset, compressedSize, fileSize, TargetFlags);
            mpqHash = new MpqHash(Name, Locale, index, Mask);
        }
    }
}