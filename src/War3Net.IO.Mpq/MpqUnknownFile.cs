// ------------------------------------------------------------------------------
// <copyright file="MpqUnknownFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public sealed class MpqUnknownFile : MpqFile
    {
        private readonly MpqHash _hash; // TODO: only store name1, name2, and mask
        private readonly uint _hashIndex;
        private readonly uint _hashCollisions;
        private readonly uint _encryptionSeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqUnknownFile"/> class.
        /// </summary>
        public MpqUnknownFile(Stream? sourceStream, MpqFileFlags flags, MpqHash mpqHash, uint hashIndex, uint hashCollisions, uint? encryptionSeed = null)
            : base(sourceStream, flags, mpqHash.Locale)
        {
            if (mpqHash.Mask == 0)
            {
                throw new ArgumentException("Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash));
            }

            if (flags.HasFlag(MpqFileFlags.Encrypted) && encryptionSeed is null)
            {
                throw new ArgumentException($"Cannot encrypt an {nameof(MpqUnknownFile)} without an encryption seed.", nameof(flags));
            }

            _hash = mpqHash;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
            _encryptionSeed = encryptionSeed ?? 0;
        }

        public uint Name1 => _hash.Name1;

        public uint Name2 => _hash.Name2;

        public uint Mask => _hash.Mask;

        internal override uint HashIndex => _hashIndex;

        internal override uint HashCollisions => _hashCollisions;

        protected override uint EncryptionSeed => _encryptionSeed;

        internal override bool Equals(MpqFile other)
        {
            throw new NotImplementedException();
        }
    }
}