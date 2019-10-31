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
        private readonly string _fileName;
        private readonly bool _isOriginalStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqKnownFile"/> class.
        /// </summary>
        public MpqKnownFile(string fileName, Stream? sourceStream, MpqFileFlags flags, MpqLocale locale, bool isOriginalStream = false)
            : base(sourceStream, flags, locale)
        {
            _fileName = fileName;
            _isOriginalStream = isOriginalStream;

            if (isOriginalStream && flags.HasFlag(MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey))
            {
                throw new ArgumentException("Cannot determine the encryption seed used in the original stream, because the filepos and filesize values that were used to adjust the base seed are unknown.");
            }
        }

        public string FileName => _fileName;

        internal override bool IsOriginalStream => _isOriginalStream;

        internal override uint HashIndex => MpqHash.GetIndex(_fileName);

        internal override uint HashCollisions => 0;

        protected override uint? EncryptionSeed => MpqEntry.CalculateEncryptionSeed(_fileName);

        internal override bool Equals(MpqFile other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_fileName, ((MpqKnownFile)other)._fileName) == 0;
        }
    }
}