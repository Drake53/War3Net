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
        // TODO: add ctor that takes baseEncryptionSeed, make current ctor throw argumentexception if flags parameter has encryption flag

        private readonly MpqHash _hash; // TODO: only store name1, name2, and mask
        private readonly uint _hashIndex; // position in hashtable
        private readonly uint _hashCollisions; // possible amount of collisions this unknown file had in old archive

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqUnknownFile"/> class.
        /// </summary>
        public MpqUnknownFile(Stream? sourceStream, MpqFileFlags flags, MpqHash mpqHash, uint hashIndex, uint hashCollisions)
            : base(sourceStream, flags, mpqHash.Locale)
        {
            if (mpqHash.Mask == 0)
            {
                throw new ArgumentException("Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash));
            }

            _hash = mpqHash;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
        }

        public uint Name1 => _hash.Name1;

        public uint Name2 => _hash.Name2;

        public uint Mask => _hash.Mask;

        internal override uint HashIndex => _hashIndex;

        internal override uint HashCollisions => _hashCollisions;

        protected override uint EncryptionSeed => throw new NotImplementedException();

        internal override bool Equals(MpqFile other)
        {
            throw new NotImplementedException();
        }
    }
}